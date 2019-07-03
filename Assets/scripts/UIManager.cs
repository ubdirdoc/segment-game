/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.Events;


public class UIManager : MonoBehaviour
{
	const string ANIMATOR_DISPLAY_VARIABLE_NAME = "MustBeOnScreen";

	public GameObject m_playerInputBoxOffset = null;

	public Animator m_playerInputFieldAnimator = null;
	public Animator m_roomDialogAnimator = null;

	public DialogManager m_dialogManager = null;

	public CanvasGroup m_roomDialogCanvasGroup = null;

	public Text m_roomDialogText = null;

	private bool m_isRoomDialogOnScreen = false;

	public InputField m_playerInputField = null;
	public Text m_playerQuestionField = null;

	public GameObject m_preventInputByKeyboard = null; 

	public Image m_fadeInOutImage = null;
	public Image m_loadingAdvicePanelBg = null;
	public Image m_loadingAdviceImage = null;
	private float m_fadeInOutTime = 1.0f;

	public UnityEvent m_playerInputEvent = null;

	public float m_inputFieldErrorBlinkingTime = 0.2f;

	public RestartPanelScript m_restartPanel = null;

	public GameObject m_exitPanel = null;

	public DiaryManagement m_diaryManagement = null;


	private bool m_androidKeyboardHasBeenVisible = false;
	private bool m_androidKeyboardSwitchFromVisibleToInvisible = false;

	private bool m_mustHighlightNewDiaryEntry = false;

	private bool m_advicePanelHasBeenDisplayed = false;



	// Use this for initialization
	void Start ()
	{
		
		StartCoroutine(SetAdvicePanelImage());
	}

	IEnumerator SetAdvicePanelImage() {
		while (!LoadingAdviceLoader.instance.IsSpriteReady() && !LoadingAdviceLoader.instance.HasSpriteFailedToLoad()) {
			yield return null;
		}

		if (!LoadingAdviceLoader.instance.HasSpriteFailedToLoad()) {
			m_loadingAdviceImage.sprite = LoadingAdviceLoader.instance.GetSprite();
		}
	}

