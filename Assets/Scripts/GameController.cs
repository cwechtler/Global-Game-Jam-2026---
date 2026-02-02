using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;

	public GameObject playerGO { get; private set; }
	public bool isPaused { get; private set; }
	public float timeDeltaTime { get; private set; }
    public int Gems { get; set; }
    public int Experience { get; set; }
	public int EnemiesKilled { get; set; }
	public int ActiveSkillIndex { get; set; }
	public string PlayerName { get; set; }
	public bool InitDone { get; private set; } = false;
	public List<HighScoreData> HighScores { get => highScores; set => highScores = value; }
	public bool ControllerConnected { get; private set; } = false;
	public bool InstructionsToggle { get => instructionsToggle; set => instructionsToggle = value; }

	private List<HighScoreData> highScores = new List<HighScoreData>(); 
	private GameObject fadePanel;
	private bool instructionsToggle = false;
	private Animator animator;
    public PlayerController player { get; set; }

    //private string apiURL = "http://10.10.10.10:8080/api/{Game Name}";
    //private string apiURL = "/api/{Game Name}";	

    private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);

			//PlayerPrefsManager.DeleteAllPlayerPrefs();

			//StartCoroutine(getRequest(apiURL));

			if (PlayerPrefsManager.DoesKeyExist(PlayerPrefsManager.PlayerNameKey)) {
				PlayerName = PlayerPrefsManager.GetPlayerName();
			}
			else {
				PlayerName = Environment.UserName;
			}

			// Check on start then let logMessageReceived listener take over.
			checkControllers();

			if (PlayerPrefs.HasKey("instructions_toggle"))
			{
				instructionsToggle = PlayerPrefsManager.GetInstructionsToggle();
			}         
        }
    }

    private void Update()
	{
		if (Input.GetButtonDown("Pause")) {
			if (!isPaused) {
				PauseGame();
			}
			else {
				ResumeGame();
			}
		}
	}

	private void OnEnable()
	{
		// Add listener for Unity's log messages
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		// Remove listener for Unity's log messages
		Application.logMessageReceived -= HandleLog;
	}

    public void AddXP(int amount)
    {
        Experience += amount;
        player.CheckSkillUnlocks(Experience);
    }

    // Used to check when a Controller with empty string is connected.
    // Typically when using a wireless controller since Input.GetJoystickNames() always returns an empty string on wireless.
    // Standard controller checking will not work in those cases.
    private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (logString.Contains("Joystick connected")) {
			//Debug.Log("Wireless controller connected!");
			ControllerConnected = true;
		}
		if (logString.Contains("Joystick reconnected")) {
			//Debug.Log("Wireless controller reconnected!");
			ControllerConnected = true;
		}
		if (logString.Contains("Joystick disconnected")) {
			//Debug.Log("Wireless controller disconnected!");
			ControllerConnected = false;
		}
	}

	private void checkControllers() {
		string[] controllers = Input.GetJoystickNames();

		if (!ControllerConnected && controllers.Length > 0)
		{
			ControllerConnected = true;
			//Debug.Log("Connected");
		}
		else if (ControllerConnected && controllers.Length == 0)
		{
			ControllerConnected = false;
			//Debug.Log("Disconnected");
		}
	}

    // Gets high scores from a web API.
    private IEnumerator getRequest(string uri)
	{
		UnityWebRequest uwr = UnityWebRequest.Get(uri);
		yield return uwr.SendWebRequest();

		Leaderboard leaderboard = FindObjectOfType<Leaderboard>();

		if (uwr.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
			if (PlayerPrefsManager.DoesKeyExist(PlayerPrefsManager.HighScoresKey))
			{
				string highScoresString = PlayerPrefsManager.GetHighScores();
				highScores = JsonConvert.DeserializeObject<List<HighScoreData>>(highScoresString);
				highScores.Sort((x, y) => y.Score.CompareTo(x.Score));

				leaderboard.SetHighScores();
			}
		}
		else
		{
			Debug.Log("Received: " + uwr.downloadHandler.text);
			string highScoresString = uwr.downloadHandler.text;
			highScores = JsonConvert.DeserializeObject<List<HighScoreData>>(highScoresString);
			highScores.Sort((x, y) => y.Score.CompareTo(x.Score));

			leaderboard.SetHighScores();
		}

		InitDone = true;
	}

    // Posts high score to a web API.
    IEnumerator postRequest(string uri, string score)
	{
		UnityWebRequest uwr = UnityWebRequest.Put(uri, score);
		uwr.method = "POST";
		uwr.SetRequestHeader("accept", "application/json");
		uwr.SetRequestHeader("Content-Type", "application/json");
		yield return uwr.SendWebRequest();

		if (uwr.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
			Debug.Log("sent: " + uwr.result);
		}
	}

	public bool SaveHighScore(string name)
	{
		HighScoreData score = new HighScoreData();
		score.Name = name;
		score.Score = EnemiesKilled;

		highScores.Add(score);
		highScores.Sort((x, y) => y.Score.CompareTo(x.Score));

		string json = JsonConvert.SerializeObject(score, Formatting.Indented);
		//StartCoroutine(postRequest(apiURL, json));  // Saves to a database if we have one.

		string highScoresJson = JsonConvert.SerializeObject(highScores, Formatting.Indented);
		PlayerPrefsManager.SetHighScores(highScoresJson);

		return true;
	}

	//public void AddEnemyType(AttackType maskType)
	//{
	//	switch (maskType)
	//	{
	//		case AttackType.mask1:
	//			//mask1++;
	//			break;
	//		case AttackType.mask2:
	//			//mask2++;
	//			break;
	//		default:
	//			break;
	//	}
	//}

	public void resetGame() {
		EnemiesKilled = 0;
		Experience = 0;
		Gems = 0;
	}

	public void FadePanel()
	{	
		fadePanel = GameObject.FindGameObjectWithTag("Fade Panel");
		fadePanel.GetComponent<Animator>().SetBool("FadeIn", true);
	}

	private void PauseGame()
	{
		timeDeltaTime = Time.deltaTime;
		isPaused = true;
		Time.timeScale = 0;
	}

	private void ResumeGame()
	{
		Time.timeScale = 1;
		isPaused = false;
	}

	private IEnumerator RespawnPlayer(int waitToSpawn)
	{
		yield return new WaitForSeconds(waitToSpawn);
		//playerGO.transform.position = spawnPoint.transform.position;
		playerGO.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		playerGO.gameObject.SetActive(true);
		playerGO.GetComponent<Rigidbody2D>().isKinematic = false;
		yield return new WaitForSeconds(1);
	}

	public IEnumerator FadeCanvasGroup_TimeScale_0(CanvasGroup canvasGroup, bool isPanelOpen, float fadeTime)
	{
		float counter = 0f;

		if (isPanelOpen) {
			while (counter < fadeTime) {
				counter += timeDeltaTime;
				canvasGroup.alpha = Mathf.Lerp(1, 0, fadeTime / timeDeltaTime);
			}
		}
		else {
			while (counter < fadeTime) {
				counter += timeDeltaTime;
				canvasGroup.alpha = Mathf.Lerp(0, 1, fadeTime / timeDeltaTime);
			}
		}
		yield return null;
	}
}
