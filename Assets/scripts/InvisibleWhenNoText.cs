/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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
