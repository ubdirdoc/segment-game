/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SEGMent;

public class PuzzleSavePos {
	public Vector3 puzzlePos = Vector3.zero;
	public Vector3 puzzleScale = Vector3.zero; 
}

public class PuzzleSolutionCheck {
	private float autorizedDifference = 0.02f;

	public PuzzleBehavior behavior;
	public Vector2 initialPos;
	public Vector2 solutionPos;
	public Vector2 currentPos;

	public PuzzleSolutionCheck() {
		behavior = null;
		solutionPos = Vector2.zero;
		currentPos = Vector2.zero;
	}

	public bool IsWellPositionned() {
		//Debug.Log(Vector2.Distance(solutionPos, currentPos));
		return (Vector2.Distance(solutionPos, currentPos) <= autorizedDifference);
	}

	public void Display() {
		Debug.Log("init " + initialPos + " ; solution " + solutionPos + " ; current " + currentPos);
	}


}


public class Main : MonoBehaviour {
	public bool m_disableRadar = false;
	Engine m_SEGMentEngine;
	public Text m_androidErrorText = null;

	public bool m_initIsDone = false;

	public string m_winString = "";

	public GameObject m_itemObject;

	public Dictionary<int, GameObject> m_sceneItemsBySEGMentIndex;
	//List<GameObject> m_sceneItems;
	Dictionary<SpriteSetter, List<int>> m_objectStateToSolutionCheck; 

	int m_roomID = -1;
	Dictionary<int, PuzzleSavePos> m_puzzleSavPositions;

	Dictionary<int, List<PuzzleSolutionCheck> > m_puzzleChecker;


	public GameObject m_clickTextObject;
	public GameObject m_clickTextRadarIcon;

	List<GameObject> m_sceneClickTexts;
	List<GameObject> m_sceneClickTextRadarIcons;

	public GameObject m_gotoObject;
	public GameObject m_gotoRadarIcon;

	//public DiaryManagement m_diaryManagement;
	
	List<GameObject> m_sceneGoTos;
	List<GameObject> m_sceneGoToRadarIcons;



	public MultiresManager m_backgroundDisplay = null;

	public UIManager m_UIManager = null;

	public string m_currentlyPlayedMusic = "";

	const string RETURN_STRING  = "!!!";
	const string ADD_STRING  = "+";

	public GameObject m_loadingIcon = null;
	public float m_minimumWaitTimeForFirstLoad = 15.0f; 

	private float m_launchDate = 0f;

	private bool m_isSceneLoaded = false;
	//private float m_initialOrthoSize = 500.0f;

	private bool m_isClicEnabled = true;

	public float m_clicDisableTimeAfterZoom = 0.1f;
	private float m_lastZoomTime = 0.0f;

	public GameObject m_radarObject = null;

	GameObject m_lastCreatedRadar = null;
	float m_lastRadarCreationTime = 0.0f;
	float m_lastInteractionClicTime = 0.0f;

	public GameObject m_preventInputByKeyboard = null;

	public float m_sceneFadeInOutTime = 1.0f;



	public string m_endingSequenceSceneName = "videoOutro";
	private bool m_isGoingToEndingSequence = false;

	private bool m_mustHighlightDiary = false;

	//private float m_itemZ = 100f;

	/*int GetHashForPuzzleSav(int roomID, int itemID) {
		int HASH_ROOM_MULTIPLIER = 10000;

		int key = roomID * HASH_ROOM_MULTIPLIER + itemID;

		return key;
	}*/

	bool IsPuzzleSavExist(int itemID) {
		//int key = GetHashForPuzzleSav(roomID, itemID);

		return m_puzzleSavPositions.ContainsKey(itemID);
	}

	void AddPuzzleSavPos(int itemID, Vector3 pos, Vector3 scale) {
		//int key = GetHashForPuzzleSav(roomID, itemID);

		if (!m_puzzleSavPositions.ContainsKey(itemID)) {
			m_puzzleSavPositions[itemID] = new PuzzleSavePos();
		}
			
		m_puzzleSavPositions[itemID].puzzlePos = pos;
		m_puzzleSavPositions[itemID].puzzleScale = scale;

	}

