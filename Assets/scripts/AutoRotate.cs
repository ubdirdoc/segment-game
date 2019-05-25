/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
	public enum ROTATION_AXIS {X, Y, Z};
	public float m_speed = 10.0f;
	public ROTATION_AXIS m_rotationAxis = ROTATION_AXIS.Z;

	// Update is called once per frame
	void Update () {
		RectTransform currentObjectRectTransform = GetComponent<RectTransform>();

		if (currentObjectRectTransform != null) {
			if (m_rotationAxis == ROTATION_AXIS.X) {
				currentObjectRectTransform.Rotate(new Vector3(m_speed * Time.deltaTime, 0, 0));
			}
			if (m_rotationAxis == ROTATION_AXIS.Y) {
				currentObjectRectTransform.Rotate(new Vector3(0, m_speed * Time.deltaTime, 0));
			}
			if (m_rotationAxis == ROTATION_AXIS.Z) {
				currentObjectRectTransform.Rotate(new Vector3(0, 0, m_speed * Time.deltaTime));
			}
		}
	}
}
