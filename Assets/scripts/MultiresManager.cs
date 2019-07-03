/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;

public class MultiresManager : MonoBehaviour {
	public SpriteRenderer m_mainSprite = null;
	public SpriteRenderer m_backgroundSprite = null;

	public float m_scaleFactor = 0.0f;

	private bool m_isLoadingNewImage = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//UpdateScene();
	}

	void UpdateScene() {
		if(m_mainSprite == null) return;
		if(m_mainSprite.sprite == null) return;
		
		m_mainSprite.transform.localScale=new Vector3(1,1,1);
		float width = m_mainSprite.sprite.bounds.size.x;
		float height = m_mainSprite.sprite.bounds.size.y;
		float worldScreenHeight = Camera.main.orthographicSize*2f;
		float worldScreenWidth = worldScreenHeight/Screen.height*Screen.width;
		
		Vector3 localScale = transform.localScale;
		
		float newWidthFactor = worldScreenWidth / width;
		float newHeighFactor = worldScreenHeight / height;
		
		if (newWidthFactor > newHeighFactor) {
			m_scaleFactor = newHeighFactor;
		} else {
			m_scaleFactor = newWidthFactor;
		}
		
		localScale.x = m_scaleFactor;
		localScale.y = m_scaleFactor;
		
		m_mainSprite.transform.localScale=localScale;
		
		//print ("Actual Sprite Width = " + width * m_scaleFactor);
		
		
		if(m_backgroundSprite == null) return;
		if(m_backgroundSprite.sprite == null) return;
		
		m_backgroundSprite.transform.localScale=new Vector3(1,1,1);
		width=m_backgroundSprite.sprite.bounds.size.x;
		height=m_backgroundSprite.sprite.bounds.size.y;
		worldScreenHeight=Camera.main.orthographicSize*2f;
		worldScreenWidth=worldScreenHeight/Screen.height*Screen.width;
		
		localScale = transform.localScale;
		
		newWidthFactor = worldScreenWidth / width;
		newHeighFactor = worldScreenHeight / height;
		
		localScale.x = newWidthFactor;
		localScale.y = newHeighFactor;
		
		m_backgroundSprite.transform.localScale=localScale;
	}

	public void ChangeImageFromURL(string imageURL) {
		StartCoroutine(ChangeImageFromURLCoroutine(imageURL));
	}

	IEnumerator ChangeImageFromURLCoroutine(string imageURL) {
		m_isLoadingNewImage = true;
		FileToSprite currentFileToSprite = gameObject.AddComponent<FileToSprite>();

		currentFileToSprite.CreateSpriteFromFile(imageURL);

		while (currentFileToSprite.GetSprite() == null) {
			yield return null;
		}

		if (m_mainSprite.sprite != null) {
			Destroy(m_mainSprite.sprite.texture);
		}

		m_mainSprite.sprite = currentFileToSprite.GetSprite();

		Destroy(m_mainSprite.gameObject.GetComponent<BoxCollider2D>());
		m_mainSprite.gameObject.AddComponent<BoxCollider2D>();

		Destroy(currentFileToSprite);

		UpdateScene();
		m_isLoadingNewImage = false;
	}

	public SpriteRenderer GetMainSpriteRenderer() {
		return m_mainSprite;
	}

	public bool IsLoadingNewImage() {
		return m_isLoadingNewImage;
	}

}
