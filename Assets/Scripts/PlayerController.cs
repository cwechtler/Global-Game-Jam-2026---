using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
public class PlayerController : MonoBehaviour
{
	[SerializeField] private int health = 100;
	[SerializeField] private Vector2 speed = new Vector2(10, 10);
    [SerializeField] private float meleAnimSpeed = 1f;
    [SerializeField] private float projectileSpeed;
	[Space]
	[Tooltip("Skill Prefabs")]
	[SerializeField] private GameObject[] skills;
	[SerializeField] private Transform skillSpawner, skillSpawnPoint, activeSkillContainer;
	[Space]
	[SerializeField] private Transform playerTransform;
	[Space]
	[SerializeField] private CanvasController canvasController;
	[SerializeField] private AttackType activeMaskType;

	public AttackType ActiveMaskType { get => activeMaskType; }
    public enum MoveDir { None, North, South, East, West }
    public MoveDir currentDirection = MoveDir.None;



    public int ExperiencePoints { get => experiencePoints; set => experiencePoints = value; }
	public List<GameObject> InventoryItems { get => inventoryItems; set => inventoryItems = value; }
	public float FireX { get => fireX; }
	public float FireY { get => fireY; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int Health { get => health; set => health = value; }

    private Vector2 FacingDirection;
    private LayerMask enemyLayer;
    public bool isAttacking = false;

    private List<GameObject> inventoryItems = new List<GameObject>();
    private readonly HashSet<int> animNameHashes = new HashSet<int>()
	{
		Animator.StringToHash("HurtNorth"),
		Animator.StringToHash("HurtSouth"),
		Animator.StringToHash("HurtEast"),
		Animator.StringToHash("HurtWest"),
		Animator.StringToHash("AttackNorth"),
		Animator.StringToHash("AttackSouth"),
		Animator.StringToHash("AttackEast"),
		Animator.StringToHash("AttackWest"),
	};

    private Rigidbody2D myRigidbody2D;
	private Animator animator;

	private bool moveHorizontaly, moveVertically;
	private bool isDead = false;

	private GameObject activeSkill;
	private int activeSkillIndex;
	private float firingRate, fireX, fireY;
	private float[] coolDownTimes;
	private float[] timerTimes;
    private float[] nextFireTimes;
    private bool[] skillWasCast;
    private bool[] hasFiredOnce;

    public int experiencePoints;

	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>(true);
        enemyLayer = LayerMask.GetMask("Enemy");

        //SetActiveSkill(0);

        coolDownTimes = new float[skills.Length];
        timerTimes = new float[skills.Length];
        skillWasCast = new bool[skills.Length];
        nextFireTimes = new float[skills.Length];
        hasFiredOnce = new bool[skills.Length];


        for (int i = 0; i < skills.Length; i++)
        {
            SkillConfig skill = skills[i].GetComponent<SkillConfig>();
            coolDownTimes[i] = skill.CoolDownTime;
            canvasController.SetCoolDownTime(i, coolDownTimes[i]);
            canvasController.SetSkillImages(i, skill.SkillImage);
        }
    }

	void Update()
	{
		if (!isDead && !GameController.instance.isPaused) {
			Move();

			SelectSkill();
			Fire();
		}
		if (Input.GetButtonDown("Enter"))
		{
			LevelManager.instance.LoadLevel(LevelManager.MainMenuString);
		}
	}

	public void AddToInventory(GameObject inventoryPrefab) {
		inventoryItems.Add(inventoryPrefab);
		canvasController.AddInventoryItem(inventoryPrefab);
	}

	public void ReduceHealth(int damage)
	{
		if (!isDead) {
            animator.SetTrigger("Hurt");
            SoundManager.instance.PlayHurtClip();

			Health -= damage;
			canvasController.ReduceHealthBar(Health);

			if (Health <= 0) {
				StartCoroutine(PlayerDeath());
			}
		}
	}


