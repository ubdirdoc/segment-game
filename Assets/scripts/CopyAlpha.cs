/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyAlpha : MonoBehaviour {
	public CanvasRenderer m_rendererToCopyTheAlphaFrom = null;

	// Update is called once per frame
	void Update () {
		CanvasRenderer currentRenderer = gameObject.GetComponent<CanvasRenderer>();

		if (currentRenderer != null && m_rendererToCopyTheAlphaFrom != null) {
			currentRenderer.SetAlpha(m_rendererToCopyTheAlphaFrom.GetAlpha());
		}
	}
}