	void AddPuzzlePieceToChecker(int roomID, PuzzleBehavior puzzlePiece) {
		if (roomID == -1) {
			return;
		}

		if (puzzlePiece == null) {
			return;
		}

		Vector3 currentPos = Vector3.zero; 
		currentPos.x = puzzlePiece.transform.localPosition.x;
		currentPos.y = puzzlePiece.transform.localPosition.y;

	//	Debug.Log("ADD " + currentPos);

		if (!m_puzzleChecker.ContainsKey(roomID)) {
			m_puzzleChecker[roomID] = new List<PuzzleSolutionCheck>();
		}

		PuzzleSolutionCheck currentSolutionCheck = new PuzzleSolutionCheck();

		currentSolutionCheck.initialPos.x = currentPos.x / puzzlePiece.GetForegroundWidth();
		currentSolutionCheck.initialPos.y = currentPos.y / puzzlePiece.GetForegroundHeight();

		currentSolutionCheck.behavior = puzzlePiece;

		m_puzzleChecker[roomID].Add(currentSolutionCheck);

		m_puzzleChecker[roomID][m_puzzleChecker[roomID].Count-1].solutionPos.x = currentSolutionCheck.initialPos.x - m_puzzleChecker[roomID][0].initialPos.x;
		m_puzzleChecker[roomID][m_puzzleChecker[roomID].Count-1].solutionPos.y = currentSolutionCheck.initialPos.y - m_puzzleChecker[roomID][0].initialPos.y;

		//Debug.Log(m_puzzleChecker[roomID].Count-1);
		//m_puzzleChecker[roomID][m_puzzleChecker[roomID].Count-1].Display();

	}

	void UpdatePuzzleChecker(int roomID) {
		if (roomID == -1) {
			return;
		}

		if (!m_puzzleChecker.ContainsKey(roomID)) {
			return;
		}

		float refX = 0f;
		float refY = 0f;
	
		for (int i = 0; i < m_puzzleChecker[roomID].Count; ++i) {
			Vector3 currentPos = Vector3.zero; 

			PuzzleBehavior currentPiece = m_puzzleChecker[roomID][i].behavior;

			currentPos.x = currentPiece.transform.localPosition.x;
			currentPos.y = currentPiece.transform.localPosition.y;

			//Debug.Log("UPDATE " + currentPos);

			m_puzzleChecker[roomID][i].currentPos.x = currentPos.x / currentPiece.GetForegroundWidth();
			m_puzzleChecker[roomID][i].currentPos.y = currentPos.y / currentPiece.GetForegroundHeight();

			if (i == 0) {
				refX = currentPos.x / currentPiece.GetForegroundWidth();
				refY = currentPos.y / currentPiece.GetForegroundHeight();
			}

			m_puzzleChecker[roomID][i].currentPos.x -= refX;
			m_puzzleChecker[roomID][i].currentPos.y -= refY;

			//Debug.Log(i);
			//m_puzzleChecker[roomID][i].Display();
		}


	}

	bool ComputePuzzleCheck(int roomID) {
		if (roomID == -1) {
			return false;
		}

		if (!m_puzzleChecker.ContainsKey(roomID)) {
			return false;
		}

		for (int i = 0; i < m_puzzleChecker[roomID].Count; ++i) {
			if (!m_puzzleChecker[roomID][i].IsWellPositionned()) {
				return false;
			}
		}

		return true;
	}

	void ChangePuzzleBehaviorForPuzzleCheck(int roomID, int index, PuzzleBehavior behavior) {
		if (roomID == -1) {
			return;
		}

		if (!m_puzzleChecker.ContainsKey(roomID)) {
			return;
		}

		if (index >= m_puzzleChecker[roomID].Count) {
			return;
		}

		m_puzzleChecker[roomID][index].behavior = behavior;
	}

	void Start () {
		m_launchDate = Time.fixedTime;

		m_initIsDone = false;

		gameObject.AddComponent<MonoBehaviourForCoroutineSingleton>();

		#if UNITY_ANDROID || UNITY_WEBGL
		GenericLog.SetAndroidDebugText(m_androidErrorText);
		#endif

		#if UNITY_WEBGL
		m_musicExtension = ".mp3";
		#endif

//		m_sceneItems = new List<GameObject>();
		m_sceneItemsBySEGMentIndex = new Dictionary<int, GameObject>();
		m_objectStateToSolutionCheck = new Dictionary<SpriteSetter, List<int>>();

		m_sceneClickTexts = new List<GameObject>();
		m_sceneClickTextRadarIcons = new List<GameObject>(); 

		m_sceneGoTos = new List<GameObject>();
		m_sceneGoToRadarIcons = new List<GameObject>();

		m_puzzleSavPositions = new Dictionary<int, PuzzleSavePos>();
		m_puzzleChecker = new Dictionary<int, List<PuzzleSolutionCheck>>();

		Application.runInBackground = true;

		m_SEGMentEngine = new SEGMent.Engine();
		m_SEGMentEngine.SetPlayerName("Player");

		MetricLogger.instance.SetPlayerID(m_SEGMentEngine.GetPlayerName());




		StartCoroutine("LoadGameStructure");

	}

