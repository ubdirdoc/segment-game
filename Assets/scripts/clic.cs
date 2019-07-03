/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */


using UnityEngine;
using System.Collections;

public class clic : MonoBehaviour {

	public AudioClip m_soundToPlay;


	void OnMouseDown() {
		AudioManager.instance.PlaySound (m_soundToPlay);
	}
}
