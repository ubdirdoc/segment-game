/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainSplash : MonoBehaviour {
	public float m_fadeInOutTime = 1.0f;
	public UIManager m_UIManager = null;

	public string m_sceneNameToChangeTo = "";

	bool m_mustSwitchScene = false;

	// Use this for initialization
	void Start () {
		m_UIManager.SetFadeInOutTime(m_fadeInOutTime);
		m_UIManager.SceneFadeIn();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			m_UIManager.SceneFadeOut();
			m_mustSwitchScene = true;
		}

		if (m_mustSwitchScene && m_UIManager.IsSceneFadedOut()) {
			SceneManager.LoadScene(m_sceneNameToChangeTo);
			//Application.LoadLevel(m_sceneNameToChangeTo);
		}
	}
}
