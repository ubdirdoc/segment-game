/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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
