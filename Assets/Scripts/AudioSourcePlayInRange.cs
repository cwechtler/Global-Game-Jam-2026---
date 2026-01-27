using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePlayInRange : MonoBehaviour
{

	private AudioSource audioSource;
	private AudioListener audioListener;
	private float distanceFromPlayer;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioListener>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		distanceFromPlayer = Vector3.Distance(transform.position, audioListener.transform.position);

		if (distanceFromPlayer <= audioSource.maxDistance) {
			PlayAudioSource(true);
		}
		else {
			PlayAudioSource(false);
		}
	}

	private void PlayAudioSource(bool inRange)
	{
		if (audioSource.clip == null) {
			return;
		}

		if (!inRange && audioSource.isPlaying) {
			audioSource.Stop();
			audioSource.loop = false;
		}
		else if (inRange && !audioSource.isPlaying) {
			audioSource.Play();
			audioSource.loop = true;
		}
	}
}