	public bool IsRoomDialogOnScreen() {
		return m_isRoomDialogOnScreen;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_mustHighlightNewDiaryEntry) {
			if (!m_isRoomDialogOnScreen) {
				m_diaryManagement.HighlightNewDiaryEntry();
				m_mustHighlightNewDiaryEntry = false;
			}
		}
		
#if UNITY_ANDROID
		if (TouchScreenKeyboard.visible) {
#else
		if (false) {
#endif
		

			//GenericLog.Log("TOUCH SIZE " + GetTouchscreenKeyboardHeight());
			RectTransform offsetRectTransform = m_playerInputBoxOffset.GetComponent<RectTransform> ();

			if (offsetRectTransform != null) {
				offsetRectTransform.anchoredPosition = new Vector2 (offsetRectTransform.anchoredPosition.x,  
				                                           		   GetRemainingHeightWhenTouchKeyboardVisible () / 2.0f);
			}

			if (!m_androidKeyboardSwitchFromVisibleToInvisible) {
				//GenericLog.Log("KEYBOARD VISIBLE");
				m_androidKeyboardHasBeenVisible = true;
			}
		}

#if UNITY_ANDROID
		if (!TouchScreenKeyboard.visible) {
#else
		if (true) {
#endif
			RectTransform offsetRectTransform = null;
			if (m_playerInputBoxOffset != null) {
				offsetRectTransform = m_playerInputBoxOffset.GetComponent<RectTransform> ();
			}

			if (offsetRectTransform != null) {
				offsetRectTransform.anchoredPosition = new Vector2 (offsetRectTransform.anchoredPosition.x,  
				                                                   0);
			}

			if (m_androidKeyboardHasBeenVisible) {
				//GenericLog.Log("KEYBOARD INVISIBLE AND WILL TRIGGER");
				m_androidKeyboardSwitchFromVisibleToInvisible = true;
				m_androidKeyboardHasBeenVisible = false;
			}

		}



		if (m_playerInputEvent != null) {
			if (m_playerInputField != null && m_playerInputField.text != "" && (Input.GetKey (KeyCode.Return) || Input.GetKey (KeyCode.KeypadEnter)
                                                                          
			                                                                          || m_androidKeyboardSwitchFromVisibleToInvisible
		                                                                          
			                                                                          )) {
				m_androidKeyboardSwitchFromVisibleToInvisible = false;
						
						
						
						
						
#if UNITY_STANDALONE_OSX
#if !UNITY_EDITOR
				if (Input.GetKey(KeyCode.KeypadEnter)) {
					//m_playerInputField.text = m_playerInputField.text.Substring(0, m_playerInputField.text.Length - 1);
					//m_roomDescriptionText.text += " <" + m_playerInputField.text + "> ";
				}
#endif
#endif
				ValidateInput ();
				
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (m_exitPanel.activeSelf) {
				m_exitPanel.SetActive(false);
			} else {
				m_exitPanel.SetActive(true);
			}
		}
		
	//	SelectAnswerField();
	}

	void ValidateInput ()
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		//SelectAnswerField();
		//m_playerInputField.ActivateInputField();
#endif
	
		//print (m_playerInputField.text.Length);
		
		m_playerInputEvent.Invoke ();
		
		EmptyAnswerField ();
	}

	public void DisplayPlayerInputField (bool isPassword)
	{
		m_playerInputFieldAnimator.SetBool (ANIMATOR_DISPLAY_VARIABLE_NAME, true);

		if (isPassword) {
			m_playerInputField.contentType = InputField.ContentType.Password;
		} else {
			m_playerInputField.contentType = InputField.ContentType.Standard;
		}
	}

	public void HidePlayerInputField ()
	{
		m_playerInputFieldAnimator.SetBool (ANIMATOR_DISPLAY_VARIABLE_NAME, false);
	}
		
	public void DisplayRoomDialog (string description)
	{
		m_isRoomDialogOnScreen = true;

		m_dialogManager.SetDialog	 (description);
		/*m_roomDescriptionText.text = description;*/
		m_roomDialogAnimator.SetBool (ANIMATOR_DISPLAY_VARIABLE_NAME, true);

		m_roomDialogCanvasGroup.blocksRaycasts = true;
	}

	public void ClearRoomDialog()
	{
		m_roomDialogText.text = "";
	}

	public void HideRoomDialog ()
	{
		m_roomDialogAnimator.SetBool (ANIMATOR_DISPLAY_VARIABLE_NAME, false);
		m_roomDialogCanvasGroup.blocksRaycasts = false;

		m_isRoomDialogOnScreen = false;
	}

	
	public string GetInputFieldText ()
	{
		if (m_playerInputField == null) {
			return "";
		} else {
			return m_playerInputField.text;
		}
	}

	public void WrongAnswerBlink ()
	{
		StartCoroutine ("WrongAnswerBlinkCoroutine");
	}

	IEnumerator WrongAnswerBlinkCoroutine ()
	{
		
		m_playerInputField.interactable = false;
		yield return new WaitForSeconds (m_inputFieldErrorBlinkingTime);
		m_playerInputField.interactable = true;

		// Solution to automatically focus on the input field again after the player presses "ENTER"
		if (m_preventInputByKeyboard == null) {
			m_playerInputField.Select();
		} else if (!m_preventInputByKeyboard.activeSelf) {
			m_playerInputField.Select();
		}

	}

	public void SetFadeInOutTime (float newTime)
	{
		m_fadeInOutTime = newTime;
	}

	public void SceneFadeIn ()
	{
		if (m_advicePanelHasBeenDisplayed) {
			m_fadeInOutImage.CrossFadeAlpha (0.0f, m_fadeInOutTime, false);
		} else {
			m_fadeInOutImage.CrossFadeAlpha (0.0f, 0, false);
			m_loadingAdviceImage.CrossFadeAlpha(0.0f, m_fadeInOutTime, false);
			m_loadingAdvicePanelBg.CrossFadeAlpha(0.0f, m_fadeInOutTime, false);

			m_advicePanelHasBeenDisplayed = true;
		}





	
	}

	public void SceneFadeOut ()
	{
		if ((m_fadeInOutTime == 0.0f)) {
			return;
		}

		m_fadeInOutImage.CrossFadeAlpha (1.0f, m_fadeInOutTime, false);
	}

	public bool IsSceneFadedOut ()
	{
		return ((m_fadeInOutImage.canvasRenderer.GetAlpha () == 1.0f) || (m_fadeInOutTime == 0.0f));
	}

	public void ChangeSceneFadeInOutImageColor(Color newColor) {
		m_fadeInOutImage.color = newColor;
	}

	public void ChangeQuestion (string question)
	{
		m_playerQuestionField.text = question;
	}

	public void AddTextToAnswer (string text)
	{
		m_playerInputField.text += text;
	}

	public void ChangeTextToAnswer (string text)
	{
		m_playerInputField.text = text;
	}

	public void ValidateAnswer ()
	{
		ValidateInput ();
	}

	public void EmptyAnswerField ()
	{
		m_playerInputField.text = "";
	}

	public void SelectAnswerField ()
	{
		m_playerInputField.Select ();
	}

	public void DiagramFileHasBeenModified() {
		m_restartPanel.ShowsForDiagramModification();
	}


	public float GetRemainingHeightWhenTouchKeyboardVisible ()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
			
			using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
			{
				View.Call("getWindowVisibleDisplayFrame", Rct);

				//GenericLog.Log(Screen.height + " +++++++++ " + Rct.Call<int>("height"));
				
				return Screen.height - Rct.Call<int>("height");
			}
		}
#else
		return Screen.height;
#endif


	}

	public void AddDiaryImage(string filename) {
		m_diaryManagement.AddDiaryImage(filename);
	}

	public bool IsDiaryImageStillLoading() {
		return m_diaryManagement.IsCurrentlyLoadingImage();
	}

	public void HighlightDiaryEntry() {
		m_mustHighlightNewDiaryEntry = true;
	}

	

}
