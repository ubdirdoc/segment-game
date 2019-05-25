/* Authors : Raphaël Marczak, Arthur Desmesure - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Globalization;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {

	public bool m_mustDisplayWordPerWord = false;


	public Text m_roomDescriptionText = null;
	public UIManager m_UIManager = null;
 
	public string[] m_splitDialog;
	public string m_splitDialogInd;
	public float m_initialDelay = 1f;
	public float m_wordAnimationTime = 0.1f;
	public float m_characterAnimationTime = 0.1f;
	public float m_spaceWaitOffset = 0.04f;
	public float m_vowelPitch = 0.99f;
	public float m_characterSoundVolume = 1.0f;
	public AudioSource m_TypeWriter;

	//int currentlyDisplayingText = 0;
	int m_index  = -1;
	string goatTalkingText;

	bool m_AnimationCoroutine = false;

	/*private CanvasGroup m_canvasGroup = null;*/


/// ..............................................................................................................................................................................................................
	public void SetDialog(string dialog) {

		m_roomDescriptionText.text = dialog;
		m_splitDialog = dialog.Split ('\n');

		m_index = 0;
		StartCoroutine (AnimateText ());
	
	}

	bool IsAVowel(char letter) {
		return (string.Compare(letter.ToString(), "a", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0 
			|| string.Compare(letter.ToString(), "e", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
			|| string.Compare(letter.ToString(), "i", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
			|| string.Compare(letter.ToString(), "o", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
			|| string.Compare(letter.ToString(), "u", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
			|| string.Compare(letter.ToString(), "y", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0);
	}

	bool IsASpace(char letter) {
		return (letter == ' ');
	}


	IEnumerator AnimateText(){
		m_AnimationCoroutine = true;

		m_roomDescriptionText.text = "";

		if (m_index == 0) {
			yield return new WaitForSeconds(m_initialDelay);
		}


		int i = 0;


		while (i < (m_splitDialog[m_index].Length))
		{
			float waitTime = 0.0f;


			if (m_mustDisplayWordPerWord) {
				waitTime = m_wordAnimationTime;

				bool mustContinue = true;

				while (mustContinue) {
					if (i >=  (m_splitDialog[m_index].Length)) {
						mustContinue = false;
					} else if ((m_splitDialog[m_index][i] == ' ') || (m_splitDialog[m_index][i] == '-')) {
						mustContinue = false;
					} 
					++i;
				}

				AudioManager.instance.PlaySoundAsSecondaryStream(m_TypeWriter.clip, m_characterSoundVolume);

			} else {
				
				waitTime = m_characterAnimationTime;

				if (IsASpace(m_splitDialog[m_index][i])) {
					waitTime += m_spaceWaitOffset;
				} if (IsAVowel(m_splitDialog[m_index][i])) {
					AudioManager.instance.PlaySoundAsSecondaryStream(m_TypeWriter.clip, m_characterSoundVolume, m_vowelPitch);
				}

				else {
					AudioManager.instance.PlaySoundAsSecondaryStream(m_TypeWriter.clip, m_characterSoundVolume);
				}
				i++;
			}
	
			if (i >= m_splitDialog[m_index].Length) {
				i = m_splitDialog[m_index].Length;
			}

			m_roomDescriptionText.text = m_splitDialog[m_index].Substring(0,i);
			yield return new WaitForSeconds(waitTime);
		}

		m_roomDescriptionText.text = m_splitDialog[m_index];
		m_AnimationCoroutine = false;
	}



	void Start () {
	}
	

	void Update ()
	{
		
		if (m_index == -1) {
			return;
		}

		if (m_index >= m_splitDialog.Length) {
			//StopAllCoroutines();
			m_UIManager.HideRoomDialog ();
			m_index = -1;
			return;
		}


		/*m_splitDialogInd = m_splitDialog [m_index];
		m_roomDescriptionText.text = m_splitDialogInd ;
*/

			
	//	Debug.Log (m_index);

			
		if (Input.GetMouseButtonDown (0)) {
			if (m_AnimationCoroutine == true) {
				StopAllCoroutines ();
				m_roomDescriptionText.text = m_splitDialog [m_index];
				m_AnimationCoroutine = false;
			} else {
				m_roomDescriptionText.text = m_splitDialog [m_index];
				m_index++;

				if (m_index < m_splitDialog.Length) {
					
					StartCoroutine (AnimateText ());
				}
			}


			
			
			

		}

	}

	/*public bool isDisplayingDialog() {
		
	}*/

}

