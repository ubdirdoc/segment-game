/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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
