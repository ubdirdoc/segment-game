/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;

public class FollowPointer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		//Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();
		//rigidBody.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
