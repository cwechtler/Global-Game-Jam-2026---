using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoseCanvas : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI total, placeholder;
	[SerializeField] private GameObject saveScore, mainMenuButton;
	[SerializeField] private TMP_InputField PlayerName;

	private Animator animator;
	private bool selected;

	void Start()
	{
		animator = gameObject.GetComponent<Animator>();
		PlayerName.text = GameController.instance.PlayerName;
		total.text = GameController.instance.EnemiesKilled.ToString();

		if(GameController.instance.PlayerName == System.Environment.MachineName) {
			EventSystem.current.SetSelectedGameObject(PlayerName.gameObject);
		}
	}

	private void Update()
	{
		if (saveScore.GetComponent<Button>().interactable == false && !selected) {
			selected = true;
			EventSystem.current.SetSelectedGameObject(mainMenuButton);
		}
	}

	public void CallLoadLevel(string name)
	{
		LevelManager.instance.LoadLevel(name);
	}

	public void MainMenu()
	{
		LevelManager.instance.LoadLevel(LevelManager.MainMenuString);
	}

	public void RestartGame()
	{
		LevelManager.instance.LoadLevel(LevelManager.Level1String, true);
	}

	public void SaveScore()
	{
		bool saved = false;
		if (PlayerName.text == "")
		{
			animator.SetBool("Flash", true);
		}
		else
		{
			saved = GameController.instance.SaveHighScore(PlayerName.text);
			if (saved)
			{
				saveScore.GetComponentInChildren<TextMeshProUGUI>().text = "Score Saved";
				saveScore.GetComponent<Button>().interactable = false;
				Image image = saveScore.GetComponentInChildren<Image>();
				image.color = new Color(image.color.r, image.color.g, image.color.b, .37f);
				PlayerName.interactable = false;
				Image nameImage = PlayerName.gameObject.GetComponent<Image>();
				nameImage.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
				EventSystem.current.SetSelectedGameObject(PlayerName.gameObject);
			}
		}
	}

	public void StopAnim() {
		animator.SetBool("Flash", false);
	}

	public void InputName()
	{
		GameController.instance.PlayerName = PlayerName.text;
		PlayerPrefsManager.SetPlayerName(PlayerName.text);
	}
}
