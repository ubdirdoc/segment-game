/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ChangeTextOnCollisionByCollidedText : MonoBehaviour {
	public Text m_textToChange;

	public List<string> m_tagsToIgnore = new List<string>();

	void OnTriggerEnter2D(Collider2D other) {
		foreach (string tag in m_tagsToIgnore) {
			if (other.gameObject.tag == tag) {
				return;
			}
		}

		Text otherText = other.gameObject.GetComponent<Text>();

		if (otherText != null) {
			m_textToChange.text = otherText.text;
		}
		
	}
}
