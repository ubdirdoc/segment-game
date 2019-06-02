﻿/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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