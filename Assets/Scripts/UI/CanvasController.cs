using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasController : MonoBehaviour
{
	[SerializeField] private PlayerController playerController;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject inventory;
	[Space]
	[SerializeField] private Color32 defaultTextColor = new Color32(0, 138, 255, 255);
	[SerializeField] private Color32 activeTextColor = new Color32(255, 0, 0, 255);
	[Space]
	[SerializeField] private Slider playerHealthBar;
	[SerializeField] private TextMeshProUGUI ScoreText;
	[SerializeField] private TextMeshProUGUI[] skillTexts;
	[SerializeField] private Slider[] skillCoolDowns;
	[SerializeField] private Image[] skillCooldownFill;

	private Button button;
	private TextMeshProUGUI buttonText;
	private Animator animator;

	private void Start()
	{
		playerHealthBar.value = 100;
		UpdateTextColor();
		foreach (Transform child in inventory.transform) {
			if (child.name == "Key") {
				print("Has Key");
			}
		}
	}

	private void Update()
	{
		ScoreText.text = GameController.instance.EnemiesKilled.ToString();
		if (GameController.instance.isPaused) {
			pausePanel.SetActive(true);
		}
		else {
			pausePanel.SetActive(false);
		}
	}

	public void AddInventoryItem(GameObject inventoryPrefab) {
		Instantiate(inventoryPrefab, inventory.transform);
	}

	public void RemoveInventoryItem(string itemKey)
	{
		foreach (Transform child in inventory.transform) {
			if (child.name == itemKey) {
				GameObject.Destroy(child.gameObject);
			}
		}
	}

	public void UpdateTextColor()
	{		
		for (int i = 0; i < skillTexts.Length; i++) {
			if (i == GameController.instance.ActiveSkillIndex) {
				skillTexts[i].color = activeTextColor;
				skillCooldownFill[i].color = activeTextColor;
			}
			else {
				skillTexts[i].color = defaultTextColor;
				skillCooldownFill[i].color = Color.white;
			}
		}
	}

	public void SetSkill(int index) {
		playerController.SetActiveSkill(index);
	}

	public void SetSkillImages(int index, Sprite sprite) {
		skillTexts[index].GetComponentInParent<Image>().sprite = sprite;
	}

	public void SetCoolDownTime(int index, float coolDownTime) {
		skillCoolDowns[index].maxValue = coolDownTime;
		skillCoolDowns[index].value = coolDownTime;
	}

	public void CoolDownTimer(float timeRemaining, float skillCoolDown, int index) {
		if (timeRemaining < skillCoolDown) {
			skillCoolDowns[index].value = timeRemaining;
		}
	}

	public void ReduceHealthBar(int amount) {
		playerHealthBar.value = amount;
	}

	public void MainMenu()
	{
		LevelManager.instance.LoadLevel(LevelManager.MainMenuString);
	}

	public void Options()
	{
		animator.SetBool("FadeOut", true);
		LevelManager.instance.LoadLevel(LevelManager.OptionsString, .9f);
	}

	public void QuitGame()
	{
		LevelManager.instance.QuitRequest();
	}
}
