/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;
using UnityEngine.UI;



public class SEGMentPath : MonoBehaviour {
	public static SEGMentPath instance = null;

	private bool m_pathAreGenerated = false;

	public string m_gameDataDirectoryName = "game/";
	public string m_gameStructureRootFileName = "Game.segment";
	public string m_applicationPath;
	public bool m_mustUseStreamingAssets =  false;

	public string m_soundDirectoryName = "Sounds/";
	public string m_soundExtension = ".wav";

	public string m_diaryDirectoryName = "Diary/";
	public string m_diaryExtension = ".png";

	public string m_videoDirectoryName = "Videos/";
	public string m_videoExtension = ".mp4";

	public string m_UIElementsDirectoryName = "UI/";
	public string m_titleLoadingImageName = "TitleLoading.png";

    StreamWriter writer = new StreamWriter("./debug.txt", true);

    string m_envGameFolder = Environment.GetEnvironmentVariable("SEGMENT_GAME_FOLDER");
    string m_envGameFile = Environment.GetEnvironmentVariable("SEGMENT_GAME_FILE");

	void Awake () {		
		if (instance == null) {
			instance = this;
			StartCoroutine(GeneratePath());
		} else {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*void GeneratePath() {




	}*/

	IEnumerator GeneratePath ()
    {
        // StreamWriter writer = new StreamWriter("./debug.txt", true);
        m_pathAreGenerated = false;

		m_applicationPath = Application.streamingAssetsPath;
        // writer.WriteLine("1: " + m_applicationPath);
        // writer.Flush();

		

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		//	#if !UNITY_EDITOR
		m_applicationPath = "file:///" + m_applicationPath;
		//	#endif
#endif

        WWW testWWW = new WWW (GetSEGMentDiagramPath());

		while (!testWWW.isDone) {
			yield return null;
		}
			
		// writer.WriteLine(" loading : " + GetSEGMentDiagramPath());
		// writer.Flush();

		if (testWWW.error != null) {

			m_applicationPath = Path.GetFullPath (".") + "/";
            // writer.WriteLine("2: " + m_applicationPath);
            // writer.Flush();



#if UNITY_ANDROID
#if !UNITY_EDITOR
		// if (m_mustUseStreamingAssets == true) {
		// m_applicationPath = "jar:file:///" + Application.dataPath + "!/assets/";
		
		// // Objet UI Texte pour afficher la modification
		// textdebug = GameObject.Find("Debug"): 
		// debugString = textdebug.GetComponent<Text>();
		// debugString.text = m_applicationPath;
		
		// } 
		// else {
		// m_applicationPath = "file://///storage/emulated/" + "/";
		m_applicationPath = "file:////mnt/sdcard/data/SEGMent" + "/";
		// }c
#endif
#endif

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_LINUX
			//	#if !UNITY_EDITOR
			m_applicationPath = "file:///" + m_applicationPath;
			//	#endif
#endif



#if UNITY_WEBGL
#if !UNITY_EDITOR
		m_applicationPath = Application.absoluteURL.Substring(0, Application.absoluteURL.LastIndexOf("/")) + "/";
		// GenericLog.Log("WEB GL APP = " + m_applicationPath);
		//return;
#else
		m_applicationPath = "http://raphael.marczak.free.fr/SEGMentWebGLSmall/SEGMent/";
#endif
#endif




        }

	

		m_pathAreGenerated = true;

        // writer.WriteLine("final: " + m_applicationPath);
        // writer.Flush();
        // writer.WriteLine(GetSEGMentDiagramPath());
        // writer.Flush();
    }
		
	public string GetSEGMentGameDataPath() {
        if(m_envGameFolder is null)
          return Path.Combine(m_applicationPath, m_gameDataDirectoryName);
	    else
		  return m_envGameFolder;
		  
	}

	public string GetSEGMentDiagramPath() {
		// For instance : 
		// - GetSEGMentGameDataPath() == /home/jcelerier/segment/segment-game/game/
		// - m_gameStructureRootFileName == Game.segment
		if(m_envGameFile is null) 
		  return Path.Combine(GetSEGMentGameDataPath(), m_gameStructureRootFileName);
		else
		  return m_envGameFile;
	}

	public string GetSoundPath() {
		return Path.Combine(GetSEGMentGameDataPath(), m_soundDirectoryName);
	}

	public string GetSoundPath(string soundName) {
		string resultString = GetSoundPath();
		resultString = Path.Combine(resultString, soundName);
		resultString = resultString + GetSoundExtension();

		return resultString;
	}

	public string GetSoundExtension() {
		return m_soundExtension;
	}

	public string GetDiaryPath() {
		return Path.Combine(GetSEGMentGameDataPath(), m_diaryDirectoryName);
	}

	public string GetDiaryPath(string diaryImage) {
		string resultString = GetDiaryPath();
		resultString = Path.Combine(resultString, diaryImage);
		resultString = resultString + GetDiaryExtension();

		return resultString;

	}

	public string GetDiaryExtension() {
		return m_diaryExtension;
	}

	public string GetVideoExtension() {
		return m_videoExtension;
	}

	public string GetVideoPath() {
		return Path.Combine(GetSEGMentGameDataPath(), m_videoDirectoryName);
	}

	public string GetVideoPath(string videoName) {
		string resultString = GetVideoPath();
		resultString = Path.Combine(resultString, videoName);
		resultString = resultString + GetVideoExtension();

		return resultString;
	}

	public bool ArePathGenerated() {
		return m_pathAreGenerated;
	}

	public string GetUIElementsPath() {
		return Path.Combine(GetSEGMentGameDataPath(), m_UIElementsDirectoryName);
	}

	public string GetTitleLoadingPanelPath() {
		return Path.Combine(GetUIElementsPath(), m_titleLoadingImageName);
	}


}
