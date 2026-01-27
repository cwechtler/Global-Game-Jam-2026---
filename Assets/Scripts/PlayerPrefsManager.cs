using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
	const string MUSIC_VOLUME_KEY = "music_volume";
	const string SFX_VOLUME_KEY = "sfx_volume";
	const string INSTRUCTIONS_TOGGLE_KEY = "instructions_toggle";

	const string PLAYER_NAME_KEY = "player_name";
	const string HIGH_SCORES_KEY = "high_scores";

	public static string PlayerNameKey => PLAYER_NAME_KEY;
	public static string HighScoresKey => HIGH_SCORES_KEY;

	public static bool DoesKeyExist(string key) {
		if (PlayerPrefs.HasKey(key)) {
			return true;
		}
		else {
			return false;
		}
	}

	public static void SetMasterVolume(float volume) {
		if (volume >= -40f && volume <= 1.000001f)
		{
			PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Master volume out of range");
		}
	}

	public static float GetMasterVolume(){
		return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
	}

	public static void SetMusicVolume(float volume){
		if (volume >= -40f && volume <= 1.000001f)
		{
			PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
		} else{
			Debug.LogError("Music volume out of range");
		}
	}

	public static float GetMusicVolume(){
		return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
	}

	public static void SetSFXVolume(float volume){
		if (volume >= -40f && volume <= 1.000001f){
			PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
		} else{
			Debug.LogError("SFX volume out of range");
		}
	}

	public static float GetSFXVolume(){
		return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
	}

	public static void SetPlayerName(string playerName)
	{
		PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
	}

	public static string GetPlayerName()
	{
		return PlayerPrefs.GetString(PLAYER_NAME_KEY);
	}

	public static void SetHighScores(string playerName)
	{
		PlayerPrefs.SetString(HIGH_SCORES_KEY, playerName);
	}

	public static string GetHighScores()
	{
		return PlayerPrefs.GetString(HIGH_SCORES_KEY);
	}

	public static void SetInstructionsToggle(bool isOn)
	{
		int tinyInt = isOn ? 1 : 0;
		PlayerPrefs.SetInt(INSTRUCTIONS_TOGGLE_KEY, tinyInt);
	}
	public static bool GetInstructionsToggle()
	{
		if (PlayerPrefs.GetInt(INSTRUCTIONS_TOGGLE_KEY) == 1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static void DeleteAllPlayerPrefs() {
		PlayerPrefs.DeleteAll();
	}
}
