using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;
	[Range(.01f, .5f)] [SerializeField] private float fadeInTime = .05f;

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioSource MusicAudioSource;
	[SerializeField] private AudioSource SFXAudioSource;
	[SerializeField] private AudioSource ambientAudioSource;
	[Space]
	[SerializeField] private AudioClip Monologue;
    [SerializeField] private AudioClip endMonologue;
    [SerializeField] private AudioClip[] music;
	[SerializeField] private AudioClip[] ambientClips;
	[SerializeField] private AudioClip[] walkClips;
    [SerializeField] private AudioClip[] runClips;
	[SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] hurtClips;
    [Space]
	[SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip scoreClip;
    [SerializeField] private AudioClip deathClip;

    public int MusicArrayLength { get => music.Length; }

	private float audioVolume = 1f;
	private int clipIndex = 0;

	private void Awake(){
		if (instance != null){
			Destroy(gameObject);
		} else{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("master_volume")) {
			ChangeMasterVolume(PlayerPrefsManager.GetMasterVolume());
		}
		else {
			ChangeMasterVolume(-20f);
		}

		if (PlayerPrefs.HasKey("music_volume")) {
			ChangeMusicVolume(PlayerPrefsManager.GetMusicVolume());
		}

		if (PlayerPrefs.HasKey("sfx_volume")) {
			ChangeSFXVolume(PlayerPrefsManager.GetSFXVolume());
		}

		PlayMusicForScene(0);
	}

	private void Update(){
		PlayRandomAmbient();

		VolumeFadeIn(MusicAudioSource);
		VolumeFadeIn(ambientAudioSource);
	}

	private void VolumeFadeIn(AudioSource audioSource) {
		if (audioVolume <= 1f){
			audioVolume += fadeInTime * Time.deltaTime;
			audioSource.volume = audioVolume;
		} else{
			audioVolume = 1f;
		}

		if (audioSource.clip != null){
			if (!audioSource.isPlaying){
				audioSource.Play();
				audioSource.volume = 0f;
				audioVolume = 0f;
			}
		}
	}

	private void VolumeFadeOut(AudioSource audioSource) {
		if (audioVolume >= 1f){
			audioVolume -= fadeInTime * Time.deltaTime;
			audioSource.volume = audioVolume;
		} else{
			audioVolume = 0f;
		}

		if (audioSource.volume <= 0f){
			audioSource.Stop();
		}
	}

	private void PlayRandomAmbient()
	{
		if (!ambientAudioSource.isPlaying && ambientClips.Length > 0) {
			clipIndex = Random.Range(0, ambientClips.Length);
			ambientAudioSource.PlayOneShot(ambientClips[clipIndex]);
		}
	}

	private void playRandomMusic()
	{
		int clip = Random.Range(1, music.Length);
		MusicAudioSource.clip = music[clip];
	}

	private void playRandomMusic(AudioClip[] audioClips)
	{
		if (!MusicAudioSource.isPlaying && audioClips.Length > 0) {
			int clip = Random.Range(0, audioClips.Length);
			MusicAudioSource.clip = audioClips[clip];
		}
	}

	public void PlayMonologue()
	{
		SFXAudioSource.PlayOneShot(Monologue, 1f);
	}
    public void StopMonologue()
    {
		SFXAudioSource.Stop();
    }

    public void PlayEndMonologue()
    {
        SFXAudioSource.PlayOneShot(endMonologue, 1f);
    }

    public void PlayMusicForScene(int index)
	{
		if (music.Length > 0) {
			MusicAudioSource.clip = music[index];
			if (LevelManager.instance.currentScene == "MikeTest") {
				MusicAudioSource.volume = .5f;
				audioVolume = 0f;
			}
		}
	}

	public void StartAudio(){
		MusicAudioSource.Play();
	}

    public void Score()
    {
        SFXAudioSource.pitch = 1f;
        SFXAudioSource.PlayOneShot(scoreClip, 2f);
    }

    public void SetButtonClip(){
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(buttonClick, 2f);
	}
	public void PlayAttackClip() {
		SFXAudioSource.pitch = Random.Range(.95f, 1.05f);
		SFXAudioSource.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
    }

    public void PlayWalkClip() {
		SFXAudioSource.pitch = 1f;

		SFXAudioSource.PlayOneShot(walkClips[Random.Range(0, walkClips.Length)], .2f);
	}

	private bool useHighPitch = false;
    public void PlayRunClip()
	{
        SFXAudioSource.pitch = useHighPitch ? 0.75f : 0.7f;
        useHighPitch = !useHighPitch;
        SFXAudioSource.PlayOneShot(runClips[Random.Range(0, runClips.Length)], .1f);
	}

	public void PlayHurtClip() {
		SFXAudioSource.pitch = Random.Range(.95f, 1.05f);
		SFXAudioSource.PlayOneShot(hurtClips[Random.Range(0, hurtClips.Length)]);
	}

	public void PlayDeathClip()
	{
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(deathClip);
	}

	public void PlayOneShot(AudioClip clip, float volume = 1f ) 
	{ 
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(clip, volume);
    }

	public void ChangeMasterVolume(float volume) {
		audioMixer.SetFloat("Master", volume);
		if (volume == -40f){
			audioMixer.SetFloat("Master", -80f);
		}
	}

	public void ChangeMusicVolume(float volume){
		audioMixer.SetFloat("Music", volume);
		if (volume == -40f){
			audioMixer.SetFloat("Music", -80f);
		}
	}

	public void ChangeSFXVolume(float volume){
		audioMixer.SetFloat("SFX", volume);
		if (volume == -40f){
			audioMixer.SetFloat("SFX", -80f);
		}
	}
}
