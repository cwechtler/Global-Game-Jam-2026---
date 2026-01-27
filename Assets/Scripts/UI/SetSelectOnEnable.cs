using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetSelectOnEnable : MonoBehaviour
{
	//private Button button;
	private bool setThis = false;
	//private string[] names;

	private void Update()
	{
		//for (int x = 0; x < names.Length; x++) {
		//	if (names[x].Length == 19 || names[x].Length == 33) {
		//		controller = true;
		//	}
		//}
		if (!setThis && EventSystem.current.currentSelectedGameObject != this.gameObject && GameController.instance.ControllerConnected)
		{
			Debug.Log("Set this");
			setThis = true;
			EventSystem.current.SetSelectedGameObject(this.gameObject);
		}
		else if (EventSystem.current.currentSelectedGameObject != null && !GameController.instance.ControllerConnected) {
			Debug.Log("Set null");
			setThis = false;
			EventSystem.current.SetSelectedGameObject(null);
		}
	}


	//private void OnEnable()
	//{
	//	//names = Input.GetJoystickNames();
	//	button = GetComponent<Button>();

	//	//for (int x = 0; x < names.Length; x++) {
	//	//	if (names[x].Length == 19 || names[x].Length == 33) {
	//	//		controller = true;
	//	//	}
	//	//}
	//	//if (controller) {		
	//		button.Select();
	//		button.OnSelect(null);
	//	//}
	//}
}
