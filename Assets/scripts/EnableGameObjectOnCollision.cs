/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnableGameObjectOnCollision : MonoBehaviour {
	public GameObject m_objectToEnableOnCollision = null;

	public List<GameObject> m_objectsToIgnore = new List<GameObject>();
	public List<string> m_tagsToIgnore = new List<string>();

	private GameObject collidedObject = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (collidedObject == null) {
			m_objectToEnableOnCollision.SetActive(false);
		} 
	}

	void OnTriggerEnter2D(Collider2D other) {
		foreach (GameObject currentGameObject in m_objectsToIgnore) {
			if (other.gameObject == currentGameObject) {
				return;
			}
		}

		foreach (string tag in m_tagsToIgnore) {
			if (other.gameObject.tag == tag) {
				return;
			}
		}

		m_objectToEnableOnCollision.SetActive(true);
		collidedObject = other.gameObject;

		MetricLogger.instance.Log("ON_ITEM", true, other.GetComponent<SpriteRenderer>().sprite.name);
		
	}

	void OnTriggerExit2D(Collider2D other) {
		foreach (GameObject currentGameObject in m_objectsToIgnore) {
			if (other.gameObject == currentGameObject) {
				return;
			}
		}

		foreach (string tag in m_tagsToIgnore) {
			if (other.gameObject.tag == tag) {
				return;
			}
		}
		
		m_objectToEnableOnCollision.SetActive(false);

		MetricLogger.instance.Log("ON_ITEM", false, other.GetComponent<SpriteRenderer>().sprite.name);
	}
}
