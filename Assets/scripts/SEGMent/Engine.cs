/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading; 

namespace SEGMent {

	public class Engine {
		private LoadGameStructure m_loader;
		private Player m_player;

		private InformationManager m_informationManager;

		private float m_timeInSec;

		private bool m_isStructureLoaded;
		private string m_structureRootFileName;

		//private Thread m_loadThread;

		public Engine () {
			m_loader = new LoadGameStructure();
			m_informationManager = new InformationManager();

			m_player = new Player(m_informationManager);

			m_timeInSec = 0f;
			m_isStructureLoaded = false;
		}

		public void SetPlayerName(string playerName) {
			m_player.SetName(playerName);
		}

		public string GetPlayerName() {
			return m_player.GetName();
		}

		public int GetCurrentRoomID() {
			return m_player.GetCurrentRoomID();
		}

		public void LoadGameStructure(string mainFile) {
			m_structureRootFileName = mainFile;

			MonoBehaviourForCoroutineSingleton.instance.StartCoroutine(LoadGameStructureCoroutine());

			//m_loadThread = new Thread(LoadGameStructureThread);

			//StartCoroutine("LoadGameStructureCoroutine");
			//m_loadThread.Start();
		}

		IEnumerator LoadGameStructureCoroutine() {
			m_isStructureLoaded = false;
			m_loader.SetRootFileName(m_structureRootFileName);
			m_loader.Load(m_player);

			while (!m_loader.IsStructureLoaded()) {
				yield return null;
			}

			m_isStructureLoaded = true;


		}

		public bool ClicToMove(float x, float y) {
			return m_player.ClicToMove(x, y);
		}

		public bool ObjectStateSolutionValidated() {
			return m_player.ObjectStateSolutionValidated();
		}

		public bool PuzzleSolutionValidated() {
			return m_player.PuzzleSolutionValidated();
		}

		public bool IsStructureLoaded() {
			return m_isStructureLoaded;	
		}

		public List<BoundingBox> GetListOfDisplacementAreas() {
			return m_player.GetListOfDisplacementAreas();
		}

		public bool UserAnswer(string answer, ref string help) {
			return m_player.Answer(answer, ref help);
		}

		public void Update(float dt) {
			m_timeInSec += dt;

			m_player.Update(); 
		}

	

		public string GetCurrentRoomDescription() {
			return m_player.GetCurrentRoomDescription();
		}

		public string PopCurrentRoomDescription() {
			return m_player.PopCurrentRoomDescription ();
		}

		public string PopCurrentRoomDiaryEntryName() {
			return m_player.PopCurrentRoomDiaryEntryName();
		}

		public bool ShouldDiaryEntryBeHighlighted() {
			return m_player.ShouldDiaryEntryBeHighlighted();
		}

		public string GetCurrentRoomBackgroundImageURL() {
			return m_player.GetCurrentRoomBackgroundImageURL();
		}

		public string GetCurrentRoomBackgroundMusicName() {
			return m_player.GetCurrentRoomBackgroundMusicName();
		}

		public bool MustCurrentRoomBackgroudMusicLoop() {
			return m_player.MustCurrentRoomBackgroudMusicLoop();
		}

		public List<PLAYER_MODIF_INFO> PopPlayerModifInfos() {
			return m_informationManager.PopPlayerModifInfos();
		}

		public List<string> PopTransitionSounds() {
			return m_informationManager.PopTransitionSoundsToPlay();
		}

		public bool CanAcceptPlayerInput() {
			return m_player.CanAnswer();
		}

		public string GetInputQuestion() {
			return m_player.GetInputQuestion();
		}

		public bool GetIsInputPasswordType() {
			return m_player.GetInputIsPassword();
		}

		public List<int> GetItemIndexes() {
			return m_player.GetListOfItemIndexes();
		}

		public BoundingBox GetItemBoundingBox(int itemID) {
			return(m_player.GetMap().GetItemBoundingBox(itemID));
		}

		public string GetItemImageURL(int itemID) {
			return(m_player.GetMap().GetItemBackgroundImageURL(itemID));
		}

		public string GetItemSoundName (int itemID) {
			return(m_player.GetMap().GetItemSoundName(itemID));
		}

		public string GetItemDescription (int itemID) {
			return(m_player.GetMap().GetItemDescription(itemID));
		}

		public bool IsItemPuzllePiece (int itemID) {
			return(m_player.GetMap().GetItemIsPuzzle(itemID));
		}

		public List<int> GetItemStopFrames(int itemID) {
			return m_player.GetMap().GetItemStopFrames(itemID);
		}

		public List<int> GetItemSolutionFrames(int itemID) {
			return m_player.GetMap().GetItemSolutionFrames(itemID);
		}

		public int GetItemStartFrame(int itemID) {
			return m_player.GetMap().GetItemStartFrame(itemID);
		}

		public void SetItemStartFrame(int itemID, int startIndex) {
			m_player.GetMap().SetItemStartFrame(itemID, startIndex);	
		}

		public List<int> GetClickTextIndexes() {
			return m_player.GetListOfClickTextIndexes();
		}

		public BoundingBox GetClickTextBoundingBox(int clickTextID) {
			return(m_player.GetMap().GetClickTextBoundingBox(clickTextID));
		}
		
		public string GetClickTextSoundName (int clickTextID) {
			return(m_player.GetMap().GetClickTextSoundName(clickTextID));
		}
		
		public string GetClickTextText (int clickTextID) {
			return(m_player.GetMap().GetClickTextText(clickTextID));
		}

		public bool IsClickTextAnURL(int clickTextID) {
			return(m_player.GetMap().IsClickTextAnURL(clickTextID));
		}

		public bool isRoomChangeImmediate() {
			return m_player.isRoomChangeImmediate();
		}

		public bool IsDiagramFileModifiedSinceLoad() {
			return m_loader.IsFileModifiedSinceLoaded();
		}

		public bool IsLeaveScene() {
			return m_player.IsLeaveScene();
		}

		public List<Item> GetAllGameItems() {
			return m_player.GetAllGameItems();
		}

	}

}


