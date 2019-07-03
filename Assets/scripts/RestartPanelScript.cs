/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestartPanelScript : MonoBehaviour {
	public string m_diagrammeModifiedText;
	public string m_diagrammeLoadFailedText;

	public Text m_errorText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowsForDiagramModification() {
		m_errorText.text = m_diagrammeModifiedText;
		gameObject.SetActive(true);
	}

	public void ShowsForLoadError() {
		m_errorText.text = m_diagrammeLoadFailedText;
		gameObject.SetActive(true);
	}
}
