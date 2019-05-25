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

public class MouseCursorGlobal : MonoBehaviour {
	public static MouseCursorGlobal instance;

	public Texture2D m_interactableTexture = null;


	// Use this for initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public Texture2D GetInteractableTexture() {
		return m_interactableTexture;
	}



	public Vector2 GetMiddleHotSpot() {

		Vector2 middleHotSpot = Vector2.zero;

		middleHotSpot.x = m_interactableTexture.width / 2.0f;
		middleHotSpot.y = m_interactableTexture.height / 2.0f;

		return middleHotSpot;
	}

	public Vector2 GetTopMiddleHotSpot() {
		Vector2 topMiddleHotSpot = Vector2.zero;

		topMiddleHotSpot.x = m_interactableTexture.width / 2.0f;
		topMiddleHotSpot.y = 0;

		return topMiddleHotSpot;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
