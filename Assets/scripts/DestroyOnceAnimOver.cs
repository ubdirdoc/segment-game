/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;

public class DestroyOnceAnimOver : MonoBehaviour {
	
	
	// Update is called once per frame
	void Update () {
		Animation includedAnimation = gameObject.GetComponent<Animation>();

		if (includedAnimation != null) {
			if (!includedAnimation.isPlaying) {
				Destroy(gameObject);
			}
		}
	}
}
