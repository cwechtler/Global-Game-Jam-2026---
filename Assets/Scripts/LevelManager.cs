using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance = null;

	public const string MainMenuString = "Main Menu";
	public const string OptionsString = "Options";
	public const string ControlsString = "Controls Menu";
	public const string Level1String = "Level 1";
	public const string LoseLevelString = "Lose Level";
    public const string WinLevelString = "Win Level";

#if UNITY_WEBGL
		[ConditionalHide("setSessionStorageGameQuitURL", true, true)]
		[Tooltip("Select if build is intended for a React App.")]
		[SerializeField] private bool forReactApp = false;
		[Space]
		[ConditionalHide("forReactApp", true, true)]
		[Tooltip("For browser session storage. Uncheck to set URL manually")]
		[SerializeField] private bool setSessionStorageGameQuitURL = true;

		[ConditionalHide(conditionalSourceFields: new string[] { "setSessionStorageGameQuitURL", "forReactApp" }, conditionalSourceFieldInverseBools: new bool[] { false, true }, hideInInspector: true, inverse: false)]
		[Tooltip("This is the string value of the url sessionstorage Item you want to get")]
		[SerializeField] private string sessionStorageItem;
		[Space]
		[ConditionalHide(conditionalSourceFields: new string[] { "setSessionStorageGameQuitURL", "forReactApp" }, true, true, true)]
		[Tooltip("Where to redirect on Quit.  *** Not applicable if for React app or setting session storage quit URL. ***")]
		[SerializeField] private string webglQuitURL = "about:blank";
#endif

    public string currentScene { get; private set; }

	private void Awake(){
		if (instance == null){
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else if (instance != this){
			Destroy(gameObject);
		}
	}

	private void Start(){
		currentScene = SceneManager.GetActiveScene().name;
		if (currentScene == MainMenuString)
		{
			SoundManager.instance.PlayMonologue();
		}
	}

	private void Update(){
		if (SceneManager.GetActiveScene().name != currentScene) {
			currentScene = SceneManager.GetActiveScene().name;
		}

		//if (Input.GetButtonDown("Enter")) {
		//	if (currentScene != MainMenuString) {
		//		LoadLevel(MainMenuString);
		//	}
		//	else {
		//		LoadLevel(Level1String);
		//	}
		//}

		if (Input.GetButtonDown("Cancel")) {
			QuitRequest();
		}
	}

	private IEnumerator LoadScene(string name, float waitTime)
	{
		GameController.instance.FadePanel();
		Debug.Log("start Coroutine");
		yield return new WaitForSeconds(waitTime);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
		yield return new WaitUntil(() => asyncOperation.isDone);
		print("Scene " + currentScene + " Loaded");
		SoundManager.instance.PlayMusicForScene(ReferanceIndex(name));
	}

	private IEnumerator LoadScene(int sceneToLoad, float waitTime)
	{
		print("Load " + sceneToLoad);
		yield return new WaitForSeconds(waitTime);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
		yield return new WaitUntil(() => asyncOperation.isDone);
		print("Scene " + currentScene + " Loaded");
	}

	private IEnumerator UnloadScene(float waitTime, string name)
	{
		float counter = 0f;

		while (counter < waitTime) {
			counter += GameController.instance.timeDeltaTime;
		}
		if (counter >= waitTime) {
			print("Unload");
			SceneManager.UnloadSceneAsync(name);
		}
		yield return null;
	}

	private int ReferanceIndex(string scene)
	{
		int randomIndex = Random.Range(1, SoundManager.instance.MusicArrayLength);
		int clipIndex;
		switch (scene) {
			case MainMenuString:
				clipIndex = 0;
				break;
			case OptionsString:
				clipIndex = 0;
				break;
			case ControlsString:
				clipIndex = 0;
				break;
            case WinLevelString:
                clipIndex = 0;
                break;
            case LoseLevelString:
				clipIndex = 0;
				break;
			case Level1String:	
				clipIndex = 1;
				break;
			case "Level 2":
				clipIndex = randomIndex;
				break;
			default:
				clipIndex = 0;
				break;
		}
		return clipIndex;
	}

	public void LoadLevel(string name, bool restart = false)
	{
		Debug.Log("Level load requested for: " + name);
		if (restart) {
			GameController.instance.resetGame();
		}
		StartCoroutine(LoadLevel(name, .9f));
	}

	public IEnumerator LoadLevel(string name, float waitTime)
	{
		GameController.instance.FadePanel();
		yield return new WaitForSeconds(waitTime);
		SoundManager.instance.PlayMusicForScene(ReferanceIndex(name));
		SceneManager.LoadScene(name);
	}

	public void LoadLevel(int levelIndex, float waitTime)
	{
		StartCoroutine(LoadScene(levelIndex, waitTime));
	}

	public void LoadLevelAdditive(string name)
	{
		SoundManager.instance.PlayMusicForScene(ReferanceIndex(name));
		SceneManager.LoadScene(name, LoadSceneMode.Additive);
	}

	public void LoadNextLevel()
	{
		StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1, .9f));
		currentScene = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name;
	}

	public void QuitRequest()
	{
		Debug.Log("Level Quit Request");

		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBGL
			Application.Quit();
			if (forReactApp){
				WebGLPluginJS.ReactQuitRequest();
			}
			else if (setSessionStorageGameQuitURL) {
				WebGLPluginJS.SessionRedirect(sessionStorageItem);
			}
			else {
				WebGLPluginJS.Redirect(webglQuitURL);
			}	
		#else
			Application.Quit();
		#endif
	}
}
