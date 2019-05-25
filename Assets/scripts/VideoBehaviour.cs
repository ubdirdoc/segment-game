/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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
