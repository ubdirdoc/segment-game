/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System;

using UnityEngine;


public class MonoBehaviourForCoroutineSingleton : MonoBehaviour
{
	public static MonoBehaviourForCoroutineSingleton instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		//DontDestroyOnLoad (gameObject);
	}
}
