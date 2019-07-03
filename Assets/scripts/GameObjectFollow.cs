/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
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
