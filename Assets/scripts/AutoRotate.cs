/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
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
