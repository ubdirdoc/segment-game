/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;

public class SoundLauncher : MonoBehaviour {

	private AudioSource m_soundStream = null;
	private bool m_soundIsReadyToPlay;

	// Use this for initialization
	void Awake () {
		m_soundStream = GetComponent<AudioSource>();
		m_soundIsReadyToPlay = false;
	}
	
	public void StopSound() {
		if (m_soundStream == null) {
			return;
		}

		m_soundStream.Stop();
	}

	public void PlayLoadedSound() {
		if (m_soundIsReadyToPlay) {
			m_soundStream.Play();
		}
	}
	

	public void PlayLoadedSoundWhenReady() {
		StartCoroutine("PlaySoundWhenReadyCoroutine");
	}
	
	public void LoadSoundByURL(string URL, bool loop) {
		m_soundIsReadyToPlay = false;
		if (m_soundStream == null) {
			return;
		}
		
		
		m_soundStream.loop = loop;
		StartCoroutine("LoadSoundByURLCoroutine", URL);
	}

	IEnumerator LoadSoundByURLCoroutine(string path)
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
		//		print("loading " + path);
		m_soundStream.clip = www.GetAudioClip(false);
		
		while(m_soundStream.clip.loadState != AudioDataLoadState.Loaded)
			yield return null;
		//		print("done loading");
		m_soundIsReadyToPlay = true;
		//m_soundStream.clip = clip;
		//m_soundStream.Play();
	}

	IEnumerator PlaySoundWhenReadyCoroutine() {
		while (!m_soundIsReadyToPlay) {
			yield return null;
		}

		m_soundStream.Play();
	}

	public bool IsSoundReadyToPlay() {
		return m_soundIsReadyToPlay;
	}
}
