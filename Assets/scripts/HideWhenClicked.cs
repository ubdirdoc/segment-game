/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWhenClicked : MonoBehaviour {
	public GameObject m_objectToHide = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hide() {
		if (m_objectToHide != null) {
			m_objectToHide.SetActive(false);
		}
	}
}
