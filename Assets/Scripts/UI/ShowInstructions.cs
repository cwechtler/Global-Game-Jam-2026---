using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInstructions : MonoBehaviour
{
	[SerializeField] private GameObject instructions;
	[SerializeField] private bool showByTimer;
	[ConditionalHide("showByTimer", true)]
	[SerializeField] private float seconds;
	[ConditionalHide("showByTimer", true,true)]
	[SerializeField] private GameObject spawnerToActivateInstructions;

	private float timeOfAnimation;

	private void Start()
	{
		timeOfAnimation = instructions.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
		if (!GameController.instance.InstructionsToggle)
		{
			StartCoroutine(SetInstructionsOn(timeOfAnimation));
		}
	}

	private IEnumerator SetInstructionsOn(float time) {
		if (showByTimer) {
			yield return new WaitForSeconds(seconds);
		}
		else {
			yield return new WaitUntil(() => spawnerToActivateInstructions.activeInHierarchy);
		}		
		instructions.SetActive(true);
		yield return new WaitForSeconds(time);
		this.gameObject.SetActive(false);
	}
}
