/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class GameObjectFollow : MonoBehaviour {
	public GameObject m_followingObject = null; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ChangeFollowingObjectPosition(transform.position.x, transform.position.y);
	}

	public void ChangeFollowingObjectPosition(float x, float y) {
		m_followingObject.transform.position = new Vector3(x, y, m_followingObject.transform.position.z);
	}

}
