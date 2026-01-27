using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
	private Button menuButton;

	void Start()
	{
		menuButton = gameObject.GetComponent<Button>();
		menuButton.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		SoundManager.instance.SetButtonClip();
	}
}