	IEnumerator LoadGameStructure() {
		while (!SEGMentPath.instance.ArePathGenerated()) {
			yield return null;
		}

		m_SEGMentEngine.LoadGameStructure(SEGMentPath.instance.GetSEGMentDiagramPath());

		while (!m_SEGMentEngine.IsStructureLoaded()) {
			yield return null;
		}

		//m_initialOrthoSize = Camera.main.orthographicSize;

		m_isClicEnabled = true;

		//GenericLog.Log("BEFORE FIRST METRIC");
		MetricLogger.instance.Log("SEGMENT_RUNNING", true);
		//GenericLog.Log("AFTER FIRST METRIC");

		if (GifPreLoader.instance.MustPreload ()) {
			List<Item> allGameItems = m_SEGMentEngine.GetAllGameItems ();

			foreach (Item currentItem in allGameItems) {
				string itemImageURL = currentItem.GetImageURL ();

				if (itemImageURL != "") {
					GameObject currentItemGameObject = Instantiate (m_itemObject, Vector3.zero, Quaternion.identity) as GameObject;

					SpriteSetter itemSpriteSetter = currentItemGameObject.GetComponent<SpriteSetter> ();

					if (itemSpriteSetter.IsAGifFile (itemImageURL)) {
						
					
						BoundingBox relativeItemPosBB = currentItem.GetRelativePosition ();

						if (itemSpriteSetter != null) {
							itemSpriteSetter.SetUIManager (m_UIManager);
							StartCoroutine (itemSpriteSetter.SetSpriteByURLRelativelyToAnotherOneCoroutine (Path.Combine (SEGMentPath.instance.GetSEGMentGameDataPath (), itemImageURL),
								null,
								relativeItemPosBB.x1, 
								relativeItemPosBB.y1,
								relativeItemPosBB.x2,
								relativeItemPosBB.y2,
								0));

							while (!itemSpriteSetter.IsSpriteLoaded ()) {
								yield return null;
							}

							itemSpriteSetter.ReleaseSprite ();

						}

					}
					Destroy (currentItemGameObject);
				}
			}
		}
		m_initIsDone = true;
	}
	
