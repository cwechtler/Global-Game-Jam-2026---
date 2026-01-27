using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreDetails : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI total, PlayerName;
	[SerializeField] private GameObject closeButton;

	public GameObject CloseButton { get => closeButton; set => closeButton = value; }

	public void setDetails(string totalScore, string name) {
		PlayerName.text = name;
		total.text = totalScore;
    }
}
