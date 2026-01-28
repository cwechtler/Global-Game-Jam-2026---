using System;
using System.Collections;
using System.Collections.Generic;
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
	[SerializeField] private GameObject lightningEndPoint;
	[Space]
	[SerializeField] private Transform playerTransform;
	[SerializeField] private GameObject rigFront, rigBack;
	[Space]
	[SerializeField] private CanvasController canvasController;

    public GameObject LightningEndPoint { get => lightningEndPoint; }
	public int ExperiencePoints { get => experiencePoints; set => experiencePoints = value; }
	public List<GameObject> InventoryItems { get => inventoryItems; set => inventoryItems = value; }
	public float FireX { get => fireX; }
	public float FireY { get => fireY; }

	private List<GameObject> inventoryItems = new List<GameObject>();

	private Rigidbody2D myRigidbody2D;
	private Animator[] animators;

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
		animators = GetComponentsInChildren<Animator>(true);

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

			//SelectSkill();
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

		SetAnimations();
		//FlipDirection();
	}

	private IEnumerator PlayerDeath()
	{
		isDead = true;
		myRigidbody2D.isKinematic = true;
		myRigidbody2D.velocity = new Vector3(0, 0, 0);
		foreach (var animator in animators) {
			animator.SetBool("IsDead", true);
		}
		SoundManager.instance.PlayDeathClip();
		yield return new WaitForSeconds(2f);
		LevelManager.instance.LoadLevel(LevelManager.LoseLevelString);
	}

	public void SetActiveSkill(int index) {
		GameController.instance.ActiveSkillIndex = index;
		activeSkillIndex = index;
		activeSkill = skills[index];
		SkillConfig activeSkillSkillConfig = activeSkill.GetComponent<SkillConfig>();
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
		else if (Input.GetButtonDown("Fire3")) {
			SetActiveSkill(2);
		}
		else if (Input.GetButtonDown("Jump")) {
			SetActiveSkill(3);
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
				string skillType = activeSkill.GetComponent<SkillConfig>().SkillElementType.ToString();
				foreach (var animator in animators) {
					animator.SetTrigger("Attack");
				}
				switch (skillType) {
					//case { skill that required casting it }:
     //                   CastSkill();
					//	break;
					//case { skill that required placing it }:
					//	PlaceSkill();
					//	break;
					default:
						StartCoroutine(ThrowSkill(fireX, fireY));
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

	private void SetAnimations()
	{
		if (moveHorizontaly || moveVertically) {
			foreach (var animator in animators) {
				if (animator.isActiveAndEnabled)
					animator.SetBool("Move", true);
			}
		}
		else {
			foreach (var animator in animators) {
				if (animator.isActiveAndEnabled)
					animator.SetBool("Move", false);
			}
		}
	}

	private void FlipDirection()
	{
		if (moveHorizontaly ) {
			rigFront.SetActive(true);
			rigBack.SetActive(false);
			float DirectionX = Mathf.Sign(myRigidbody2D.velocity.x);

			if (DirectionX == 1) {
				playerTransform.localScale = new Vector2(1f, 1f);
			}
			if (DirectionX == -1) {
				playerTransform.localScale = new Vector2(-1f, 1f);
			}
		}

		if (moveVertically) {
			float DirectionY = Mathf.Sign(myRigidbody2D.velocity.y);

			if (DirectionY == 1) {
				rigFront.SetActive(false);
				rigBack.SetActive(true);
			}
			if (DirectionY == -1) {
				rigFront.SetActive(true);
				rigBack.SetActive(false);
			}
		}
	}
}
