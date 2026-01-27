using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{

	[SerializeField] GameObject scorePrefab;
	[SerializeField] GameObject content;
	[SerializeField] GameObject scoreDetails;

	public bool isloaded = false;
	private GameObject selected;

	void Start()
	{
		if (GameController.instance.InitDone && !isloaded)
		{
			SetHighScores();
		}
	}

	public void SetHighScores()
	{
		int scoreCount = GameController.instance.HighScores.Count;

		for (int i = 0; i < scoreCount; i++)
		{
			GameObject scoreRow = Instantiate(scorePrefab, transform.position, Quaternion.identity) as GameObject;
			scoreRow.transform.SetParent(content.transform, false);
			PlayerScorePrefabController prefabController = scoreRow.GetComponent<PlayerScorePrefabController>();

			prefabController.PlayerRank.text = (i + 1).ToString();
			prefabController.Index = i;

			prefabController.PlayerScore.text = GameController.instance.HighScores[i].Score.ToString();
			prefabController.PlayerName.text = GameController.instance.HighScores[i].Name;
			
			if (i == 0)
			{
				prefabController.Image1.SetActive(true);
			}
		}

		isloaded = true;
	}

	public void OpenDetails(int index)
	{
		selected = EventSystem.current.currentSelectedGameObject;

		string score = GameController.instance.HighScores[index].Score.ToString();
		string highScoreName = GameController.instance.HighScores[index].Name;

		HighScoreDetails highScoreDetails = scoreDetails.GetComponent<HighScoreDetails>();
		highScoreDetails.setDetails(score, highScoreName);

		scoreDetails.SetActive(true);
		EventSystem.current.SetSelectedGameObject(highScoreDetails.CloseButton);	
	}

	public void CloseDetails() { 
		scoreDetails.SetActive(false);
		EventSystem.current.SetSelectedGameObject(selected);
	}
}
