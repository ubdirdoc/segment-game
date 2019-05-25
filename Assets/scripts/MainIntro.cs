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


public class MainIntro : MonoBehaviour {

	public bool m_mustStreamVideo = false;

	public string m_sceneToGoToOnClick = "main";

	public float m_fadeOutTime = 0.5f;

	public Image m_loadingAdviceImage = null;
	public VideoPlayer m_videoPlayer = null;

	bool m_isAlreadyChangingScene = false;

	public CanvasGroup m_canvasGroup = null;

	private bool m_isVideoOver = false;

	public Text m_skipText = null;
	public string m_standaloneText = "Appuyez sur ECHAP pour passer";
	public string m_androidText = "Tapez avec deux doigts pour passer";
	public MakeTextTemporallyAppears m_textBlinking = null;

	// Use this for initialization
	void Start () {
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
	}

	void VideoOver(UnityEngine.Video.VideoPlayer vp) {
		m_isVideoOver = true;
	}

	IEnumerator VideoStartCoroutine() {
		while (!SEGMentPath.instance.ArePathGenerated()) {
			yield return null;
		}

		m_videoPlayer.url = SEGMentPath.instance.GetVideoPath("intro");
		m_videoPlayer.Prepare();

		while (!m_videoPlayer.isPrepared) {
			yield return null;
		}

		m_videoPlayer.Play ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.touchCount >= 2 || (m_isVideoOver/* && Input.GetMouseButtonDown(0)*/)  ) {

			if (!m_isAlreadyChangingScene) {
				m_videoPlayer.Pause();
				m_isAlreadyChangingScene = true;
				StartCoroutine("ChangeSceneCoroutine");
			}

		} else	if (!m_isVideoOver && Input.GetMouseButtonDown(0)) {
			if (m_textBlinking != null) {
				m_textBlinking.Blink();
			}
		} else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape)) {
			if (m_textBlinking != null) {
				m_textBlinking.Blink();
			}
		}
	}

	IEnumerator ChangeSceneCoroutine() {

		if (m_canvasGroup != null) {
			while (m_canvasGroup.alpha < 1.0f) {
				m_canvasGroup.alpha +=  m_fadeOutTime * Time.deltaTime;
				yield return null;
			}
		} 

		while (!LoadingAdviceLoader.instance.IsSpriteReady() && !LoadingAdviceLoader.instance.HasSpriteFailedToLoad()) {
			yield return null;
		}
			
		if (!LoadingAdviceLoader.instance.HasSpriteFailedToLoad()) {
			m_loadingAdviceImage.sprite = LoadingAdviceLoader.instance.GetSprite();

			while (m_loadingAdviceImage.color.a <= 1.0f) {
				Color c = new Color(m_loadingAdviceImage.color.r, m_loadingAdviceImage.color.g, m_loadingAdviceImage.color.b, m_loadingAdviceImage.color.a + m_fadeOutTime * Time.deltaTime);
				m_loadingAdviceImage.color = c;
				yield return null;
			}
		}






		yield return null;
		SceneManager.LoadScene(m_sceneToGoToOnClick);
		yield return null;
	}
}
