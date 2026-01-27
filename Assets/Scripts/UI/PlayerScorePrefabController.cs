using TMPro;
using UnityEngine;

public class PlayerScorePrefabController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI playerRank, playerScore, playerName;
	[SerializeField] private GameObject Image;

	public TextMeshProUGUI PlayerRank { get => playerRank; set => playerRank = value; }
	public TextMeshProUGUI PlayerScore { get => playerScore; set => playerScore = value; }
	public TextMeshProUGUI PlayerName { get => playerName; set => playerName = value; }
	public GameObject Image1 { get => Image; set => Image = value; }
	public int Index { get => index; set => index = value; }

	private int index;

	public void LoadPlayerDetails()
	{
		Leaderboard leaderboard = GetComponentInParent<Leaderboard>();
		leaderboard.OpenDetails(index);
	}
}
