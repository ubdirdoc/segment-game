/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance = null;

	public AudioSource m_soundStream;
	public AudioSource m_musicStream;
	public AudioSource m_secondarySoundStream;

	private bool m_isMusicLoaded = false;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySound(AudioClip soundClipToPlay, float volume = 1.0f, float pitch = 1.0f) {
		m_soundStream.pitch = pitch;
		m_soundStream.volume = volume;
		m_soundStream.clip = soundClipToPlay;
		m_soundStream.Play ();
	}

	public void PlaySoundAsSecondaryStream(AudioClip soundClipToPlay, float volume = 1.0f, float pitch = 1.0f) {
		m_secondarySoundStream.pitch = pitch;
		m_secondarySoundStream.volume = volume;
		m_secondarySoundStream.clip = soundClipToPlay;
		m_secondarySoundStream.Play ();
	}

	public void PlayMusicByURL(string URL, bool mustLoop) {
		m_musicStream.loop = mustLoop;
		StartCoroutine("PlayMusicByURLCoroutine", URL);
	}

	public void StopMusic() {
		m_musicStream.Stop();
	}

	public void FadeOutMusic(float timeToFadeOut) {
		StartCoroutine(FadeOutMusicCoroutine(timeToFadeOut));
	}

	IEnumerator FadeOutMusicCoroutine(float timeToFadeOut) {
		float startVolume = m_musicStream.volume;

		while (m_musicStream.volume >= 0) {
			m_musicStream.volume -= Time.deltaTime * startVolume / timeToFadeOut;
			yield return null;
		}

		m_musicStream.Stop();
		m_musicStream.volume = startVolume;
	}

	IEnumerator PlayMusicByURLCoroutine(string path)
	{
		m_isMusicLoaded = false;
		#if UNITY_WEBGL
		WWW www = new WWW(path);
		#else
		//Debug.Log(path);
		string filePath = /*"file:///" +*/ path;
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		filePath = filePath.Replace("\\", "/");
		#endif
		WWW www = new WWW(filePath);
		//Debug.Log(www.text);
		#endif

		while (!www.isDone) {
			yield return null;
		}

		GenericLog.Log("loading " + path);
		AudioClip clip = www.GetAudioClip(false);

		while(clip.loadState != AudioDataLoadState.Loaded)
			yield return null;
		GenericLog.Log("done loading");

		m_musicStream.clip = clip;
		m_musicStream.Play();
		m_isMusicLoaded = true;
	}

	public bool isMusicLoaded() {
		return m_isMusicLoaded;
	}

	public void PlaySoundByURL(string URL) {
		m_soundStream.loop = false;
		StartCoroutine("PlaySoundByURLCoroutine", URL);
	}
	
	public void StopSound() {
		m_soundStream.Stop();
	}
	
	IEnumerator PlaySoundByURLCoroutine(string path)
	{
		#if UNITY_WEBGL
		WWW www = new WWW(path);
		#else
		//Debug.Log(path);
		string filePath = /*"file:///" +*/ path;
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		filePath = filePath.Replace("\\", "/");
		#endif
		WWW www = new WWW(filePath);
		//Debug.Log(www.text);
		#endif

		while (!www.isDone) {
			yield return null;
		}

		GenericLog.Log("loading " + path);
		AudioClip clip = www.GetAudioClip(false);
		
		while(clip.loadState != AudioDataLoadState.Loaded)
			yield return null;
		GenericLog.Log("done loading");
		
		m_soundStream.clip = clip;
		m_soundStream.Play();
	}

	public void Mute(bool mustMute) {
		m_musicStream.mute = mustMute;
	}
}