	void Update () {
		if (!m_initIsDone) {
			return;	
		}

		m_SEGMentEngine.Update(Time.deltaTime); 


		/*if (Input.GetKeyDown(KeyCode.R)) {
			foreach (KeyValuePair<int, GameObject> currentDictionnaryItem in m_sceneItemsBySEGMentIndex) {
				PuzzleBehavior currentBehavior = currentDictionnaryItem.Value.GetComponent<PuzzleBehavior>();

				if (currentBehavior != null) {
					if (currentBehavior.IsDragDropEnabled()) {
						currentBehavior.RandomizePos();
					}
				}
			}

		}*/

		if (Input.GetKeyDown(KeyCode.Space)) {
			LogPlayerCurrentInformation();

		} /*else if (Input.GetKeyDown(KeyCode.Escape)) {
			MetricLogger.instance.Log("SEGMENT_RUNNING", false);
			Application.Quit();

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif*/
			// m_SEGMentEngine.UserAnswer("Solution");
		//}

		List<PLAYER_MODIF_INFO> playerChanges = m_SEGMentEngine.PopPlayerModifInfos();
		foreach(PLAYER_MODIF_INFO currentInfo in playerChanges) {
			if (currentInfo == PLAYER_MODIF_INFO.PLAYER_MOVED) {
				StartCoroutine("UpdateNewRoomCoroutine");

			}
		}

		if (m_SEGMentEngine.IsDiagramFileModifiedSinceLoad()) {
			m_UIManager.DiagramFileHasBeenModified();
		}

		ZoomOnClic zoomManager = GetComponent<ZoomOnClic>();

		if (zoomManager != null) {
			if (zoomManager.IsZooming()) {
				m_lastZoomTime = Time.fixedTime;
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			UpdatePuzzleChecker(m_roomID);
			if (ComputePuzzleCheck(m_roomID)) {
				m_SEGMentEngine.PuzzleSolutionValidated();
				//Debug.Log("YEAHHHHHHH !");
			}
		}


		if (m_isSceneLoaded) {
			if (CheckStateObjectSolutions()) {
				m_SEGMentEngine.ObjectStateSolutionValidated();
				PreventClickOnStateObject();
				
				
				m_objectStateToSolutionCheck.Clear();
			}
		}



	}

	IEnumerator UpdateNewRoomCoroutine() {
		Resources.UnloadUnusedAssets();
		Cursor.visible = false;

		//GenericLog.Log("UPDATE NEW ROOM COROUTINE");
		m_isSceneLoaded = false;
		m_isClicEnabled = false;

		//m_loadingIcon.SetActive(true);

		if (m_SEGMentEngine.isRoomChangeImmediate()) {
			m_UIManager.SetFadeInOutTime(0.0f);
		} else {
			m_UIManager.SetFadeInOutTime(m_sceneFadeInOutTime);
		}

		PreventClickOnStateObject();
		m_UIManager.SceneFadeOut();

		List<string> transitionSoundsToPlay = m_SEGMentEngine.PopTransitionSounds();
		foreach (string soundToPlay in transitionSoundsToPlay) {
			AudioManager.instance.PlaySoundByURL(SEGMentPath.instance.GetSoundPath(soundToPlay));
		}

		while (!m_UIManager.IsSceneFadedOut()) {
			yield return null;
		}

		ZoomOnClic zoomManager = GetComponent<ZoomOnClic>();

		if (zoomManager != null) {
			zoomManager.reinit();
		}

		//Camera.main.orthographicSize = m_initialOrthoSize;



		StartCoroutine("UpdateSceneCoroutine");

		while (!m_isSceneLoaded) {
			yield return null;
		}

		UpdateUI();

		//m_loadingIcon.SetActive(false);

		m_UIManager.SceneFadeIn();
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		Cursor.visible = true;
		m_isClicEnabled = true;
	}

	IEnumerator UpdateSceneCoroutine() {
		//GenericLog.Log("UPDATE SCENE");
		while (!SEGMentPath.instance.ArePathGenerated()) {
			yield return null;
		}

		//Author - Vincent Casamayou - June 2019
 		//Check and Update the Radar
		m_disableRadar = true;
		Debug.Log("The Radar is " + m_SEGMentEngine.GetCurrentRadar());
		if (m_SEGMentEngine.GetCurrentRadar() == true){
			m_disableRadar = false;
		}
	

		if (m_backgroundDisplay != null) {
			GenericLog.Log("APPLI PATH : " + SEGMentPath.instance.GetSEGMentDiagramPath());
			//GenericLog.Log("GDATADIRECTORY PATH : " + m_gameDataDirectoryName);
			GenericLog.Log("GETCURRENTROOMBACKGROUND PATH : " + m_SEGMentEngine.GetCurrentRoomBackgroundImageURL());

			m_backgroundDisplay.ChangeImageFromURL(Path.Combine(SEGMentPath.instance.GetSEGMentGameDataPath(), m_SEGMentEngine.GetCurrentRoomBackgroundImageURL()));

			while (m_backgroundDisplay.IsLoadingNewImage()) {
				yield return null;
			}


			MetricLogger.instance.Log ("CHANGE_ROOM", m_SEGMentEngine.GetCurrentRoomBackgroundImageURL());

			yield return null;
		}

		foreach(KeyValuePair<int, GameObject> currentItemKeyValue in m_sceneItemsBySEGMentIndex) {
		//for (int i = 0; i < m_sceneItems.Count ; ++i) {
		//foreach (GameObject item in m_sceneItems) {
			//GameObject currentItem = m_sceneItems[i];
			GameObject currentItem = currentItemKeyValue.Value;
			SpriteSetter currentSpriteSetter = currentItem.GetComponent<SpriteSetter> ();
			PuzzleBehavior currentPuzzleBehavior = currentItem.GetComponent<PuzzleBehavior>();

			if (currentSpriteSetter != null) {
				m_SEGMentEngine.SetItemStartFrame(currentItemKeyValue.Key, currentSpriteSetter.GetCurrentSpriteFrameNum());
				currentSpriteSetter.ReleaseSprite();
			}

			if (currentPuzzleBehavior != null) {
				if (currentPuzzleBehavior.IsDragDropEnabled()) {
					//AddPuzzleSavPos(m_roomID, i, currentItem.transform.localPosition, currentItem.transform.localScale);
					AddPuzzleSavPos(currentItemKeyValue.Key, currentItem.transform.localPosition, currentItem.transform.localScale);
				}
				//currentPuzzleBehavior.
			}

			Destroy(currentItem);
		}

		//m_sceneItems.Clear();
		m_sceneItemsBySEGMentIndex.Clear();

		List<int> itemIndexes = m_SEGMentEngine.GetItemIndexes();
		int orderInLayer = 0;

		m_roomID = m_SEGMentEngine.GetCurrentRoomID();

		int currentPuzzleIndex = 0;

		m_objectStateToSolutionCheck.Clear();

		foreach (int index in itemIndexes) {
			string itemImageURL = m_SEGMentEngine.GetItemImageURL (index);

			if (itemImageURL != "") {
				BoundingBox relativeItemPosBB = m_SEGMentEngine.GetItemBoundingBox (index);
				GameObject currentItem = Instantiate (m_itemObject, Vector3.zero, Quaternion.identity) as GameObject;



				SpriteSetter itemSpriteSetter = currentItem.GetComponent<SpriteSetter> ();
				
				if (itemSpriteSetter != null) {
					itemSpriteSetter.SetUIManager(m_UIManager);
					StartCoroutine(itemSpriteSetter.SetSpriteByURLRelativelyToAnotherOneCoroutine(Path.Combine(SEGMentPath.instance.GetSEGMentGameDataPath(), itemImageURL),
					                                                      m_backgroundDisplay.GetMainSpriteRenderer (),
					                                                      relativeItemPosBB.x1, 
					                                                      relativeItemPosBB.y1,
					                                                      relativeItemPosBB.x2,
																		  relativeItemPosBB.y2,
																		  orderInLayer));

					while (!itemSpriteSetter.IsSpriteLoaded()) {
						yield return null;
					}
				}

				string itemSoundName = m_SEGMentEngine.GetItemSoundName(index);
				string itemDescriptionName = m_SEGMentEngine.GetItemDescription(index);

				bool isPuzzlePiece = m_SEGMentEngine.IsItemPuzllePiece(index);

				if (isPuzzlePiece) {
					PuzzleBehavior currentPuzzleBehavior = currentItem.GetComponent<PuzzleBehavior>();

					if (currentPuzzleBehavior) {
						currentPuzzleBehavior.SetUIManager(m_UIManager);
						currentPuzzleBehavior.EnableDragDrop(true);

						//if (IsPuzzleSavExist(m_roomID, m_sceneItems.Count)) {
							//int key = GetHashForPuzzleSav(m_roomID, m_sceneItems.Count);

						if (IsPuzzleSavExist(index)) {
							//int key = GetHashForPuzzleSav(m_roomID, index);
							currentItem.transform.localPosition = m_puzzleSavPositions[index].puzzlePos;
							currentItem.transform.localScale = m_puzzleSavPositions[index].puzzleScale;

							ChangePuzzleBehaviorForPuzzleCheck(m_roomID, currentPuzzleIndex, currentPuzzleBehavior);
							currentPuzzleIndex++;

						} else {
							AddPuzzlePieceToChecker(m_roomID, currentPuzzleBehavior);
							currentPuzzleBehavior.RandomizePos();
						}
					}
				}



				if (itemDescriptionName != "") {
					Text itemText = currentItem.GetComponent<Text>();

					if (itemText != null) {
						itemText.text = itemDescriptionName;
					}
				}



				SoundLauncher itemSoundLauncher = currentItem.GetComponent<SoundLauncher>();

				if (itemSoundName != "") {
					if (itemSoundLauncher != null) {
						itemSoundLauncher.LoadSoundByURL(SEGMentPath.instance.GetSoundPath(itemSoundName), true);
					}

					itemSoundLauncher.PlayLoadedSoundWhenReady();
				}

				List<int> stopFrames = m_SEGMentEngine.GetItemStopFrames(index);
				List<int> solutionFrames = m_SEGMentEngine.GetItemSolutionFrames(index);

				AudioSource itemAudioSource = currentItem.GetComponent<AudioSource>();

				if (itemAudioSource != null) {
					if (stopFrames.Count > 0) {
						itemAudioSource.volume = 0f;
					}
				}

				foreach (int stopFrame in stopFrames) {
					itemSpriteSetter.SetStopFrame(stopFrame);
				}

				if (solutionFrames.Count > 0) {
					m_objectStateToSolutionCheck[itemSpriteSetter] = solutionFrames;
				}

				itemSpriteSetter.setFrameIndex(m_SEGMentEngine.GetItemStartFrame(index));

				itemSpriteSetter.StartGifLoop();

				//m_sceneItems.Add(currentItem);

				m_sceneItemsBySEGMentIndex[index] = currentItem;

				orderInLayer++;
				yield return null;
			}

		}

		List<int> clickTextIndexes = m_SEGMentEngine.GetClickTextIndexes();

		foreach (GameObject clickText in m_sceneClickTexts) {
			Destroy(clickText);
		}

		foreach (GameObject radarIcon in m_sceneClickTextRadarIcons) {
			Destroy(radarIcon);
		}

		m_sceneClickTexts.Clear();
		m_sceneClickTextRadarIcons.Clear();

		foreach (int index in clickTextIndexes) {
			SpriteRenderer clickTextSpriteRenderer = m_clickTextObject.GetComponent<SpriteRenderer>();

			if (clickTextSpriteRenderer != null) {
				GameObject currentClickText = Instantiate (m_clickTextObject, Vector3.zero, Quaternion.identity) as GameObject;
				GameObject currentClickTextRadarIcon = Instantiate (m_clickTextRadarIcon, Vector3.zero, Quaternion.identity) as GameObject;

				BoundingBox relativeClickTextPosBB = m_SEGMentEngine.GetClickTextBoundingBox (index);

				SpriteSetter clickTextSpriteSetter = currentClickText.GetComponent<SpriteSetter> ();
				
				if (clickTextSpriteSetter != null) {
					clickTextSpriteSetter.SetSpriteRelativelyToAnotherOne(clickTextSpriteRenderer.sprite,
					                                                      m_backgroundDisplay.GetMainSpriteRenderer (),
					                                                      relativeClickTextPosBB.x1, 
					                                                      relativeClickTextPosBB.y1,
					                                                      relativeClickTextPosBB.x2,
					                                                      relativeClickTextPosBB.y2);
				}

				string clickTextSoundName = m_SEGMentEngine.GetClickTextSoundName(index);
				string clickTextText = m_SEGMentEngine.GetClickTextText(index);
				bool isClickTextAnURL = m_SEGMentEngine.IsClickTextAnURL(index);

				if (clickTextText != "") {
					Text actualText = currentClickText.GetComponent<Text>();
					
					if (actualText != null) {
						actualText.text = clickTextText;
					}
				}

				if (clickTextSoundName != "") {
					SoundLauncher clickTextSoundLauncher = currentClickText.GetComponent<SoundLauncher>();
					
					if (clickTextSoundLauncher != null) {
						clickTextSoundLauncher.LoadSoundByURL(SEGMentPath.instance.GetSoundPath(clickTextSoundName), false);
					}
				}

				MustTextBeConsideredAsURL mustClicTextBeSeenAsAnURL = currentClickText.GetComponent<MustTextBeConsideredAsURL>();

				if (mustClicTextBeSeenAsAnURL != null) {
					mustClicTextBeSeenAsAnURL.SetTextAsBeingAnURL(isClickTextAnURL);
				}

				m_sceneClickTexts.Add(currentClickText);

				// Create a click text radar icon with a respected ratio
				if (currentClickText.transform.localScale.x < currentClickText.transform.localScale.y) {
					currentClickTextRadarIcon.transform.localScale = new Vector3(currentClickText.transform.localScale.x, currentClickText.transform.localScale.x, 1.0f);
				} else {
					currentClickTextRadarIcon.transform.localScale = new Vector3(currentClickText.transform.localScale.y, currentClickText.transform.localScale.y, 1.0f);
				}

				currentClickTextRadarIcon.transform.localPosition = currentClickText.transform.localPosition;

				m_sceneClickTextRadarIcons.Add (currentClickTextRadarIcon);
				// *********** //

				yield return null;


			}
		}

		if (m_preventInputByKeyboard != null) {
			if (m_sceneClickTexts.Count > 0) {
				m_preventInputByKeyboard.SetActive(true);
			} else {
				m_preventInputByKeyboard.SetActive(false);
			}
		}


		foreach (GameObject goTo in m_sceneGoTos) {
			Destroy(goTo);
		}
		
		foreach (GameObject radarIcon in m_sceneGoToRadarIcons) {
			Destroy(radarIcon);
		}

		List<BoundingBox> goToAreas = m_SEGMentEngine.GetListOfDisplacementAreas ();

		foreach (BoundingBox currentGoToBB in goToAreas) {
			SpriteRenderer goToAreaSpriteRenderer = m_gotoObject.GetComponent<SpriteRenderer> ();
			
			if (goToAreaSpriteRenderer != null) {
				GameObject currentGoTo = Instantiate (m_gotoObject, Vector3.zero, Quaternion.identity) as GameObject;
				GameObject currentGoToRadarIcon = Instantiate (m_gotoRadarIcon, Vector3.zero, Quaternion.identity) as GameObject;
				
				BoundingBox relativeGoToPosBB = currentGoToBB;
				
				SpriteSetter goToSpriteSetter = currentGoTo.GetComponent<SpriteSetter> ();
				
				if (goToSpriteSetter != null) {
					goToSpriteSetter.SetSpriteRelativelyToAnotherOne (goToAreaSpriteRenderer.sprite,
					                                                      m_backgroundDisplay.GetMainSpriteRenderer (),
					                                                      relativeGoToPosBB.x1, 
					                                                      relativeGoToPosBB.y1,
					                                                      relativeGoToPosBB.x2,
					                                                      relativeGoToPosBB.y2);
				}
				


				m_sceneGoTos.Add (currentGoTo);
				
				// Create a click text radar icon with a respected ratio
				if (currentGoTo.transform.localScale.x < currentGoTo.transform.localScale.y) {
					currentGoToRadarIcon.transform.localScale = new Vector3 (currentGoTo.transform.localScale.x, currentGoTo.transform.localScale.x, 1.0f);
				} else {
					currentGoToRadarIcon.transform.localScale = new Vector3 (currentGoTo.transform.localScale.y, currentGoTo.transform.localScale.y, 1.0f);
				}
				
				currentGoToRadarIcon.transform.localPosition = currentGoTo.transform.localPosition;
				
				m_sceneGoToRadarIcons.Add (currentGoToRadarIcon);
				// *********** //
			}
		}

		m_mustHighlightDiary = false;

		string diaryEntry = m_SEGMentEngine.PopCurrentRoomDiaryEntryName();

		if (diaryEntry != "") {
			m_UIManager.AddDiaryImage(SEGMentPath.instance.GetDiaryPath(diaryEntry));

			while (m_UIManager.IsDiaryImageStillLoading()) {
				yield return null;
			}

			if (m_SEGMentEngine.ShouldDiaryEntryBeHighlighted()) {
				m_mustHighlightDiary = true;
			}

		}


		UpdatePuzzleChecker(m_roomID);

		while (Time.fixedTime - m_launchDate < m_minimumWaitTimeForFirstLoad) {
			yield return null;
		}
		
		if (m_SEGMentEngine.GetCurrentRoomBackgroundMusicName() != "") {
			if (m_currentlyPlayedMusic != m_SEGMentEngine.GetCurrentRoomBackgroundMusicName()) {
				AudioManager.instance.PlayMusicByURL(SEGMentPath.instance.GetSoundPath(m_SEGMentEngine.GetCurrentRoomBackgroundMusicName()),
				                                     m_SEGMentEngine.MustCurrentRoomBackgroudMusicLoop());
				while (!AudioManager.instance.isMusicLoaded()) {
					yield return null;
				}
			}
		} else {
			AudioManager.instance.StopMusic();
		}


	

		yield return null;
			
		
		m_currentlyPlayedMusic = m_SEGMentEngine.GetCurrentRoomBackgroundMusicName();

		m_isSceneLoaded = true;
	}

	void UpdateUI() {
		//GenericLog.Log("UPDATE UI");

		if (m_UIManager != null) {
			//string roomDescription = m_SEGMentEngine.GetCurrentRoomDescription();
			string roomDescription = m_SEGMentEngine.PopCurrentRoomDescription();
			if (roomDescription != "") {
				m_UIManager.DisplayRoomDialog(roomDescription);
			} else {
				m_UIManager.ClearRoomDialog();
				m_UIManager.HideRoomDialog();
			}

			if (m_SEGMentEngine.CanAcceptPlayerInput()) {
				m_UIManager.DisplayPlayerInputField(m_SEGMentEngine.GetIsInputPasswordType());
				m_UIManager.ChangeQuestion(m_SEGMentEngine.GetInputQuestion());
				m_UIManager.EmptyAnswerField();
				//m_UIManager.SelectAnswerField();
			} else {
				m_UIManager.HidePlayerInputField();
			}

			if (m_mustHighlightDiary) {
				m_UIManager.HighlightDiaryEntry();
				m_mustHighlightDiary = false;
			}

		
		}
	}

	void LogPlayerCurrentInformation() {
		string logString = "";
		logString += "********* " + m_SEGMentEngine.GetPlayerName() + " *********";
		logString += "\n";
		logString += "IN ROOM";
		logString += "\n";
		logString += "Description: " + m_SEGMentEngine.GetCurrentRoomDescription();
		logString += "\n";
		logString += "Image URL: " + m_SEGMentEngine.GetCurrentRoomBackgroundImageURL();
		logString += "\n";
		logString += "Music name: " + m_SEGMentEngine.GetCurrentRoomBackgroundMusicName();
		logString += "\n";

		GenericLog.Log(logString);
	}

	public void GetAnswerFromInputField() {
		string help = "";

		if (!m_SEGMentEngine.UserAnswer(m_UIManager.GetInputFieldText(), ref help)) {
			m_UIManager.WrongAnswerBlink();

			if (help != "") {
				m_UIManager.ChangeQuestion(help);
			} else {
				m_UIManager.ChangeQuestion(m_SEGMentEngine.GetInputQuestion());
			}

			MetricLogger.instance.Log("ANSWER", false, m_UIManager.GetInputFieldText());
		} else {
			m_UIManager.ChangeQuestion(m_winString);
			MetricLogger.instance.Log("ANSWER", true, m_UIManager.GetInputFieldText());
		}
	}

	public void UserClicOnBackground(GameObject background, Vector3 pos) {
		if (!m_isClicEnabled) {
			return;
		}

		if (!IsClicEnabledAfterZoom()) {
			return;
		}

		MetricLogger.instance.Log("PLAYER_CLIC", pos.x, pos.y);

		if (m_SEGMentEngine.IsLeaveScene()) {
			StartCoroutine(GoToEndingSequence());
			return;
		}

		if (!m_SEGMentEngine.ClicToMove(pos.x, pos.y)) {
			// No radar creation if the user actually clicked on an interactable area during the same frame.
			// We also have to check in the "UserClicOnClickText" function, as we cannot know the order in which
			// the clicks are taken into account.
			if (m_lastInteractionClicTime == Time.time) {
				return;
			}

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			mousePos = new Vector3(mousePos.x, mousePos.y, 0.0f);

			if (!m_disableRadar) {
				m_lastCreatedRadar = (GameObject) Instantiate(m_radarObject, mousePos, Quaternion.identity);
				m_lastRadarCreationTime = Time.time;
			}
			

		}
	}

	public void UserMute(bool mustMute) {
		AudioManager.instance.Mute(mustMute);
	}

	public void UserClicOnClickText(GameObject clickText, Vector3 pos) {
		if (!m_isClicEnabled) {
			return;
		}

		if (!IsClicEnabledAfterZoom()) {
			return;
		}

		Text textToHandle = clickText.GetComponent<Text>();
		MustTextBeConsideredAsURL mustClicTextBeSeenAsAnURL = clickText.GetComponent<MustTextBeConsideredAsURL>();

		if (textToHandle != null) {
			MetricLogger.instance.Log("GO_TO_URL", textToHandle.text, pos.x, pos.y);
			if (mustClicTextBeSeenAsAnURL != null && mustClicTextBeSeenAsAnURL.IsMatchingTextAnURL()) {
				#if UNITY_WEBGL
				//Application.ExternalEval("window.open(\"" + textToHandle.text + \"");"); 
				Application.ExternalEval("window.open(\""+textToHandle.text+"\")");
				#else
				Application.OpenURL(textToHandle.text);
				#endif
			} else if (textToHandle.text != RETURN_STRING) {
				if ((textToHandle.text.Length > ADD_STRING.Length) && (textToHandle.text.StartsWith (ADD_STRING))) {
					string actualTextToAdd = textToHandle.text.Substring(ADD_STRING.Length, textToHandle.text.Length - ADD_STRING.Length);
					MetricLogger.instance.Log ("CLICK_TEXT_ADD", actualTextToAdd, pos.x, pos.y);
					m_UIManager.AddTextToAnswer (actualTextToAdd);
				} else {
					MetricLogger.instance.Log ("CLICK_TEXT_REPLACE", textToHandle.text, pos.x, pos.y);
					m_UIManager.ChangeTextToAnswer (textToHandle.text);
				}
			} else {
				m_UIManager.ValidateAnswer ();
			}

			SoundLauncher clickTextSoundLauncher = clickText.GetComponent<SoundLauncher> ();
			
			if (clickTextSoundLauncher != null) {
				clickTextSoundLauncher.PlayLoadedSound();
			}

		}

		// Destroy a potentially created radar if the user actually clicked on an interactable area
		m_lastInteractionClicTime = Time.time;
		if (m_lastInteractionClicTime == m_lastRadarCreationTime)  {
			if (m_lastCreatedRadar != null) {
				Destroy(m_lastCreatedRadar);
			}
		}
		// ********** //

	}

	bool IsClicEnabledAfterZoom() {
		return (Time.fixedTime > m_lastZoomTime + m_clicDisableTimeAfterZoom);
	}

	bool CheckStateObjectSolutions() {
		 

		if (m_objectStateToSolutionCheck.Count == 0) {
			return false;
		}

		foreach (KeyValuePair<SpriteSetter, List<int>> currentElement in m_objectStateToSolutionCheck) {
			int currentFrame = currentElement.Key.GetCurrentSpriteFrameNum();
			bool isValidated = false;

			foreach (int solutionFrame in currentElement.Value) {
				if (currentFrame == solutionFrame) {
					isValidated = true;
				}
			}

			if (!isValidated) {
				return false;
			}
		}

		return true;
	}

	void PreventClickOnStateObject() {
		foreach (KeyValuePair<int, GameObject> currentItemKeyValue in m_sceneItemsBySEGMentIndex) {
			SpriteSetter currentSpriteSetter = currentItemKeyValue.Value.GetComponent<SpriteSetter>();

			if (currentSpriteSetter != null) {
				currentSpriteSetter.PreventClic();
			}
		}
	}

	IEnumerator GoToEndingSequence() {
		if (!m_isGoingToEndingSequence) {
			m_isGoingToEndingSequence = true;
		
			m_UIManager.ChangeSceneFadeInOutImageColor(Color.black);

			m_UIManager.SetFadeInOutTime (m_sceneFadeInOutTime);

			PreventClickOnStateObject ();
			m_UIManager.SceneFadeOut ();

			while (!m_UIManager.IsSceneFadedOut ()) {
				yield return null;
			}

			AudioManager.instance.FadeOutMusic(1.0f);
			AudioManager.instance.StopSound();

			SceneManager.LoadScene (m_endingSequenceSceneName);
			m_isGoingToEndingSequence = false;
		}
	}


	
}
