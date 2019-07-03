/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAdviceLoader : MonoBehaviour {
	public static LoadingAdviceLoader instance = null;

	public Sprite m_loadedSprite = null;
	bool m_isLoadingAdviceReady = false;
	bool m_hasLoadingAdviceFailedToLoad = false;

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(LoadImage());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadImage() {
		m_isLoadingAdviceReady = false;

		while (!SEGMentPath.instance.ArePathGenerated()) {
			yield return null;
		}

		Debug.Log(SEGMentPath.instance.GetTitleLoadingPanelPath());

		FileToSprite currentFileToSprite = gameObject.AddComponent<FileToSprite>();
		currentFileToSprite.CreateSpriteFromFile(SEGMentPath.instance.GetTitleLoadingPanelPath());



		while (currentFileToSprite.IsSpriteLoading()) {
			yield return null;
		}

		if (currentFileToSprite.HasLoadingFailed()) {
			m_hasLoadingAdviceFailedToLoad = true;
		} else {
			m_loadedSprite = currentFileToSprite.GetSprite();
			m_isLoadingAdviceReady = true;
		} 

		Destroy(currentFileToSprite);




	}

	public Sprite GetSprite() {
		return m_loadedSprite;
	}

	public bool IsSpriteReady() {
		return m_isLoadingAdviceReady;
	}

	public bool HasSpriteFailedToLoad() {
		return m_hasLoadingAdviceFailedToLoad;
	}
}
