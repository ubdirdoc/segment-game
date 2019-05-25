/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class DiaryManagement : MonoBehaviour {
	byte[] m_imageData;

	public Image m_backgroundImage = null;

	public bool m_hasAlreadyAnImage = false;
	public bool m_isLoadingImage = false;

	public GameObject m_diaryButton = null;
	public GameObject m_diaryPanel = null;

	public CanvasGroup m_diaryCanvasGroup = null;

	public float m_fadeInOutSpeed = 0.5f;

	bool m_mustStopFadeIn = false;
	bool m_mustStopFadeOut = true;

	bool m_diaryIsDisplayed = false;

	public SpriteBlink m_buttonBlink = null;
	public AudioSource m_diaryHighlightSound = null;

	public AudioSource m_diaryOpeningSound = null;

	// Use this for initialization
	void Awake () {
		m_hasAlreadyAnImage = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_diaryButton != null) {
			if (!m_hasAlreadyAnImage && m_diaryButton.activeSelf) {
				m_diaryButton.SetActive(false);
			} else if (m_hasAlreadyAnImage && !m_diaryButton.activeSelf) {
				m_diaryButton.SetActive(true);
			}
		}

		if (Input.GetKeyDown(KeyCode.B)) {
			HighlightNewDiaryEntry();
		}
	}

	public void HighlightNewDiaryEntry() {
		if (m_buttonBlink != null) {
			m_buttonBlink.Blink();
		}

		if (m_diaryHighlightSound != null) {
			if (!m_diaryHighlightSound.isPlaying) {
				m_diaryHighlightSound.Play();
			}
		}
	}

	public bool IsCurrentlyLoadingImage() {
		return m_isLoadingImage;
	}

	public void AddDiaryImage(string filename) {
		if (m_backgroundImage == null) {
			return;
		}

		StartCoroutine(LoadDiaryPage(filename));
	}

	IEnumerator LoadDiaryPage(string filename) {
		m_isLoadingImage = true;

		FileToSprite currentFileToSprite = gameObject.AddComponent<FileToSprite>();
		currentFileToSprite.CreateSpriteFromFile(filename);

		while (currentFileToSprite.IsSpriteLoading()) {
			yield return null;
		}

		Sprite sprite = currentFileToSprite.GetSprite();

		if (sprite != null) {
			
			if (!m_hasAlreadyAnImage) {
				m_backgroundImage.sprite = sprite;
				m_hasAlreadyAnImage = true;
			} else {
				MergeToBackgroundImage(sprite);
			}
		}

		Destroy(currentFileToSprite);
		m_isLoadingImage = false;
		yield return null;
	}

	// based on https://sushanta1991.blogspot.com/2016/09/how-to-merge-2-texture-in-unity.html
	public void MergeToBackgroundImage(Sprite imgToAdd) {

		if ((imgToAdd.texture.width != m_backgroundImage.sprite.texture.width) ||
			(imgToAdd.texture.height != m_backgroundImage.sprite.texture.height)) {
			GenericLog.Log("Images sizes do not match for diary merge !!! All images should be the exact same size");
			return;
		}

		Color[] cols1 = m_backgroundImage.sprite.texture.GetPixels();
		Color[] cols2 = imgToAdd.texture.GetPixels();
	

		for(var i = 0; i < cols1.Length; ++i)
		{
			float rOut = (cols2[i].r * cols2[i].a) + (cols1[i].r * (1 - cols2[i].a));
			float gOut = (cols2[i].g * cols2[i].a) + (cols1[i].g * (1 - cols2[i].a));
			float bOut = (cols2[i].b * cols2[i].a) + (cols1[i].b * (1 - cols2[i].a));
			float aOut = cols2[i].a + (cols1[i].a * (1 - cols2[i].a));

			cols1[i] = new Color(rOut,gOut,bOut,aOut);
		}

		m_backgroundImage.sprite.texture.SetPixels(cols1);
		m_backgroundImage.sprite.texture.Apply();
	}

	public void TogglePanelOnOff() {
		if (m_diaryPanel != null) {
			if (m_diaryIsDisplayed) {
				StartCoroutine(DisableDiaryPanel());
			} else {
				StartCoroutine(EnableDiaryPanel());

				if (m_diaryOpeningSound != null) {
					if (!m_diaryOpeningSound.isPlaying) {
						m_diaryOpeningSound.Play();
					}
				}
			}



			m_diaryIsDisplayed = !m_diaryIsDisplayed;
		}
	}

	IEnumerator EnableDiaryPanel() {
		m_mustStopFadeIn = false;
		m_mustStopFadeOut = true;

		m_diaryPanel.SetActive(true);

		while (!m_mustStopFadeIn && m_diaryCanvasGroup.alpha < 1.0f) {
			m_diaryCanvasGroup.alpha = m_diaryCanvasGroup.alpha + Time.deltaTime * m_fadeInOutSpeed;

			if (m_diaryCanvasGroup.alpha > 1.0f) {
				m_diaryCanvasGroup.alpha = 1.0f;
			}

			yield return null;
		}

		yield return null;
	}

	IEnumerator DisableDiaryPanel() {
		m_mustStopFadeOut = false;
		m_mustStopFadeIn = true;

		while (!m_mustStopFadeOut && m_diaryCanvasGroup.alpha > 0.0f) {
			m_diaryCanvasGroup.alpha = m_diaryCanvasGroup.alpha - Time.deltaTime * m_fadeInOutSpeed;

			if (m_diaryCanvasGroup.alpha < 0.0f) {
				m_diaryCanvasGroup.alpha = 0.0f;
			}

			yield return null;
		}

		if (!m_mustStopFadeOut) {
			m_diaryPanel.SetActive(false);
		}

		yield return null;
	}


}
