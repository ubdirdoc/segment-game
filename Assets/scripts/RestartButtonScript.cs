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
