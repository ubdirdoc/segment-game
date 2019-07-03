/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InvisibleWhenNoText : MonoBehaviour {
	public Text m_textToCheck;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Image image = gameObject.GetComponent<Image>();
		
		if (image != null) {
			if (m_textToCheck.text == "") {
				image.enabled = false;
			} else {
				image.enabled = true;
			}

		}

	}
}
