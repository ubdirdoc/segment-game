/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class SetInteractableAreaSize : MonoBehaviour {
	public GameObject m_area;
	public GameObject m_radarIconScaller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAreaSize(Vector3 newScale) {
		m_area.transform.localScale = newScale;

		if (newScale.x < newScale.y) {
			m_radarIconScaller.transform.localScale = new Vector3(newScale.x, newScale.x, 1);
		} else {
			m_radarIconScaller.transform.localScale = new Vector3(newScale.y, newScale.y, 1);
		}
	}

	public GameObject GetArea() {
		return m_area;
	}
}
