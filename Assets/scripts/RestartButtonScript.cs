/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartButtonScript : MonoBehaviour {
	public string m_sceneName = "main";
	public GameObject m_panel;
	public UIManager m_UIManager;
	bool m_mustSwitchScene = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_mustSwitchScene && m_UIManager.IsSceneFadedOut()) {
			//Application.LoadLevel(m_sceneName);

			SceneManager.LoadScene(m_sceneName);


			m_mustSwitchScene = false;
			m_panel.SetActive(false);
		}
	
	}

	public void RestartScene() {
		m_UIManager.SceneFadeOut();
		m_mustSwitchScene = true;


	}
}
