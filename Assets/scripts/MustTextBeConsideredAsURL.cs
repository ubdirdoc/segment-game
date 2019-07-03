/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;

public class MustTextBeConsideredAsURL : MonoBehaviour {

	private bool m_textIsAnURL = false;

	public void SetTextAsBeingAnURL(bool isURL) {
		m_textIsAnURL = isURL;
	}

	public bool IsMatchingTextAnURL() {
		return m_textIsAnURL;
	}
}
