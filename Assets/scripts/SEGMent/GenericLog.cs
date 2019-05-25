/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

#if UNITY_EDITOR
using UnityEngine;
#else
using System.Diagnostics;
#endif

#if UNITY_ANDROID || UNITY_WEBGL
using UnityEngine.UI;
#endif


public static class GenericLog
{
#if UNITY_ANDROID || UNITY_WEBGL
	public static Text m_logText = null;

	public static void SetAndroidDebugText(Text logText) {
		m_logText = logText;
	}
#endif
	public static void Log(string logString) {
		#if UNITY_EDITOR
		Debug.Log(logString);
		#else
		Debug.WriteLine(logString);
		#endif

#if UNITY_ANDROID || UNITY_WEBGL
		if (m_logText != null) {
			m_logText.text += "\n" + logString;
		}

		int logTextMaxSize = 5000;

		if (m_logText.text.Length > logTextMaxSize) {
			m_logText.text = m_logText.text.Substring(m_logText.text.Length - logTextMaxSize, logTextMaxSize);
		}
#endif
	}
}


