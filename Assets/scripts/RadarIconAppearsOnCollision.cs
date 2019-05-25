/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class RadarIconAppearsOnCollision : MonoBehaviour {
	public float m_onScreenTime = 2.0f;

	float m_appearanceDate = 0f;
	bool isLogoOnScreen = false;

	Animator m_animator = null;
	SpriteRenderer m_iconSprite = null;
	BoxCollider2D m_boxCollider = null;

	Vector3 m_boxColliderInitialSize = Vector3.zero;

	// Use this for initialization
	void Awake () {
		m_animator = gameObject.GetComponent<Animator>();
		m_iconSprite = gameObject.GetComponent<SpriteRenderer>();
		m_boxCollider = gameObject.GetComponent<BoxCollider2D>();


		if (m_iconSprite != null) {
			m_iconSprite.enabled = false;
		}

		if (m_boxCollider != null) {
			m_boxColliderInitialSize = new Vector2(m_boxCollider.size.x * gameObject.transform.localScale.x, m_boxCollider.size.y * gameObject.transform.localScale.y);
		}

		//Appears();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLogoOnScreen && (Time.time > (m_appearanceDate + m_onScreenTime))) {
			Disappears();
		}

		// Adapts the size of the collider so that it is not a single "dot" in the middle of the object
		// (case when the localscale of the icon is only 0.0001)
		// but represents then the bounding box of the icon when "big"
		if (m_boxCollider != null) {
			m_boxCollider.size = new Vector2(m_boxColliderInitialSize.x / gameObject.transform.localScale.x, m_boxColliderInitialSize.y / gameObject.transform.localScale.y);

			//print ((int)m_boxColliderInitialSize.x);
		}
	}

	void Appears() {
		// Insure that the logo will be displayed with a ratio respected.
		Vector3 currentLocalScale = gameObject.transform.localScale;

		if (currentLocalScale.x < currentLocalScale.y) {
			gameObject.transform.localScale = new Vector3(currentLocalScale.x, currentLocalScale.x, 1);
		} else {
			gameObject.transform.localScale = new Vector3(currentLocalScale.y, currentLocalScale.y, 1);
		}
		// **** //

		if ((m_animator != null) && (m_iconSprite != null) && !isLogoOnScreen) {
			isLogoOnScreen = true;

			m_iconSprite.enabled = true;
			m_animator.SetTrigger("IconAppear");

			m_appearanceDate = Time.time;
		}
	}

	void Disappears() {
		if ((m_animator != null)) {
			isLogoOnScreen = false;

			m_animator.SetTrigger("IconDisappear");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//print("COLLISION");

		if (other.CompareTag("Radar")) {
			Appears();
		}
		
	}
}
