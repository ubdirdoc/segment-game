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
using UnityEngine.UI;

public class SpriteBlink : MonoBehaviour {
	public Image m_imageToChangeColor;

	public int m_nbOfBlink;
	public Color m_colorToBlinkTo;
	public float m_speed;

	private bool m_isAlreadyBlinking = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Blink() {
		StartCoroutine(BlinkCoroutine());
	}

	IEnumerator BlinkCoroutine() {
		if (!m_isAlreadyBlinking) {
			m_isAlreadyBlinking = true;

			Color initialColor = m_imageToChangeColor.color;

			for (int i = 0; i < m_nbOfBlink; i++) {
				float lerpValue = 0;

				while (lerpValue < 1) {
					lerpValue += Time.deltaTime * m_speed;
					m_imageToChangeColor.color = Color.Lerp(initialColor, m_colorToBlinkTo, lerpValue);
					yield return null;
				}

				lerpValue = 1;

				while (lerpValue > 0) {
					lerpValue -= Time.deltaTime * m_speed;
					m_imageToChangeColor.color = Color.Lerp(initialColor, m_colorToBlinkTo, lerpValue);
					yield return null;
				}
			}

			m_isAlreadyBlinking = false;
		}
		yield return null;

	}
}
