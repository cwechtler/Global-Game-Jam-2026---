using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [Header("Name")]
    [SerializeField] private GameObject myDestructable;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private string myTrigger;    
    [SerializeField] private AudioClip soundClip;
    [Space]
    [SerializeField] private Animator exitAnimator;
    [SerializeField] private string exitTrigger;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Something(2f));
    }

    // Called by Animation Event
    void ActivateExit()
    {
        exitAnimator.SetTrigger(exitTrigger);
    }

    IEnumerator Something(float value)
    {
        yield return new WaitUntil(()=>myDestructable == null);
        SoundManager.instance.PlayDestructibleSound(soundClip);
        myAnimator.SetTrigger(myTrigger);
    }


}
