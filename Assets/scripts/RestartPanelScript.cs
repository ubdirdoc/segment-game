/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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
