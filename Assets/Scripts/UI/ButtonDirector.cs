using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDirector : MonoBehaviour
{
	[SerializeField] private bool mainMenu;
	[ConditionalHide("mainMenu", true, false)]
	[SerializeField] private AudioClip portalClose;
	public void CallLoadLevel(string name)
	{
		LevelManager.instance.LoadLevel(name);
	}

	public void StartGame()
	{
		LevelManager.instance.LoadLevel(LevelManager.Level1String, true);
	}

	public void StartGameFromAnimation()
	{
		GameObject taggedGO = GameObject.FindGameObjectWithTag("StartAnim");
		Animator animator = taggedGO.GetComponent<Animator>();
		animator.SetBool("Start", true);
		AudioSource audiosource = GetComponentInChildren<AudioSource>();
		if (audiosource != null)
		{
			audiosource.clip = portalClose;
			audiosource.Play();
		}
	}

	public void MainMenu()
	{
		LevelManager.instance.LoadLevel(LevelManager.MainMenuString);
	}

	public void ControlsMenu()
	{
		LevelManager.instance.LoadLevel(LevelManager.ControlsString);
	}

	public void Quit()
	{
		LevelManager.instance.QuitRequest();
	}
}
