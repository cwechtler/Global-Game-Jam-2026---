using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

	//turn tips off
	//Difficulty level
	//Key bindings


	//[SerializeField] private float panelFadeTime;
	[Space]
	[SerializeField] private Slider masterVolumeSlider;
	[SerializeField] private Slider musicVolumeSlider;
	[SerializeField] private Slider sfxVolumeSlider;
	[SerializeField] private Toggle instructionsToggle;
	[Space]
	//[SerializeField] private GameObject canvasGroupPanel;
	//[SerializeField] private GameObject fadePanel;
	[SerializeField] private Slider startSliderInSelected;

	private CanvasGroup canvasGroup;
	private Animator canvasGroupAnimator;
	private Animator fadeAnimator;

	void Start ()
	{
		startSliderInSelected.Select();

		//if (GameController.instance.isPaused) {
		//	canvasGroup = canvasGroupPanel.GetComponent<CanvasGroup>();
		//	canvasGroupAnimator = canvasGroupPanel.GetComponent<Animator>();
		//	canvasGroupAnimator.SetBool("FadeIn", true);
		//}
		//else {
		//	fadeAnimator = fadePanel.GetComponent<Animator>();
		//}

		GetSavedVolumeKeys();
		if (PlayerPrefs.HasKey("mouse_controls"))
		{
			instructionsToggle.isOn = PlayerPrefsManager.GetInstructionsToggle();
		}
		else
		{
			instructionsToggle.isOn = GameController.instance.InstructionsToggle;
		}
	}

	void Update()
	{
		SoundManager.instance.ChangeMasterVolume(masterVolumeSlider.value);
		SoundManager.instance.ChangeMusicVolume(musicVolumeSlider.value);
		SoundManager.instance.ChangeSFXVolume(sfxVolumeSlider.value);
		GameController.instance.InstructionsToggle = instructionsToggle.isOn;
	}

	private void GetSavedVolumeKeys()
	{
		if (PlayerPrefs.HasKey("master_volume")) {
			masterVolumeSlider.value = PlayerPrefsManager.GetMasterVolume();
		}
		else {
			masterVolumeSlider.value = -20f;
		}

		if (PlayerPrefs.HasKey("music_volume")) {
			musicVolumeSlider.value = PlayerPrefsManager.GetMusicVolume();
		}
		else {
			musicVolumeSlider.value = 0f;
		}

		if (PlayerPrefs.HasKey("sfx_volume")) {
			sfxVolumeSlider.value = PlayerPrefsManager.GetSFXVolume();
		}
		else {
			sfxVolumeSlider.value = 0f;
		}
	}

	public void MainMenu() {
		LevelManager.instance.LoadLevel(LevelManager.MainMenuString);
	}

	public void ControlsMenu()
	{
		LevelManager.instance.LoadLevel(LevelManager.ControlsString);
	}

	public void SaveAndExit(){
		PlayerPrefsManager.SetMasterVolume (masterVolumeSlider.value);
		PlayerPrefsManager.SetMusicVolume (musicVolumeSlider.value);
		PlayerPrefsManager.SetSFXVolume(sfxVolumeSlider.value);
		PlayerPrefsManager.SetInstructionsToggle(instructionsToggle.isOn);
		LevelManager.instance.LoadLevel(LevelManager.MainMenuString);

		//if (GameController.instance.isPaused)
		//{
		//	GameCanvasController gameCanvasController = FindObjectOfType<GameCanvasController>();
		//	gameCanvasController.FadeCanvas();
		//	canvasGroupAnimator.SetBool("FadeIn", false);
		//}
		//else {
		//	fadeAnimator.SetBool("FadeOut", true);
		//	LevelManager.instance.LoadLevel(0, .9f);
		//}	
	}
	
	public void SetDefaults(){
		masterVolumeSlider.value = -20f;
		musicVolumeSlider.value = 0f;
		sfxVolumeSlider.value = 0f;
		instructionsToggle.isOn = false;
	}
}