    private void Move()
	{
		float inputY = Input.GetAxis("Vertical");
		float inputX = Input.GetAxis("Horizontal");

        if (!EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButton(0))
			{
				Vector3 direction = MousePointerDirection();

				inputX = Mathf.Clamp(direction.x, -1, 1);
				inputY = Mathf.Clamp(direction.y, -1, 1);
			}
		}

        if (inputX != 0 || inputY != 0)
        {
            FacingDirection = new Vector2(inputX, inputY).normalized;
        }
        //Debug.Log("Facing Direction: " + FacingDirection);

        myRigidbody2D.velocity = new Vector2(speed.x * inputX, speed.y * inputY);
		moveHorizontaly = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
		moveVertically = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;

		SetAnimations(inputX, inputY);
		//FlipDirection();
	}

	private IEnumerator PlayerDeath()
	{
		isDead = true;
		myRigidbody2D.isKinematic = true;
		myRigidbody2D.velocity = new Vector3(0, 0, 0);
		//foreach (var animator in animator) {
			animator.SetBool("Death", true);
		//}
		SoundManager.instance.PlayDeathClip();
		yield return new WaitForSeconds(2f);
		LevelManager.instance.LoadLevel(LevelManager.LoseLevelString);
	}

	public void SetActiveSkill(int index) {
		GameController.instance.ActiveSkillIndex = index;
		activeSkillIndex = index;
		activeSkill = skills[index];
		SkillConfig activeSkillSkillConfig = activeSkill.GetComponent<SkillConfig>();
        activeMaskType = activeSkillSkillConfig.AttackType;
        firingRate = activeSkillSkillConfig.FireRate;
		canvasController.UpdateTextColor();
	}

	private void SelectSkill()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetButtonDown("Space"))
		{
			if (activeSkillIndex == skills.Length - 1) activeSkillIndex = 0;
			else activeSkillIndex++;
			SetActiveSkill(activeSkillIndex);
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (activeSkillIndex == 0) activeSkillIndex = skills.Length - 1;
			else activeSkillIndex--;
			SetActiveSkill(activeSkillIndex);
		}

		if (Input.GetButtonDown("Fire1")) {
			SetActiveSkill(0);
        }
		else if (Input.GetButtonDown("Fire2")) {
			SetActiveSkill(1);
        }
		else if (Input.GetButton("Fire3")) {
            //SetActiveSkill(2);
            animator.SetTrigger("Attack");
        }
		else if (Input.GetButtonDown("Jump")) {
			ReduceHealth(0);
            //SetActiveSkill(3);
        }
	}

    private void Fire()
    {
        Vector2 direction = FacingDirection;
        if (direction == Vector2.zero)
            return;

        // Rotate skill spawner to face movement direction
        skillSpawner.eulerAngles =
            new Vector3(0, 0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);

        // Loop through ALL skills the player has
        for (int index = 0; index < skills.Length; index++)
        {
            TryFireSkill(index, direction);
        }

        UpdateSkillCooldowns();
    }

    private void TryFireSkill(int index, Vector2 direction)
    {
        SkillConfig skillConfig = skills[index].GetComponent<SkillConfig>();

        // Fire rate check
        if (Time.time < nextFireTimes[index])
            return;

        // Cooldown check
        if (skillWasCast[index])
            return;

        // FireOnce check
        if (skillConfig.FireOnce && hasFiredOnce[index])
            return;

        // Enemy detection
        bool enemyInRange = false;

        if (skillConfig is ConeSkill cone)
            enemyInRange = ConeHasEnemy(cone, direction);
        else if (skillConfig is RadialSkill radial)
            enemyInRange = RadialHasEnemy(radial);

        else if (skillConfig is ExpandingSkill expanding)
            enemyInRange = ExpandingHasEnemy(expanding);

        else if (skillConfig is ProjectileConfig projectile)
            enemyInRange = ProjectileHasEnemy(projectile, direction);

        if (!enemyInRange)
            return; // Skip firing if no enemy

        if (skillConfig is ConeSkill coneSkill)
        {
            animator.SetTrigger("Attack");
            IsAttacking = true;
            animator.speed = meleAnimSpeed; // fixed attack animation speed
            FireCone(coneSkill, direction);
        }

        // Perform the correct attack based on subclass
        else if (skillConfig is ProjectileConfig projectile)
            FireProjectile(projectile, direction);

        else if (skillConfig is RadialSkill radial)
            FireRadial(radial);

        else if (skillConfig is ExpandingSkill expanding)
            StartCoroutine(FireExpanding(expanding));

        // Set next fire time
        nextFireTimes[index] = Time.time + skillConfig.FireRate;

        // Start cooldown
        skillWasCast[index] = true;

        // Mark FireOnce skills
        if (skillConfig.FireOnce)
            hasFiredOnce[index] = true;
    }

    private bool ConeHasEnemy(ConeSkill config, Vector2 facing)
    {
        float radius = config.Radius;
        float angleLimit = Mathf.Cos(config.Angle * Mathf.Deg2Rad);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (var hit in hits)
        {
            //Debug.Log(hit);
            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            float dot = Vector2.Dot(facing, toTarget);

            if (dot >= angleLimit)
                return true;
        }

        return false;
    }


    private void FireCone(ConeSkill config, Vector2 facing)
    {
        float radius = config.Radius;
        float angleLimit = Mathf.Cos(45f * Mathf.Deg2Rad);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            float dot = Vector2.Dot(facing, toTarget);

            if (dot >= angleLimit)
            {
                if (config.UseAnimationHitTiming)
                    StartCoroutine(DelayedHit(hit, config));
                else
                    hit.GetComponentInParent<Enemy>().reduceHealth(config.GetDamage());
            }

        }
    }
    private IEnumerator DelayedHit(Collider2D hit, ConeSkill config)
    {
        yield return new WaitForSeconds(config.HitDelay); // add this to your config
        hit.GetComponentInParent<Enemy>().reduceHealth(config.GetDamage());
    }


    private bool RadialHasEnemy(RadialSkill config)
    {
        return Physics2D.OverlapCircle(transform.position, config.Radius, enemyLayer);
    }

    private void FireRadial(RadialSkill config)
    {
        Debug.Log("Firing Radial Skill");
        if (config.RadialVFX != null) 
        {
            GameObject vfx = Instantiate(config.RadialVFX, transform.position, Quaternion.identity);
            vfx.transform.SetParent(transform);
        }
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, config.Radius, enemyLayer);

        foreach (var hit in hits)
            hit.GetComponent<Enemy>().reduceHealth(config.GetDamage());
    }

    private bool ExpandingHasEnemy(ExpandingSkill config)
    {
        return Physics2D.OverlapCircle(transform.position, config.StartRadius, enemyLayer);
    }

    private IEnumerator FireExpanding(ExpandingSkill config)
    {
        float radius = 0f;

        while (radius < config.MaxRadius)
        {
            radius += config.ExpandSpeed * Time.deltaTime;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

            foreach (var hit in hits)
                hit.GetComponent<Enemy>().reduceHealth(config.GetDamage());

            yield return null;
        }
    }

    private bool ProjectileHasEnemy(ProjectileConfig config, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, config.Range, enemyLayer);
        return hit.collider != null;
    }

    private void FireProjectile(ProjectileConfig config, Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject proj = Instantiate(config.ProjectileVFX, skillSpawner.position, rotation);

        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.AreaEffect = config.AreaEffect;
        projectile.Damage = config.GetDamage();
        //projectile.ProjectileCollisionSound = config.ProjectileCollisionSound.clip;
        proj.GetComponent<Rigidbody2D>().velocity = dir * config.ProjectileSpeed;
    }

    private void UpdateSkillCooldowns()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skillWasCast[i])
            {
                if (timerTimes[i] < coolDownTimes[i])
                {
                    timerTimes[i] += Time.deltaTime;
                    canvasController.CoolDownTimer(timerTimes[i], coolDownTimes[i], i);
                }
                else if (timerTimes[i] >= coolDownTimes[i])
                {
                    timerTimes[i] = 0;
                    skillWasCast[i] = false;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (skills == null)
            return;

        // Always draw something, even if not moving
        Vector2 facing = FacingDirection;
        if (facing == Vector2.zero)
            facing = Vector2.right;

        foreach (var skill in skills)
        {
            if (skill == null)
                continue;
            // -------------------------
            // CONE SKILL GIZMO
            // -------------------------
            ConeSkill cone = skill.GetComponent<ConeSkill>();
            if (cone != null)
            {
                float radius = cone.Radius;
                float angle = cone.Angle;
                Vector3 origin = transform.position;

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(origin, radius);

                float halfAngle = angle;
                Vector3 leftDir = Quaternion.Euler(0, 0, halfAngle) * facing;
                Vector3 rightDir = Quaternion.Euler(0, 0, -halfAngle) * facing;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin, origin + leftDir * radius);
                Gizmos.DrawLine(origin, origin + rightDir * radius);

                Gizmos.color = new Color(1, 0, 0, 0.2f);
                for (float a = -halfAngle; a <= halfAngle; a += 5f)
                {
                    Vector3 dir = Quaternion.Euler(0, 0, a) * facing;
                    Gizmos.DrawLine(origin, origin + dir * radius);
                }

                continue;
            }

            // -------------------------
            // RADIAL SKILL GIZMO
            // -------------------------
            RadialSkill radial = skill.GetComponent<RadialSkill>();
            if (radial != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, radial.Radius);
                continue;
            }

            // -------------------------
            // EXPANDING SKILL GIZMO
            // -------------------------
            ExpandingSkill expanding = skill.GetComponent<ExpandingSkill>();
            if (expanding != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, expanding.MaxRadius);
                continue;
            }



            //ConeSkill cone = skill.GetComponent<ConeSkill>();
            //if (cone == null)
            //    continue;

            //float radius = cone.Radius;
            //float angle = cone.Angle;

            //Vector3 origin = transform.position;

            //// Draw radius circle
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(origin, radius);

            //// Draw cone edges
            //float halfAngle = angle;

            //Vector3 leftDir = Quaternion.Euler(0, 0, halfAngle) * facing;
            //Vector3 rightDir = Quaternion.Euler(0, 0, -halfAngle) * facing;

            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(origin, origin + leftDir * radius);
            //Gizmos.DrawLine(origin, origin + rightDir * radius);

            //// Fill the cone with debug lines (optional but helpful)
            //Gizmos.color = new Color(1, 0, 0, 0.2f);
            //for (float a = -halfAngle; a <= halfAngle; a += 5f)
            //{
            //    Vector3 dir = Quaternion.Euler(0, 0, a) * facing;
            //    Gizmos.DrawLine(origin, origin + dir * radius);
            //}
        }
    }

    //private void Fire() {
    //	fireY = Input.GetAxis("SpellVertical");
    //	fireX = Input.GetAxis("SpellHorizontal");

    //	if (Input.touchCount == 2 && Input.touches[0].phase == TouchPhase.Began)
    //	{
    //		Debug.Log("Right Click (Two Finger Tap)");
    //		// Handle right-click behavior here
    //	}

    //	if (Input.GetMouseButton(1) || (Input.touchCount == 2 && Input.touches[0].phase == TouchPhase.Began)) {
    //		Vector3 direction = MousePointerDirection();

    //		fireX = Mathf.Clamp(direction.x, -1, 1);
    //		fireY = Mathf.Clamp(direction.y, -1, 1);
    //	}

    //	if ((fireX != 0 || fireY != 0)) {
    //		skillSpawner.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-fireY, -fireX) * 180 / Mathf.PI);
    //		if (skillWasCast[activeSkillIndex] == false) {
    //			skillWasCast[activeSkillIndex] = true;
    //			string skillType = activeSkill.GetComponent<SkillConfig>().MaskType.ToString();
    //			//foreach (var animator in animator) {
    //				animator.SetTrigger("Attack");
    //			//}
    //			switch (skillType) {
    //				//case { skill that required casting it }:
    //				// CastSkill();
    //				//	break;
    //				//case { skill that required placing it }:
    //				//	PlaceSkill();
    //				//	break;
    //				default:
    //					//StartCoroutine(ThrowSkill(fireX, fireY));
    //					break;
    //			}
    //		}
    //	}

    //	for (int i = 0; i < skills.Length; i++) {
    //		if (skillWasCast[i]) {
    //			if (timerTimes[i] < coolDownTimes[i]) {
    //				timerTimes[i] += Time.deltaTime;
    //				canvasController.CoolDownTimer(timerTimes[i], coolDownTimes[i], i);
    //			}
    //			else if (timerTimes[i] >= coolDownTimes[i]) {
    //				timerTimes[i] = 0;
    //				skillWasCast[i] = false;
    //			}
    //		}
    //	}
    //}

    //   private void CastSkill() {
    //	GameObject spell = Instantiate(activeSkill, transform.position, Quaternion.identity, activeSkillContainer) as GameObject;
    //}

    //private void PlaceSkill() {
    //	GameObject spell = Instantiate(activeSkill, skillSpawnPoint.position, Quaternion.identity) as GameObject;
    //}

    //private IEnumerator ThrowSkill(float fireX, float fireY)
    //{
    //	GameObject SkillConfig = Instantiate(activeSkill, transform.position, Quaternion.identity) as GameObject;
    //	Rigidbody2D SkillConfigRidgidbody2D = SkillConfig.GetComponent<Rigidbody2D>();
    //	SkillConfigRidgidbody2D.velocity = new Vector3(fireX, fireY, 0);
    //	SkillConfigRidgidbody2D.velocity = (Vector3.Normalize(SkillConfigRidgidbody2D.velocity) * projectileSpeed);
    //	yield return new WaitForSeconds(firingRate);
    //}

    private Vector3 MousePointerDirection()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z += Camera.main.nearClipPlane;

		Vector3 heading = mousePosition - transform.position;
		float distance = heading.magnitude - 5;
		Vector3 direction = heading / distance;
		return direction;
	}

	private void SetAnimations(float inputX, float inputY)
    {
        // Animation speed based on stick magnitude
        if (!isAttacking)
        {
            float moveAmount = new Vector2(inputX, inputY).magnitude;
            animator.speed = Mathf.Lerp(0.5f, 1.5f, moveAmount);
        }

        animator.SetBool("RunNorth", false);
        animator.SetBool("RunSouth", false);
        animator.SetBool("RunEast", false);
        animator.SetBool("RunWest", false);
        animator.SetBool("Idle", false);
        animator.ResetTrigger("Attack");

        // No movement
        if (Mathf.Abs(inputX) < 0.1f && Mathf.Abs(inputY) < 0.1f)
		{
            animator.SetBool("Idle", true);
            currentDirection = MoveDir.None;
			if (CanReturnToIdle())
			{
                animator.Play("IdleSouth");
                
            }
                
            return;
		}

        // Vertical dead zone so slight up/down doesn't override left/right
        float verticalDeadZone = 0.2f;

		if (Mathf.Abs(inputY) < verticalDeadZone)
		{
			// Treat as horizontal movement
			if (inputX > 0)
			{
				animator.SetBool("RunEast", true);
				currentDirection = MoveDir.East;
			}
			else
			{
				animator.SetBool("RunWest", true);
				currentDirection = MoveDir.West;
			}

            return;
        }

        // If vertical is strong enough, use it
        if (Mathf.Abs(inputY) >= Mathf.Abs(inputX))
        {
			if (inputY > 0)
			{
				animator.SetBool("RunNorth", true);
                currentDirection = MoveDir.North;
            }
            else
			{
				animator.SetBool("RunSouth", true);
				currentDirection = MoveDir.South;
            }
        }
        else
        {
			if (inputX > 0)
			{
				animator.SetBool("RunEast", true);
				currentDirection = MoveDir.East;
            }
			else
			{
				animator.SetBool("RunWest", true);
				currentDirection = MoveDir.West;
            }
        }
    }

    private bool CanReturnToIdle()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (animNameHashes.Contains(state.shortNameHash))
		{
			if (state.normalizedTime >= 1f)
            {
                return true;
            }

            return false;
        }

        return true;
    }
}
