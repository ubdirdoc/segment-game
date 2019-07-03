/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System;
using System.Collections.Generic;
using UnityEngine;


public class GifPreLoader : MonoBehaviour
{
	public static GifPreLoader instance = null;
	public bool m_mustPreload = true;

	Dictionary<string, List<GifSprite>> m_fileNameToGifSprite;

	void Awake () {		
		if (instance == null) {
			instance = this;
			m_fileNameToGifSprite = new Dictionary<string, List<GifSprite>>();
		} else {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public void SetNewGifSpriteList(string filename, List<GifSprite> list) {
		if (!m_fileNameToGifSprite.ContainsKey(filename)) {
			m_fileNameToGifSprite[filename] = list;
		}
	}

	public List<GifSprite> GetSpriteList(string filename) {
		if (!m_fileNameToGifSprite.ContainsKey(filename)) {
			return null;
		} else {
			return m_fileNameToGifSprite[filename];
		}
	}

	public bool MustPreload() {
		return m_mustPreload;
	}

}


