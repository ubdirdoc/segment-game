/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;





public class VideoBehaviour : MonoBehaviour {

	public string m_ressourcesDirectory = "Ressources";
	public string m_urlPath = "intro.mp4";
	string m_Path;

	void Start () {


		m_Path = Path.GetFullPath (".");

		VideoPlayer currentVideoPlayer = gameObject.GetComponent <VideoPlayer> ();


		currentVideoPlayer.url = m_Path + "/" + m_ressourcesDirectory + "/" + m_urlPath;
		currentVideoPlayer.Play ();
					
	
	}




	void Update () {
		
	}
}
