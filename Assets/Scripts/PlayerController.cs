using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
	[SerializeField] private int health = 100;
	[SerializeField] private Vector2 speed = new Vector2(10, 10);
	[SerializeField] private float projectileSpeed;
	[Space]
	[Tooltip("Skill Prefabs")]
	[SerializeField] private GameObject[] skills;
	[SerializeField] private Transform skillSpawner, skillSpawnPoint, activeSkillContainer;
	[Space]
	[SerializeField] private Transform playerTransform;
	[Space]
	[SerializeField] private CanvasController canvasController;
	[SerializeField] private maskType activeMaskType;

	public maskType ActiveMaskType { get => activeMaskType; }
    public enum MoveDir { None, North, South, East, West }
    public MoveDir currentDirection = MoveDir.None;



    public int ExperiencePoints { get => experiencePoints; set => experiencePoints = value; }
	public List<GameObject> InventoryItems { get => inventoryItems; set => inventoryItems = value; }
	public float FireX { get => fireX; }
	public float FireY { get => fireY; }

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
	private bool[] skillWasCast;
	public int experiencePoints;

	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>(true);

		//SetActiveSkill(0);

		//coolDownTimes = new float[skills.Length];
		//timerTimes = new float[skills.Length];
		//skillWasCast = new bool[skills.Length];

		//for (int i = 0; i < skills.Length; i++) {
		//	SkillConfig skill = skills[i].GetComponent<SkillConfig>();
		//	coolDownTimes[i] = skill.CoolDownTime;
		//	canvasController.SetCoolDownTime(i, coolDownTimes[i]);
		//	canvasController.SetSkillImages(i, skill.SkillImage);
		//}
	}

	void Update()
	{
		if (!isDead && !GameController.instance.isPaused) {
			Move();

			SelectSkill();
			//Fire();
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

			health -= damage;
			canvasController.ReduceHealthBar(health);

			if (health <= 0) {
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
        activeMaskType = activeSkillSkillConfig.MaskType;
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

	private void Fire() {
		fireY = Input.GetAxis("SpellVertical");
		fireX = Input.GetAxis("SpellHorizontal");

		if (Input.touchCount == 2 && Input.touches[0].phase == TouchPhase.Began)
		{
			Debug.Log("Right Click (Two Finger Tap)");
			// Handle right-click behavior here
		}

		if (Input.GetMouseButton(1) || (Input.touchCount == 2 && Input.touches[0].phase == TouchPhase.Began)) {
			Vector3 direction = MousePointerDirection();

			fireX = Mathf.Clamp(direction.x, -1, 1);
			fireY = Mathf.Clamp(direction.y, -1, 1);
		}

		if ((fireX != 0 || fireY != 0)) {
			skillSpawner.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-fireY, -fireX) * 180 / Mathf.PI);
			if (skillWasCast[activeSkillIndex] == false) {
				skillWasCast[activeSkillIndex] = true;
				string skillType = activeSkill.GetComponent<SkillConfig>().MaskType.ToString();
				//foreach (var animator in animator) {
					animator.SetTrigger("Attack");
				//}
				switch (skillType) {
					//case { skill that required casting it }:
					// CastSkill();
					//	break;
					//case { skill that required placing it }:
					//	PlaceSkill();
					//	break;
					default:
						//StartCoroutine(ThrowSkill(fireX, fireY));
						break;
				}
			}
		}

		for (int i = 0; i < skills.Length; i++) {
			if (skillWasCast[i]) {
				if (timerTimes[i] < coolDownTimes[i]) {
					timerTimes[i] += Time.deltaTime;
					canvasController.CoolDownTimer(timerTimes[i], coolDownTimes[i], i);
				}
				else if (timerTimes[i] >= coolDownTimes[i]) {
					timerTimes[i] = 0;
					skillWasCast[i] = false;
				}
			}
		}
	}

	private void CastSkill() {
		GameObject spell = Instantiate(activeSkill, transform.position, Quaternion.identity, activeSkillContainer) as GameObject;
	}

	private void PlaceSkill() {
		GameObject spell = Instantiate(activeSkill, skillSpawnPoint.position, Quaternion.identity) as GameObject;
	}

	private IEnumerator ThrowSkill(float fireX, float fireY)
	{
		GameObject SkillConfig = Instantiate(activeSkill, transform.position, Quaternion.identity) as GameObject;
		Rigidbody2D SkillConfigRidgidbody2D = SkillConfig.GetComponent<Rigidbody2D>();
		SkillConfigRidgidbody2D.velocity = new Vector3(fireX, fireY, 0);
		SkillConfigRidgidbody2D.velocity = (Vector3.Normalize(SkillConfigRidgidbody2D.velocity) * projectileSpeed);
		yield return new WaitForSeconds(firingRate);
	}

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
        float moveAmount = new Vector2(inputX, inputY).magnitude;
        animator.speed = Mathf.Lerp(0.5f, 1.5f, moveAmount);

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
