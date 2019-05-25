/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent
{
	public class GameStructureRooms: GameStructure
	{
		public static int CREATION_ERROR = -1;

		private List<Room> m_rooms;
		private List<Item> m_items;
		private List<ClickText> m_clickTexts;

		//private List<Room> m_roomsPile;

		private bool m_roomChangeShouldBeImmediate = false;
	
		public GameStructureRooms (InformationManager informationManager):base(informationManager)
		{
			m_rooms = new List<Room>();
			m_roomsPile = new List<Room>();
			m_items = new List<Item>();
			m_clickTexts = new List<ClickText>();
		}

		public int CreateRoom() {
			// The room ID matches the index in the List m_rooms;
			Room roomToAdd = new Room();
			roomToAdd.SetNodeID(m_rooms.Count);

			m_rooms.Add(roomToAdd);

			return roomToAdd.GetNodeID();
		}

		public void SetRoomDescription(int roomID, string description, bool mustBeRepeated = false) {
			if (roomID >= m_rooms.Count) {
				return;
			}

			m_rooms[roomID].SetDescription(description, mustBeRepeated);
		}

		public void SetRoomBackgroundImageURL(int roomID, string imageURL) {
			if (roomID >= m_rooms.Count) {
				return;
			}
			
			m_rooms[roomID].SetBackgroundImageURL(imageURL);
		}

		public void SetRoomBackgroundMusic(int roomID, string soundName, bool mustLoop) {
			if (roomID >= m_rooms.Count) {
				return;
			}
			
			m_rooms[roomID].SetBackgroundSoundName(soundName, mustLoop);
		}

		public void SetRoomDiaryEntry(int roomID, string diaryEntryName, bool mustBeHighlighted = false) {
			if (roomID >= m_rooms.Count) {
				return;
			}

			m_rooms[roomID].SetDiaryEntry(diaryEntryName, mustBeHighlighted);
		}

		public int CreateItem(int roomID, BoundingBox relativePosInRoom) {
			if (roomID >= m_rooms.Count) {
				return CREATION_ERROR;
			}
			
			// The item ID matches the index in the List m_items;
			Item itemToAdd = new Item(m_rooms[roomID], relativePosInRoom);
			itemToAdd.SetNodeID(m_items.Count);
			
			m_items.Add(itemToAdd);
			
			return itemToAdd.GetNodeID();
		}

		public void SetItemBackgroundImageURL(int itemID, string imageURL) {
			if (itemID >= m_items.Count) {
				return;
			}
			
			m_items[itemID].SetImageURL(imageURL);
		}

		public string GetItemBackgroundImageURL(int itemID) {
			if (itemID >= m_items.Count) {
				return "";
			}
			
			return(m_items[itemID].GetImageURL());
		}

		public BoundingBox GetItemBoundingBox(int itemID) {
			if (itemID >= m_items.Count) {
				return null;
			}
			
			return(m_items[itemID].GetRelativePosition());
		}

		public void SetItemSoundName(int itemID, string soundName) {
			if (itemID >= m_items.Count) {
				return;
			}
			
			m_items[itemID].SetSoundName(soundName);
		}

		public string GetItemDescription(int itemID) {
			if (itemID >= m_items.Count) {
				return "";
			}
			
			return(m_items[itemID].GetDescription());
		}

		public void SetItemDescription(int itemID, string description) {
			if (itemID >= m_items.Count) {
				return;
			}
			
			m_items[itemID].SetDescription(description);
		}

		public bool GetItemIsPuzzle(int itemID) {
			if (itemID >= m_items.Count) {
				return false;
			}

			return(m_items[itemID].IsPuzzlePiece());
		}

		public void SetItemIsPuzzle(int itemID, bool isPuzzle) {
			if (itemID >= m_items.Count) {
				return;
			}

			m_items[itemID].SetPuzzlePiece(isPuzzle);
		}

		public void AddItemStopState(int itemID, int stopStateFrame) {
			if (itemID >= m_items.Count) {
				return;
			}

			m_items[itemID].AddStopFrame(stopStateFrame);
		}

		public List<int> GetItemStopFrames(int itemID) {
			if (itemID >= m_items.Count) {
				return null;
			}

			return m_items[itemID].GetStopFrames();
		}

		public void AddItemSolutionState(int itemID, int solutionStateFrame) {
			if (itemID >= m_items.Count) {
				return;
			}

			m_items[itemID].AddSolutionFrame(solutionStateFrame);
		}

		public List<int> GetItemSolutionFrames(int itemID) {
			if (itemID >= m_items.Count) {
				return null;
			}

			return m_items[itemID].GetSolutionFrames();
		}
		
		public string GetItemSoundName(int itemID) {
			if (itemID >= m_items.Count) {
				return "";
			}
			
			return(m_items[itemID].GetSoundName());
		}

		public int GetItemStartFrame(int itemID) {
			if (itemID >= m_items.Count) {
				return -1;
			}

			return(m_items[itemID].GetStartFrame());
		}

		public void SetItemStartFrame(int itemID, int frameIndex) {
			if (itemID >= m_items.Count) {
				return;
			}

			m_items[itemID].SetStartFrame(frameIndex);
			
		}

		public int CreateClickText(int roomID, BoundingBox relativePosInRoom) {
			if (roomID >= m_rooms.Count) {
				return CREATION_ERROR;
			}
			
			// The item ID matches the index in the List m_items;
			ClickText clickTextToAdd = new ClickText(m_rooms[roomID], relativePosInRoom);
			clickTextToAdd.SetNodeID(m_clickTexts.Count);
			
			m_clickTexts.Add(clickTextToAdd);
			
			return clickTextToAdd.GetNodeID();
		}
		
		public BoundingBox GetClickTextBoundingBox(int clickTextID) {
			if (clickTextID >= m_clickTexts.Count) {
				return null;
			}
			
			return(m_clickTexts[clickTextID].GetRelativePosition());
		}
		
		public void SetClickTextSoundName(int clickTextID, string soundName) {
			if (clickTextID >= m_clickTexts.Count) {
				return;
			}
			
			m_clickTexts[clickTextID].SetSoundName(soundName);
		}
		
		public string GetClickTextText(int clickTextID) {
			if (clickTextID >= m_clickTexts.Count) {
				return "";
			}
			
			return(m_clickTexts[clickTextID].GetText());
		}

		public bool IsClickTextAnURL(int clickTextID) {
			if (clickTextID >= m_clickTexts.Count) {
				return false;
			}

			return(m_clickTexts[clickTextID].IsURL());
		}
		
		public void SetClickTextText(int clickTextID, string text, bool isURL) {
			if (clickTextID >= m_clickTexts.Count) {
				return;
			}
			
			m_clickTexts[clickTextID].SetText(text);
			m_clickTexts[clickTextID].SetIsURL(isURL);
		}
		
		public string GetClickTextSoundName(int clickTextID) {
			if (clickTextID >= m_clickTexts.Count) {
				return "";
			}
			
			return(m_clickTexts[clickTextID].GetSoundName());
		}

		public int CreateSolutionTransition(int roomIDFrom, int roomIDTo, string question, List<string> solutions, Dictionary<string, string> wrongAnswers, bool isPassword, bool isImmediate, bool isUnique) {
			if (roomIDFrom >= m_rooms.Count || roomIDTo >= m_rooms.Count) {
				return -1;
			}

			SolutionTransition transitionToAdd = new SolutionTransition(m_rooms[roomIDFrom], m_rooms[roomIDTo], question, solutions, wrongAnswers, isPassword, isImmediate, isUnique);
			transitionToAdd.SetTransitionID(m_transitions.Count);

			m_transitions.Add(transitionToAdd);

			return transitionToAdd.GetTransitionID();
		}

		public int CreateClickableTransition(int roomIDFrom, int roomIDTo, BoundingBox relativeBB, bool isImmediate, bool isUnique) {
			if (roomIDFrom >= m_rooms.Count || roomIDTo >= m_rooms.Count) {
				return -1;
			}
			
			AreaClicTransition transitionToAdd = new AreaClicTransition(m_rooms[roomIDFrom], m_rooms[roomIDTo], relativeBB, isImmediate, isUnique);
			transitionToAdd.SetTransitionID(m_transitions.Count);
			
			m_transitions.Add(transitionToAdd);

			return transitionToAdd.GetTransitionID();
		}

		public int CreateBackTransition(int roomIDFrom, BoundingBox relativeBB, bool isImmediate) {
			if (roomIDFrom >= m_rooms.Count) {
				return -1;
			}

			AreaClicTransition transitionToAdd = new AreaClicTransition(m_rooms[roomIDFrom], null, relativeBB, isImmediate);
			transitionToAdd.SetTransitionID(m_transitions.Count);

			m_transitions.Add(transitionToAdd);

			return transitionToAdd.GetTransitionID();
		}

		public int CreateStateObjectSolutionTransition(int roomIDFrom, int roomIDTo, bool isImmediate, bool isUnique) {
			if (roomIDFrom >= m_rooms.Count) {
				return -1;
			}

			ObjectStateTransition transitionToAdd = new ObjectStateTransition(m_rooms[roomIDFrom], m_rooms[roomIDTo], isImmediate, isUnique);
			transitionToAdd.SetTransitionID(m_transitions.Count);

			m_transitions.Add(transitionToAdd);

			return transitionToAdd.GetTransitionID();
		}

		public int CreatePuzzleSolutionTransition(int roomIDFrom, int roomIDTo, bool isImmediate, bool isUnique) {
			if (roomIDFrom >= m_rooms.Count) {
				return -1;
			}

			PuzzleTransition transitionToAdd = new PuzzleTransition(m_rooms[roomIDFrom], m_rooms[roomIDTo], isImmediate, isUnique);
			transitionToAdd.SetTransitionID(m_transitions.Count);

			m_transitions.Add(transitionToAdd);

			return transitionToAdd.GetTransitionID();
		}

		public void TeleportToRoom(int roomID) {
			if (roomID >= m_rooms.Count) {
				return;
			}

			TeleportToRoom(m_rooms[roomID]);
		}

		void TeleportToRoom(Room roomToGoTo) {
			m_token = roomToGoTo;

			//TODO: manage transitions in an event manager !
		}

		public Room GetCurrentRoom() {
			if (m_token.GetNodeType() == NODE_TYPE.ROOM_TYPE) {
				return (Room) m_token;
			} else {
				return null;
			}
		}

		public int GetCurrentRoomID() {
			Room currentRoom = GetCurrentRoom();
			if (currentRoom != null) {
				return currentRoom.GetNodeID();
			} else {
				return -1;
			}
		}

		public bool ObjectStateSolutionValidated() {
			
			Room currentRoom = GetCurrentRoom();

			if (currentRoom != null) {
				List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions();

				foreach (GraphTransition currentTransition in outGoingTransitions) {
					if (currentTransition.GetTransitionType() == TRANSITION_TYPE.OBJECT_STATE_SOLUTION_TYPE) {
					//	GenericLog.Log("heyaaaaaaa2");
						FireTransition(currentTransition);
					//	GenericLog.Log("heyaaaaaaa3");
					//	m_roomChangeShouldBeImmediate = currentTransition.IsImmediate();
						return true;
					}
				}
			}

			return false;
		}

		public bool PuzzleSolutionValidated() {

			Room currentRoom = GetCurrentRoom();

			if (currentRoom != null) {
				List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions();

				foreach (GraphTransition currentTransition in outGoingTransitions) {
					if (currentTransition.GetTransitionType() == TRANSITION_TYPE.PUZZLE_SOLUTION_TYPE) {
						//	GenericLog.Log("heyaaaaaaa2");
						FireTransition(currentTransition);
						//	GenericLog.Log("heyaaaaaaa3");
						//	m_roomChangeShouldBeImmediate = currentTransition.IsImmediate();
						return true;
					}
				}
			}

			return false;
		}



		public bool ReceiveAnswer(string answer, ref string help) {
			Room currentRoom = GetCurrentRoom();

			help = "";

			if (currentRoom != null) {
				List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions();

				foreach (GraphTransition currentTransition in outGoingTransitions) {
					if (currentTransition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) {
						SolutionTransition currentSolutionTransition = (SolutionTransition) currentTransition;

						List<string> solutions = currentSolutionTransition.GetSolutions();

                        var wrongAnswers = currentSolutionTransition.GetWrongAnswerToHelp();

                        foreach (KeyValuePair<string, string> wrongAnswerToHelp in wrongAnswers) {
							if (string.Equals(wrongAnswerToHelp.Key, answer, System.StringComparison.CurrentCultureIgnoreCase)) {
								help = wrongAnswerToHelp.Value;
								return false;
							}
						}

						foreach (string currentSolution in solutions) {
						//	if (currentSolution == answer) {
							if (string.Equals(currentSolution, answer, System.StringComparison.CurrentCultureIgnoreCase)) {
								FireTransition(currentTransition);
								m_roomChangeShouldBeImmediate = currentSolutionTransition.IsImmediate();
								return true;
							}
						}

                        if (wrongAnswers.ContainsKey("__default__"))
                        {
                            help = wrongAnswers["__default__"];
                            return false;
                        }


					}
				}
			}

			return false;
		}

		public bool IsLeaveRoom() {
			Room currentRoom = GetCurrentRoom();

			if (currentRoom != null) {
				return (currentRoom.GetOutgoingTransitions().Count == 0);
			}

			return false;
		}

		public bool ClicToMove(float x, float y) {
			Room currentRoom = GetCurrentRoom();
			
			if (currentRoom != null) {
				List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions();
				
				foreach (GraphTransition currentTransition in outGoingTransitions) {
					if ((currentTransition.GetTransitionType () == TRANSITION_TYPE.CLIC_AREA_TYPE)
						|| (currentTransition.GetTransitionType () == TRANSITION_TYPE.BACK_TRANSITION)) {
						AreaClicTransition currentClicTransition = (AreaClicTransition)currentTransition;
						BoundingBox currentTransitionBB = currentClicTransition.GetRelativeBoundingBox ();

						if ((x >= currentTransitionBB.x1) && (x <= currentTransitionBB.x2) && (y >= currentTransitionBB.y1) && (y <= currentTransitionBB.y2)) {
							if (currentTransition.GetTransitionType () == TRANSITION_TYPE.CLIC_AREA_TYPE) {
								m_roomsPile.Add ((Room)m_token);
								FireTransition (currentTransition);
								m_roomChangeShouldBeImmediate = currentClicTransition.IsImmediate ();
							} else {
								if (m_roomsPile.Count > 0) {
									Room roomToGoBack = m_roomsPile [m_roomsPile.Count - 1];
									m_roomsPile.RemoveAt (m_roomsPile.Count - 1);

									TeleportToRoom (roomToGoBack);
									//GenericLog.Log ("AH AHHHHH");

								}
							}

							return true;
						}
						//BoundingBox transitionBB = currentClicTransition.
					} 
				}
			}
			
			return false;
		}

		public List<BoundingBox> GetListOfDisplacementAreas ()
		{
			List<BoundingBox> resultList = new List<BoundingBox> ();

			Room currentRoom = GetCurrentRoom ();
			
			if (currentRoom != null) {
				List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions ();

				foreach (GraphTransition currentTransition in outGoingTransitions) {
					if ((currentTransition.GetTransitionType () == TRANSITION_TYPE.CLIC_AREA_TYPE) ||
						(currentTransition.GetTransitionType () == TRANSITION_TYPE.BACK_TRANSITION)){
						AreaClicTransition currentClicTransition = (AreaClicTransition)currentTransition;
						BoundingBox currentTransitionBB = currentClicTransition.GetRelativeBoundingBox ();

						resultList.Add (currentTransitionBB);
					}
				}
			}

			return resultList;

		}

		public bool isRoomChangeImmediate() {
			return m_roomChangeShouldBeImmediate;
		}

		public List<Item> getAllItems() {
			return m_items;
		}

	}
}

