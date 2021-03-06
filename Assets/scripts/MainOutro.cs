/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainOutro : MonoBehaviour {
	public bool m_mustStreamVideo = false;

	public float m_fadeInTime = 0.5f;

	public Image m_fadeInImage = null;
	public VideoPlayer m_videoPlayer = null;

	bool m_isAlreadyChangingScene = false;

	private bool m_isVideoOver = false;

	public Text m_skipText = null;
	public string m_standaloneText = "Appuyez sur ECHAP pour quitter";
	public string m_androidText = "Tapez avec deux doigts pour quitter";
	public MakeTextTemporallyAppears m_textBlinking = null;

	// Use this for initialization
	void Start () {
//		MetricLogger.instance.Log("CREDIT_START", false);
		m_videoPlayer.Pause();

		StartCoroutine(StartSceneCoroutine());
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.touchCount >= 2 || (m_isVideoOver && Input.GetMouseButtonDown(0))  ) {

			//	MetricLogger.instance.Log("SEGMENT_RUNNING", false);
			Application.Quit();

			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif

		} else if (!m_isVideoOver && Input.GetMouseButtonDown(0)) {
			if (m_textBlinking != null) {
				m_textBlinking.Blink();
			}
		} else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape)) {
			if (m_textBlinking != null) {
				m_textBlinking.Blink();
			}
		}
	}

	void VideoOver(UnityEngine.Video.VideoPlayer vp) {
		m_isVideoOver = true;
	}

	IEnumerator StartSceneCoroutine() {


		while (m_fadeInImage.color.a >= 0.0f) {
			Color c = new Color(m_fadeInImage.color.r, m_fadeInImage.color.g, m_fadeInImage.color.b, m_fadeInImage.color.a - m_fadeInTime * Time.deltaTime);
			m_fadeInImage.color = c;
			yield return null;
		}
		yield return null;
		if (m_videoPlayer != null) {
			m_videoPlayer.loopPointReached += VideoOver;
			if (!m_mustStreamVideo) {
				m_videoPlayer.Play();
			} else {
				StartCoroutine(VideoStartCoroutine());
			}

			if (m_skipText != null) {
				m_skipText.text = m_standaloneText;
				#if UNITY_ANDROID
				m_skipText.text = m_androidText;
				#endif
			}
		}
		yield return null;
	}

	IEnumerator VideoStartCoroutine() {
		while (!SEGMentPath.instance.ArePathGenerated()) {
			yield return null;
		}

		m_videoPlayer.url = SEGMentPath.instance.GetVideoPath("outro");
		m_videoPlayer.Play ();
	}

}
