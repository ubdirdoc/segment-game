/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.UI;

public class MetricFrequency {
	public float m_frequency;
	public float m_lastTimeCalled;
}

public class MetricLogger : MonoBehaviour {
	//public Text m_debutText;

	public static MetricLogger instance = null;
	public string m_playerID = "";

	public string m_metricDirectory = "metrics/";
	public string m_metricPath = "";

	public enum logType {NO_LOG, CONSOLE_LOG, FILE_LOG, PHP_LOG};

	public int m_sessionID;

	public string m_separator = ";";

	public logType m_typeOfLog = logType.NO_LOG;
	public string m_logFilename = "log.txt";

	public float m_timeInMsFromBeginning = 0.0f;

	public string m_currentDate;

	public string m_url = "";

	public Dictionary<string, MetricFrequency> m_frequencyManager;

	void Awake () {		
		if (instance == null) {
			instance = this;
			m_sessionID = (int) Random.Range(0, int.MaxValue);
			m_frequencyManager = new Dictionary<string, MetricFrequency>();

			//m_metricPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1) + m_metricDirectory;

			m_metricPath = Path.GetFullPath(".") + "/" + m_metricDirectory;

			m_currentDate = System.DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
		} else {
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad (gameObject);
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_timeInMsFromBeginning += Time.deltaTime;
	}

	public void Log(string metricName, params object[] values) {
		if (m_playerID == "") {
			return;
		}

		if (m_frequencyManager.ContainsKey(metricName)) {
			MetricFrequency currentFrequency = m_frequencyManager[metricName];

			if (currentFrequency.m_lastTimeCalled + currentFrequency.m_frequency < m_timeInMsFromBeginning) {
				m_frequencyManager[metricName].m_lastTimeCalled = m_timeInMsFromBeginning;
			} else {
				return;
			}
		}

		string args = "";

		if (values.Length !=0) {
			for (int i = 0; i < values.Length - 1; ++i) {
				args += values[i] + m_separator;
			}
			
			args += values[values.Length - 1];
		}
	

		switch (m_typeOfLog) {
		case logType.NO_LOG:
			break;
		case logType.CONSOLE_LOG:
			//print(m_sessionID + " | " + m_currentDate + " | " + m_timeInMsFromBeginning + " | " + m_playerID + " | " + metricName + " | " + args);
			print(m_timeInMsFromBeginning + " | " + metricName + " | " + args + " | " + m_playerID + " | " + m_sessionID + " | " + m_currentDate);
			break;
		case logType.FILE_LOG:
#if UNITY_WEBPLAYER
			/*object[] parametres = new object[2]{metricName, args};
			
			StartCoroutine("Phpcoroutine", parametres);*/
#else
			StreamWriter metricFile = new StreamWriter(m_metricPath + m_playerID + "_" + m_sessionID + ".csv", true); 
			string stringArgs = args;
			
			stringArgs = stringArgs.Replace ('#', '$');
			
			stringArgs = stringArgs.Replace (' ', '_');
			
			metricFile.WriteLine(m_timeInMsFromBeginning + ";" + metricName + ";" + m_playerID + ";" + m_sessionID + ";" + m_currentDate + ";" + stringArgs);
			metricFile.Close();
#endif
			break;
		}
	}

	/*
	    IEnumerator Phpcoroutine(object[] args) {
		while (GlobalVariables.instance == null || m_playerID == null) {
			yield return null;
		}

		if (GlobalVariables.instance.m_playerID != "void") {
			string stringArgs = (string) args [1];
			
			stringArgs = stringArgs.Replace ('#', '$');
			
			stringArgs = stringArgs.Replace (' ', '_');
			
			string phpUrl = GlobalVariables.instance.m_applicationPath + "addline.php?file=" + GlobalVariables.instance.m_playerID + "_" + m_sessionID + ".txt" + "&metric=" + m_timeInMsFromBeginning + ";" + args[0] + ";" + stringArgs + ";" + GlobalVariables.instance.m_playerID + ";" + m_sessionID + ";" + m_currentDate;
			
//			print (phpUrl);
			
			WWW www = new WWW(phpUrl);
			//print("http://raphael.marczak.free.fr/LaBRI_AST/GaME8/addline.php?metric=" + m_timeInMsFromBeginning + ";" + args[0] + ";" + stringArgs + ";" + m_playerID + ";" + m_sessionID + ";" + m_currentDate);
			while(!www.isDone && string.IsNullOrEmpty(www.error)) {
				yield return null;
			}
		//	print (www.error);
		}
	}
	
	//	public void Log(string metricName, string value) {
//		string[] values = new string[1];
//
//		values[0] = value;
//
//		Log (metricName, values);
//	}
*/
	public void SetMetricFrequency(string metricName, float frequencyInSec) {
		if (m_frequencyManager.ContainsKey(metricName)) {
			m_frequencyManager[metricName].m_frequency = frequencyInSec;
		} else {
			MetricFrequency currentFrequency = new MetricFrequency();
			
			currentFrequency.m_frequency = frequencyInSec;
			currentFrequency.m_lastTimeCalled = -frequencyInSec;

			m_frequencyManager.Add(metricName, currentFrequency);
		}
	}

	public void SetPlayerID(string playerID) {
		m_playerID = playerID;
	}


}
