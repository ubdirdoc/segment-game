/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


namespace SEGMent
{
    public class RoomDescription
    {
        public string backgroundImageURL = "";
        public string backgroundMusicName = "";

        public bool backgroundMusicMustLoop = true;

        public string description = "";
        public bool descriptionMustBeRepeated = false;

        public string diaryItem = "";
        public bool newDiaryItemMustBeHighlighted = false;

        public List<string> includedItems = new List<string>();
        public List<string> includedClickText = new List<string>();

        public BoundingBox bb = new BoundingBox();

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** ROOM ***\n";
            resultString += "Background Image URL: " + backgroundImageURL + "\n";
            resultString += "Background Music Name: " + backgroundMusicName + "\n";
            resultString += "Background Music Must Loop: " + backgroundMusicMustLoop + "\n";
            resultString += "Description: " + description + "\n";
            resultString += "Description repeated: " + descriptionMustBeRepeated + "\n";
            resultString += "Diary Item: " + diaryItem + "\n";
            resultString += "BB: " + bb.ToString() + "\n";
            foreach (string itemsID in includedItems)
            {
                resultString += "Included item: " + itemsID + "\n";
            }
            foreach (string clickTextID in includedClickText)
            {
                resultString += "Included click text: " + clickTextID + "\n";
            }

            return resultString;
        }
    }

    public class ItemDescription
    {
        public bool isPuzzlePiece = false;
        public string imageURL = "";
        public string soundName = "";

        public string description = "";

        public BoundingBox absoluteBB = new BoundingBox();
        public BoundingBox relativeBB = new BoundingBox();

        public string includingRoomID = "";

        public List<int> stopFrames = new List<int>();
        public List<int> solutionFrames = new List<int>();

        public override string ToString()
        {
            string resultString = "";

            if (isPuzzlePiece)
            {
                resultString += "*** PUZZLE ***\n";
            }
            else
            {
                if (stopFrames.Count == 0)
                {
                    resultString += "*** ITEM ***\n";
                }
                else
                {
                    resultString += "*** STATES ITEM *** [ ";
                    foreach (int i in stopFrames)
                    {
                        resultString += i + " ";
                    }
                    resultString += "] [ ";
                    foreach (int i in solutionFrames)
                    {
                        resultString += i + " ";
                    }
                    resultString += "]\n";
                }
            }

            resultString += "Image URL: " + imageURL + "\n";
            resultString += "Sound Name: " + soundName + "\n";
            resultString += "Description: " + description + "\n";
            resultString += "Absolute BB: " + absoluteBB.ToString() + "\n";
            resultString += "Relative BB: " + relativeBB.ToString() + "\n";
            resultString += "In room: " + includingRoomID + "\n";

            return resultString;
        }
    }

    public class ClickTextDescription
    {
        public string soundName = "";

        public string text = "";
        public bool isURL = false;

        public BoundingBox absoluteBB = new BoundingBox();
        public BoundingBox relativeBB = new BoundingBox();

        public string includingRoomID = "";

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** CLICK TEXT ***\n";
            resultString += "Sound Name: " + soundName + "\n";
            resultString += "Text: " + text + "\n";
            resultString += "Is URL ? " + isURL + "\n";
            resultString += "Absolute BB: " + absoluteBB.ToString() + "\n";
            resultString += "Relative BB: " + relativeBB.ToString() + "\n";
            resultString += "In room: " + includingRoomID + "\n";

            return resultString;
        }
    }

    public class DisplacementDescription
    {
        public bool isBackDisplacement = false;

        public string roomFromID = "";
        public string roomToID = "";

        public BoundingBox bb = new BoundingBox();

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** DISPLACEMENT ***\n";
            resultString += "Room from: " + roomFromID + "\n";
            resultString += "Room to: " + roomToID + "\n";
            resultString += "BB: " + bb.ToString() + "\n";

            return resultString;
        }
    }

    public class SolutionDescription
    {


        public List<string> expectedInputs = new List<string>();
        public bool isPassword = false;

        //public string includingRoomId = "";

        //public BoundingBox bb = new BoundingBox();

        public override string ToString()
        {
            string resultString = "";


            resultString += "*** INPUT SOLUTION ***\n";
            resultString += "is password ?" + isPassword + " \n";

            foreach (string input in expectedInputs)
            {
                resultString += "Expected input: " + input + "\n";
            }



            //			resultString += "Including room: " + includingRoomId + "\n";
            //resultString += "BB: " + bb.ToString() + "\n";

            return resultString;
        }
    }

    public class WrongAnswersDescription
    {
        public Dictionary<string, string> wrongAnswersToHelp = new Dictionary<string, string>();

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** WRONG ANSWER ***\n";
            foreach (KeyValuePair<string, string> wrongAnswer in wrongAnswersToHelp)
            {
                resultString += "Wrong Answer: " + wrongAnswer.Key + " // " + wrongAnswer.Value + "\n";
            }

            //			resultString += "Including room: " + includingRoomId + "\n";
            //resultString += "BB: " + bb.ToString() + "\n";

            return resultString;
        }
    }

    public class InformationDescription
    {
        public string information = "";
        public bool mustRemain = false;

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** INFORMATION ***\n";
            resultString += "Information: " + information + "\n";
            resultString += "Must remain: " + mustRemain + "\n";

            return resultString;
        }
    }

    public class DiaryDescription
    {
        public string diaryItemName = "";
        public bool mustBeHighlighted = false;

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** DIARY ***\n";
            resultString += "Diary: " + diaryItemName + "\n";
            resultString += "Must be highlighted: " + mustBeHighlighted + "\n";

            return resultString;
        }
    }

    public class StopStateDescription
    {
        public int stopFrameValue = 0;
        public bool isSolutionState = false;

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** STOP STATE ***\n";
            resultString += "Stop frame value: " + stopFrameValue + "\n";
            resultString += "Is solution state ? " + isSolutionState + "\n";

            return resultString;
        }
    }

    public class SoundDescription
    {
        public string soundName = "";
        public bool mustLoop = true;

        public override string ToString()
        {
            string resultString = "";

            resultString += "*** SOUND ***\n";
            resultString += "Sound name: " + soundName + "\n";
            resultString += "Must Loop: " + mustLoop + "\n";

            return resultString;
        }
    }

    public class TransitionDescription
    {
        public bool isBackTransition = false;
        public bool isAObjectStateTransition = false;
        public bool isAPuzzleTransition = false;

        public string fromID = "";
        public string toID = "";

        public List<string> soundNames = new List<string>();

        public BoundingBox outgoingBoudingBox = null;

        public string question = "?";
        public List<string> solutions = new List<string>();
        public Dictionary<string, string> wrongAnswersToHelp = new Dictionary<string, string>();

        public bool isPassword = false;

        public bool isImmediateTransition = false;
        public bool isUniqueTransition = false;


        public override string ToString()
        {
            string resultString = "";

            if (isAObjectStateTransition)
            {
                resultString += "*** OBJECT STATE TRANSITION ***\n";
            }
            else
            {
                resultString += "*** TRANSITION ***\n";
            }

            if (!isBackTransition)
            {
                resultString += fromID + "-->" + toID + "\n";
            }
            else
            {
                resultString += fromID + "-->" + "BACK" + "\n";
            }
            foreach (string currentSound in soundNames)
            {
                resultString += "Sound: " + currentSound + "\n";
            }
            if (outgoingBoudingBox != null)
            {
                resultString += "->" + outgoingBoudingBox + "\n";
            }



            resultString += "Question: " + question + "\n";
            foreach (string currentSolution in solutions)
            {
                resultString += "Solution: " + currentSolution + "\n";
            }
            resultString += "Is Password " + isPassword + "\n";
            foreach (KeyValuePair<string, string> wrongAnswer in wrongAnswersToHelp)
            {
                resultString += "Wrong Answer: " + wrongAnswer.Key + " // " + wrongAnswer.Value + "\n";
            }
            resultString += "Is Immediate: " + isImmediateTransition + "\n";
            resultString += "Is Unique: " + isUniqueTransition + "\n";

            return resultString;
        }
    }

    public class LoadGameStructure
    {
        private DateTime m_fileSystemTimeWhenLoaded = DateTime.MinValue;
        private string m_rootFileName = "./game_data/Main.dia";

        private bool m_isStructureLoaded = false;

        private Dictionary<string, RoomDescription> m_diagramIDToRoomDescription;
        private Dictionary<string, ItemDescription> m_diagramIDToItemDescription;
        private Dictionary<string, DisplacementDescription> m_diagramIDToDisplacementDescription;
        private Dictionary<string, SolutionDescription> m_diagramIDToSolutionDescription;
        private Dictionary<string, WrongAnswersDescription> m_diagramIDToWrongAnswersDescription;
        private Dictionary<string, ClickTextDescription> m_diagramIDToClickTextDescription;

        private Dictionary<string, InformationDescription> m_diagramIDToInformationDescription;
        private Dictionary<string, StopStateDescription> m_diagramIDToStopStateDescription;
        private Dictionary<string, SoundDescription> m_diagramIDToSoundDescription;

        private Dictionary<string, DiaryDescription> m_diagramIDToDiaryDescription;

        private Dictionary<string, TransitionDescription> m_diagramIDToTransitionDescription;

        /*
		// ID of the initial state object in the diagram
		private string m_initialStateID = "";
		*/

        // IDs of all the initial states objects. Ideally, only one must exist in the diagram
        // but in case of a "wrong" copy/paste, we store all the IDs, to select one with an actual
        // link to a room.
        private List<string> m_initialStateIDs;

        private List<string> m_ObjectStateSolutionIDs;

        private List<string> m_puzzleSolutionIDs;

        // ID of the room represented by the initial state object
        private string m_inititalRoomID = "";

        public LoadGameStructure()
        {
            m_isStructureLoaded = false;

            m_diagramIDToRoomDescription = new Dictionary<string, RoomDescription>();
            m_diagramIDToItemDescription = new Dictionary<string, ItemDescription>();
            m_diagramIDToDisplacementDescription = new Dictionary<string, DisplacementDescription>();
            m_diagramIDToSolutionDescription = new Dictionary<string, SolutionDescription>();
            m_diagramIDToWrongAnswersDescription = new Dictionary<string, WrongAnswersDescription>();
            m_diagramIDToClickTextDescription = new Dictionary<string, ClickTextDescription>();

            m_diagramIDToInformationDescription = new Dictionary<string, InformationDescription>();
            m_diagramIDToStopStateDescription = new Dictionary<string, StopStateDescription>();
            m_diagramIDToSoundDescription = new Dictionary<string, SoundDescription>();

            m_diagramIDToDiaryDescription = new Dictionary<string, DiaryDescription>();

            m_diagramIDToTransitionDescription = new Dictionary<string, TransitionDescription>();

            m_initialStateIDs = new List<string>();
            m_ObjectStateSolutionIDs = new List<string>();
            m_puzzleSolutionIDs = new List<string>();
        }

        public void SetRootFileName(string rootFileName)
        {
            m_rootFileName = rootFileName;
        }

        public bool IsFileModifiedSinceLoaded()
        {
            if (!File.Exists(m_rootFileName))
            {
                return false;
            }

            if (m_fileSystemTimeWhenLoaded == DateTime.MinValue)
            {
                return false;
            }

            return (m_fileSystemTimeWhenLoaded != File.GetLastWriteTime(m_rootFileName));
        }

        public void Load(Player playerToSet)
        {
            MonoBehaviourForCoroutineSingleton.instance.StartCoroutine(LoadCoroutine(playerToSet));
        }

        IEnumerator LoadXML(Player playerToSet)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (m_rootFileName.StartsWith("file:///"))
                m_rootFileName = m_rootFileName.Remove(0, 8);
            GenericLog.Log(m_rootFileName);



            m_fileSystemTimeWhenLoaded = File.GetLastWriteTime(m_rootFileName);


            GenericLog.Log(m_rootFileName);
            //	#if UNITY_WEBGL
            WWW xmlwww = new WWW(m_rootFileName);
            while (!xmlwww.isDone)
            {
                //		GenericLog.Log("LOAD YIELD");
                yield return null;
            }

            xmlDoc.LoadXml(xmlwww.text);

            GenericLog.Log("XML LOAD COMPLETE");
            //return;
            //#else
            //	xmlDoc.Load (m_rootFileName);
            //#endif


            LoadNodes(xmlDoc);
            LoadTransitions(xmlDoc);

            InclusionManagement();
            LinkManagement();

            DisplayAllRooms();
            DisplayAllItems();
            DisplayAllTransitions();
            DisplayAllClickText();

            GenericLog.Log("Initial Room ID = " + m_inititalRoomID);

            TranslateDiagrammeElementsToSEGMentEngineElements(playerToSet);

            m_isStructureLoaded = true;
            yield return null;
        }

        IEnumerator LoadJSON(Player playerToSet)
        {
            Debug.Log(m_rootFileName);
            var loader = new SEGMent.Json.LoadJson();
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (m_rootFileName.StartsWith("file:///"))
                m_rootFileName = m_rootFileName.Remove(0, 8);
#endif
            m_fileSystemTimeWhenLoaded = File.GetLastWriteTime(m_rootFileName);

			WWW xmlwww = new WWW(m_rootFileName);
			while (!xmlwww.isDone) {
				yield return null;
			}

			loader.Load(xmlwww.text, playerToSet);
			GenericLog.Log("JSON LOAD COMPLETE");
			
			m_isStructureLoaded = true;
			yield return null;
		}
		IEnumerator LoadCoroutine(Player playerToSet) {
            
            if (m_rootFileName.EndsWith(".dia") || m_rootFileName.EndsWith(".seg"))
			{
				return LoadXML(playerToSet);				
			}
			else
			{
				return LoadJSON(playerToSet);
			}
		}

		public bool IsStructureLoaded() {
			return m_isStructureLoaded;
		}
		
		public void LoadNodes (XmlDocument xmlDoc)
		{
			XmlNodeList objectsList = xmlDoc.GetElementsByTagName (GlobalXMLTags.OBJECT);
			
			foreach (XmlNode objectInfo in objectsList) {
				XmlAttribute objectType = objectInfo.Attributes [GlobalXMLTags.TYPE_ATTRIBUTE];
				
				if (objectType != null) {
					if ((objectType.Value == GlobalXMLTags.ROOM_TYPE) || (objectType.Value == GlobalXMLTags.OBJECT_TYPE)) {
						// For DIA file, the XML tags are identical. This should be adapted for I-Score for instance.
						// The Objects and Rooms are discriminated through the directory in which the image is located.

						XmlElement roomOrObjectElement = (XmlElement)objectInfo;
						XmlNodeList attributesList = roomOrObjectElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
						
						foreach (XmlNode roomOrObjectAttributeInfo in attributesList) {
							XmlAttribute roomAttributeName = roomOrObjectAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
							
							if (roomAttributeName != null) {
								if (roomAttributeName.Value == GlobalXMLTags.FILE_ATTRIBUTE) {
									string currentFileName = GetFileURL (roomOrObjectAttributeInfo);
									currentFileName = currentFileName.Replace("\\", "/");

									if (currentFileName.Contains(GlobalXMLTags.ROOMS_DIRECTORY)) {
										LoadRooms (objectInfo);
									} else if (currentFileName.Contains(GlobalXMLTags.ITEMS_DIRECTORY)) {
										LoadItems(objectInfo, false);
									} else if (currentFileName.Contains(GlobalXMLTags.PUZZLES_DIRECTORY)) {
										LoadItems(objectInfo, true);
									}
								}
							}
						}


					} else if (objectType.Value == GlobalXMLTags.INFORMATION_TYPE) {
						LoadInformation (objectInfo);
					} else if (objectType.Value == GlobalXMLTags.DIARY_TYPE) {
						LoadDiary (objectInfo);
					} else if (objectType.Value == GlobalXMLTags.OBJECT_STOP_STATE_TYPE) {
						LoadObjectStopState (objectInfo);
					} else if (objectType.Value == GlobalXMLTags.DISPLACEMENT_AREA_TYPE) {
						LoadDisplacements (objectInfo);
					} else if (objectType.Value == GlobalXMLTags.SOLUTION_TYPE) {
						LoadSolutions (objectInfo);	
					} else if (objectType.Value == GlobalXMLTags.WRONG_ANSWER_TYPE) {
						LoadWrongAnswers (objectInfo);	
					} else if (objectType.Value == GlobalXMLTags.SOUND_TYPE) {
						LoadSounds (objectInfo, true);	
					} else if (objectType.Value == GlobalXMLTags.SOUND_FORCE_NO_LOOP_TYPE) {
						LoadSounds (objectInfo, false);
					} else if (objectType.Value == GlobalXMLTags.INITIAL_STATE_TYPE) {
						LoadInitialState (objectInfo);	
					} else if (objectType.Value == GlobalXMLTags.OBJECT_STATES_SOLUTION_TYPE) {
						LoadObjectStateSolutionTransition(objectInfo);
					} else if (objectType.Value == GlobalXMLTags.PUZZLE_SOLUTION_TYPE) {
						LoadPuzzleSolutionTransition(objectInfo);
					} else if (objectType.Value == GlobalXMLTags.CLICK_TEXT_TYPE) {
						LoadClickText (objectInfo, false);
					} else if (objectType.Value == GlobalXMLTags.URL_TYPE) {
						LoadClickText(objectInfo, true);	
					} else {
						GenericLog.Log (objectType.Value);
					}
					
				}
			}
		}
		
		public void LoadRooms (XmlNode roomInfo)
		{
			string roomID = "";
			XmlAttribute idAttribute = roomInfo.Attributes[GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				roomID = idAttribute.Value;

				if (!m_diagramIDToRoomDescription.ContainsKey(roomID)) {
					m_diagramIDToRoomDescription[roomID] = new RoomDescription();
				} else {
					GenericLog.Log ("WARNING : One room with the same diagram id " + roomID + " already exists!");
					return;
				}
			} 

			else {
				GenericLog.Log ("WARNING : One room in the diagram does not have a diagram id!");
				return;
			}

			XmlElement roomElement = (XmlElement)roomInfo;
			XmlNodeList attributesList = roomElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode roomAttributeInfo in attributesList) {
				XmlAttribute roomAttributeName = roomAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (roomAttributeName != null) {
					if (roomAttributeName.Value == GlobalXMLTags.BOUNDING_BOX_ATTRIBUTE) {
						BoundingBox currentBB = GetBoundingBox (roomAttributeInfo);

						m_diagramIDToRoomDescription[roomID].bb = currentBB;
					} 
					
					else if (roomAttributeName.Value == GlobalXMLTags.FILE_ATTRIBUTE) {
						string currentFileName = GetFileURL (roomAttributeInfo);
						currentFileName = currentFileName.Replace("\\", "/");
						m_diagramIDToRoomDescription[roomID].backgroundImageURL = currentFileName;
					}
				}
			}

			GenericLog.Log(m_diagramIDToRoomDescription[roomID].ToString());
		}


		public void LoadItems (XmlNode objectInfo, bool isPuzzle = false)
		{
			string itemID = "";
			XmlAttribute idAttribute = objectInfo.Attributes[GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				itemID = idAttribute.Value;
				
				if (!m_diagramIDToItemDescription.ContainsKey(itemID)) {
					m_diagramIDToItemDescription[itemID] = new ItemDescription();
				} else {
					GenericLog.Log ("WARNING : One object with the same diagram id " + itemID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING : One room in the diagram does not have a diagram id!");
				return;
			}
			
			XmlElement itemElement = (XmlElement)objectInfo;
			XmlNodeList attributesList = itemElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode itemAttributeInfo in attributesList) {
				XmlAttribute itemAttributeName = itemAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (itemAttributeName != null) {
					if (itemAttributeName.Value == GlobalXMLTags.BOUNDING_BOX_ATTRIBUTE) {
						BoundingBox currentBB = GetBoundingBox (itemAttributeInfo);
						
						m_diagramIDToItemDescription[itemID].absoluteBB = currentBB;
					} 
					
					else if (itemAttributeName.Value == GlobalXMLTags.FILE_ATTRIBUTE) {
						string currentFileName = GetFileURL (itemAttributeInfo);

						currentFileName = currentFileName.Replace("\\", "/");
						m_diagramIDToItemDescription[itemID].imageURL = currentFileName;
					}
				}
			}

			m_diagramIDToItemDescription[itemID].isPuzzlePiece = isPuzzle;
			
			GenericLog.Log(m_diagramIDToItemDescription[itemID].ToString());
		}
		
		public void LoadInformation (XmlNode informationInfo)
		{
			string informationID = "";
			XmlAttribute idAttribute = informationInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				informationID = idAttribute.Value;
				
				if (!m_diagramIDToInformationDescription.ContainsKey (informationID)) {
					m_diagramIDToInformationDescription [informationID] = new InformationDescription ();
				} else {
					GenericLog.Log ("WARNING : One information with the same diagram id " + informationID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING : One information in the diagram does not have a diagram id!");
				return;
			}
			
			
			XmlElement infoElement = (XmlElement)informationInfo;
			XmlNodeList attributesList = infoElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode infoAttributeInfo in attributesList) {
				XmlAttribute infoAttributeName = infoAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (infoAttributeName != null) {
					if (infoAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string currentInformation = GetDIAString (infoAttributeInfo);
						
						m_diagramIDToInformationDescription [informationID].information = currentInformation;

						if (GetDIAFontStyle(infoAttributeInfo) == GlobalXMLTags.FONT_STYLE_BOLD) {
							m_diagramIDToInformationDescription [informationID].mustRemain = true;
						} else {
							m_diagramIDToInformationDescription [informationID].mustRemain = false;
						}
					}
					
				}
			}
			
			GenericLog.Log(m_diagramIDToInformationDescription[informationID].ToString());	
		}

		public void LoadDiary (XmlNode diaryInfo)
		{
			string diaryID = "";
			XmlAttribute idAttribute = diaryInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];

			if (idAttribute != null) {
				diaryID = idAttribute.Value;

				if (!m_diagramIDToDiaryDescription.ContainsKey (diaryID)) {
					m_diagramIDToDiaryDescription [diaryID] = new DiaryDescription ();
				} else {
					GenericLog.Log ("WARNING : One diary item with the same diagram id " + diaryID + " already exists!");
					return;
				}
			} 

			else {
				GenericLog.Log ("WARNING : One information in the diagram does not have a diagram id!");
				return;
			}


			XmlElement diaryElement = (XmlElement)diaryInfo;
			XmlNodeList attributesList = diaryElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);

			foreach (XmlNode diaryAttributeInfo in attributesList) {
				XmlAttribute diaryAttributeName = diaryAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];

				if (diaryAttributeName != null) {
					if (diaryAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string currentDiaryInformation = GetDIAString (diaryAttributeInfo);

						m_diagramIDToDiaryDescription [diaryID].diaryItemName = currentDiaryInformation;

						if (GetDIAFontStyle(diaryAttributeInfo) == GlobalXMLTags.FONT_STYLE_BOLD) {
							m_diagramIDToDiaryDescription [diaryID].mustBeHighlighted = true;
						} else {
							m_diagramIDToDiaryDescription [diaryID].mustBeHighlighted = false;
						}
					}

				

				}
			}

			GenericLog.Log(m_diagramIDToDiaryDescription[diaryID].ToString());	
		}

		public void LoadObjectStopState (XmlNode informationInfo)
		{
			GenericLog.Log("STOP STATE !");

			string stopStateID = "";
			XmlAttribute idAttribute = informationInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];

			if (idAttribute != null) {
				stopStateID = idAttribute.Value;

				if (!m_diagramIDToStopStateDescription.ContainsKey (stopStateID)) {
					m_diagramIDToStopStateDescription [stopStateID] = new StopStateDescription ();
				} else {
					GenericLog.Log ("WARNING : One stop state with the same diagram id " + stopStateID + " already exists!");
					return;
				}
			} 

			else {
				GenericLog.Log ("WARNING : One stop state in the diagram does not have a diagram id!");
				return;
			}


			XmlElement stopStateElement = (XmlElement)informationInfo;
			XmlNodeList attributesList = stopStateElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);

			foreach (XmlNode stopStateAttributeInfo in attributesList) {
				XmlAttribute stopStateAttributeName = stopStateAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];

				if (stopStateAttributeName != null) {
					if (stopStateAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string currentStopStateValue = GetDIAString (stopStateAttributeInfo);

						int frameValue = -1;

						if (Int32.TryParse(currentStopStateValue, out frameValue)) {
							m_diagramIDToStopStateDescription [stopStateID].stopFrameValue = frameValue ;
						}

						if (GetDIAFontStyle(stopStateAttributeInfo) == GlobalXMLTags.FONT_STYLE_BOLD) {
							m_diagramIDToStopStateDescription [stopStateID].isSolutionState = true;
						} else {
							m_diagramIDToStopStateDescription [stopStateID].isSolutionState = false;
						}
					}

				}
			}

			GenericLog.Log(m_diagramIDToStopStateDescription[stopStateID].ToString());	
		}
		
		public void LoadDisplacements (XmlNode displacementInfo)
		{
			string displacementsID = "";
			XmlAttribute idAttribute = displacementInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				displacementsID = idAttribute.Value;
				
				if (!m_diagramIDToDisplacementDescription.ContainsKey (displacementsID)) {
					m_diagramIDToDisplacementDescription [displacementsID] = new DisplacementDescription ();
				} else {
					GenericLog.Log ("WARNING : One displacement with the same diagram id " + displacementsID + " already exists!");
					return;
				}
			} 

			else {
				GenericLog.Log ("WARNING : One displacement area in the diagram does not have a diagram id!");
				return;
			}

			XmlElement displacementElement = (XmlElement)displacementInfo;
			XmlNodeList attributesList = displacementElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode displacementAttributeInfo in attributesList) {
				XmlAttribute displacementAttributeName = displacementAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (displacementAttributeName != null) {
					if (displacementAttributeName.Value == GlobalXMLTags.BOUNDING_BOX_ATTRIBUTE) {
						BoundingBox currentBB = GetBoundingBox (displacementAttributeInfo);
						
						m_diagramIDToDisplacementDescription[displacementsID].bb = currentBB;
					} 
				}
			}

			GenericLog.Log(m_diagramIDToDisplacementDescription[displacementsID].ToString());

		}
		
		public void LoadSolutions (XmlNode solutionInfo)
		{
			string solutionsID = "";
			XmlAttribute idAttribute = solutionInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				solutionsID = idAttribute.Value;
				
				if (!m_diagramIDToSolutionDescription.ContainsKey (solutionsID)) {
					m_diagramIDToSolutionDescription [solutionsID] = new SolutionDescription ();
				} else {
					GenericLog.Log ("WARNING : One solution with the same diagram id " + solutionsID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING : One solution in the diagram does not have a diagram id!");
				return;
			}
			
			XmlElement solutionElement = (XmlElement)solutionInfo;
			XmlNodeList attributesList = solutionElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode solutionAttributeInfo in attributesList) {
				XmlAttribute solutionAttributeName = solutionAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (solutionAttributeName != null) {
					if (solutionAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string currentExpectedSolution = GetDIAString(solutionAttributeInfo);

						string[] solutions = currentExpectedSolution.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);


						foreach (string solution in solutions) {
							m_diagramIDToSolutionDescription[solutionsID].expectedInputs.Add(solution);
						}

						if (GetDIAFontStyle(solutionAttributeInfo) == GlobalXMLTags.FONT_STYLE_BOLD) {
							m_diagramIDToSolutionDescription [solutionsID].isPassword = true;
						} else {
							m_diagramIDToSolutionDescription [solutionsID].isPassword = false;
						}
					}

				}
			}
			
			GenericLog.Log(m_diagramIDToSolutionDescription[solutionsID].ToString());	
		}

		public void LoadWrongAnswers (XmlNode wrongAnswerInfo)
		{
			string wrongAnswerID = "";
			XmlAttribute idAttribute = wrongAnswerInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				wrongAnswerID = idAttribute.Value;
				
				if (!m_diagramIDToWrongAnswersDescription.ContainsKey (wrongAnswerID)) {
					m_diagramIDToWrongAnswersDescription [wrongAnswerID] = new WrongAnswersDescription ();
				} else {
					GenericLog.Log ("WARNING : One wrong answers with the same diagram id " + wrongAnswerID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING : One wrong answers in the diagram does not have a diagram id!");
				return;
			}
			
			XmlElement wrongAnswerElement = (XmlElement)wrongAnswerInfo;
			XmlNodeList attributesList = wrongAnswerElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode wrongAnswerAttributeInfo in attributesList) {
				XmlAttribute wrongAnswerAttributeName = wrongAnswerAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (wrongAnswerAttributeName != null) {
										
					if (wrongAnswerAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string currentExpectedSolution = GetDIAString(wrongAnswerAttributeInfo);
						
						string[] wrongAnswers = currentExpectedSolution.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
						
						List<string> wrongAnswerAndMatchingHelp = new List<string>();

						foreach (string wrongAnswer in wrongAnswers) {
							wrongAnswerAndMatchingHelp.Add(wrongAnswer);

							if (wrongAnswerAndMatchingHelp.Count == 2) {
								m_diagramIDToWrongAnswersDescription[wrongAnswerID].wrongAnswersToHelp[wrongAnswerAndMatchingHelp[0]] = wrongAnswerAndMatchingHelp[1];
								wrongAnswerAndMatchingHelp.Clear();
							}
						}
					}
					
				}
			}
			
			GenericLog.Log(m_diagramIDToWrongAnswersDescription[wrongAnswerID].ToString());	
		}
		
		public void LoadSounds (XmlNode soundInfo, bool mustLoop)
		{
			string SoundID = "";
			XmlAttribute idAttribute = soundInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				SoundID = idAttribute.Value;
				
				if (!m_diagramIDToSoundDescription.ContainsKey (SoundID)) {
					m_diagramIDToSoundDescription [SoundID] = new SoundDescription ();
					m_diagramIDToSoundDescription [SoundID].mustLoop = mustLoop;
				} else {
					GenericLog.Log ("WARNING: One sound with the same diagram id " + SoundID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING: One sound in the diagram does not have a diagram id!");
				return;
			}


			XmlElement soundElement = (XmlElement)soundInfo;
			XmlNodeList attributesList = soundElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode soundAttributeInfo in attributesList) {
				XmlAttribute soundAttributeName = soundAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (soundAttributeName != null) {
					if (soundAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string currentSoundName = GetDIAString (soundAttributeInfo);
						
						m_diagramIDToSoundDescription [SoundID].soundName = currentSoundName;
					}
					
				}
			}
			
			GenericLog.Log(m_diagramIDToSoundDescription[SoundID].ToString());	
		}

		public void LoadInitialState (XmlNode initialStateInfo)
		{
			string initialStateID = "";
			XmlAttribute idAttribute = initialStateInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				initialStateID = idAttribute.Value;
				/*
				if (m_initialStateID != initialStateID) {
					m_initialStateID = initialStateID;
				} 
				*/

				m_initialStateIDs.Add(initialStateID);
			} 
			
			else {
				GenericLog.Log ("WARNING: The initial state in the diagram does not have a diagram id!");
				return;
			}
			

		}

		public void LoadObjectStateSolutionTransition (XmlNode objectStateTransitionInfo)
		{
			string objectStateSolutionID = "";
			XmlAttribute idAttribute = objectStateTransitionInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];

			if (idAttribute != null) {
				objectStateSolutionID = idAttribute.Value;

				m_ObjectStateSolutionIDs.Add(objectStateSolutionID);
			} 

			else {
				GenericLog.Log ("WARNING: The object state solution in the diagram does not have a diagram id!");
				return;
			}


		}

		public void LoadPuzzleSolutionTransition (XmlNode puzzleTransitionInfo)
		{
			string puzzleSolutionID = "";
			XmlAttribute idAttribute = puzzleTransitionInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];

			if (idAttribute != null) {
				puzzleSolutionID = idAttribute.Value;

				m_puzzleSolutionIDs.Add(puzzleSolutionID);
			} 

			else {
				GenericLog.Log ("WARNING: The puzzle solution in the diagram does not have a diagram id!");
				return;
			}


		}

		public void LoadClickText (XmlNode clickTextInfo, bool isURL) {
			string clickTextID = "";
			XmlAttribute idAttribute = clickTextInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				clickTextID = idAttribute.Value;
				
				if (!m_diagramIDToClickTextDescription.ContainsKey (clickTextID)) {
					m_diagramIDToClickTextDescription [clickTextID] = new ClickTextDescription ();
					m_diagramIDToClickTextDescription [clickTextID].isURL = isURL;
				} else {
					GenericLog.Log ("WARNING : One click text with the same diagram id " + clickTextID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING : One click text in the diagram does not have a diagram id!");
				return;
			}
			
			XmlElement clickTextElement = (XmlElement)clickTextInfo;
			XmlNodeList attributesList = clickTextElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode clickTextAttributeInfo in attributesList) {
				XmlAttribute clickTextAttributeName = clickTextAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (clickTextAttributeName != null) {
					if (clickTextAttributeName.Value == GlobalXMLTags.BOUNDING_BOX_ATTRIBUTE) {
						BoundingBox currentBB = GetBoundingBox (clickTextAttributeInfo);
						
						m_diagramIDToClickTextDescription[clickTextID].absoluteBB = currentBB;
					} else if (clickTextAttributeName.Value == GlobalXMLTags.TEXT_ATTRIBUTE) {
						string clickText = GetDIAString (clickTextAttributeInfo);

						m_diagramIDToClickTextDescription [clickTextID].text = clickText;
					}
				}
			}
			
			GenericLog.Log(m_diagramIDToClickTextDescription[clickTextID].ToString());
		}
		
		
		public BoundingBox GetBoundingBox (XmlNode fatherNode)
		{
			BoundingBox resultBB = new BoundingBox ();
			
			XmlElement fatherElement = (XmlElement)fatherNode;
			XmlNodeList rectanglesList = fatherElement.GetElementsByTagName (GlobalXMLTags.AREA);
			
			foreach (XmlNode areaInfo in rectanglesList) {
				XmlAttribute areaName = areaInfo.Attributes [GlobalXMLTags.VALUE_ATTRIBUTE];
				
				if (areaName != null) {
					string stringBB = areaName.Value;
					
					char[] separators = {',', ';'};
					string[] splittedBBString = stringBB.Split (separators);
					
					if (splittedBBString.Length == 4) {
						resultBB.x1 = float.Parse (splittedBBString [0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
						resultBB.y1 = float.Parse (splittedBBString [1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
						resultBB.x2 = float.Parse (splittedBBString [2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
						resultBB.y2 = float.Parse (splittedBBString [3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
					}
				}
			}
			
			return resultBB;
		}
		
		public string GetFileURL (XmlNode fatherNode) {
			string resultString = "";
			
			XmlElement fatherElement = (XmlElement)fatherNode;
			XmlNodeList stringsList = fatherElement.GetElementsByTagName (GlobalXMLTags.STRING);
			
			foreach (XmlNode stringInfo in stringsList) {
				if (stringInfo.ChildNodes.Count > 0) {
					resultString = stringInfo.ChildNodes[0].Value;
					resultString = resultString.Substring(1, resultString.Length - 2);
				}
			}
			
			return resultString;
			
		}

		// Careful !
		// There is a better way to parse the xml for the DIAString. If bug, it would be great to reconsider this function
		public string GetDIAString (XmlNode fatherNode) {
			string resultString = "";
			XmlNode textChild = fatherNode.FirstChild;
		
			if (textChild != null) {
				XmlNode stringChild = textChild.FirstChild;

				if (stringChild != null) {
					XmlElement stringElement = (XmlElement) stringChild;

					XmlNodeList stringsList = stringElement.GetElementsByTagName (GlobalXMLTags.STRING);
					
					foreach (XmlNode stringInfo in stringsList) {
						if (stringInfo.ChildNodes.Count > 0) {
							resultString = stringInfo.ChildNodes[0].Value;
							resultString = resultString.Substring(1, resultString.Length - 2);
						}
					}
				}
			}

			return resultString;
		}

		public int GetDIAFontStyle (XmlNode fatherNode) {
			int resultFontStyle = -1;
			XmlNode textChild = fatherNode.FirstChild;

			if (textChild != null) {
				XmlNode stringChild = textChild.FirstChild;
				XmlNode fontChild = stringChild.NextSibling;


				if (fontChild != null) {
					XmlNode fontAttributes = fontChild.FirstChild;

					if (fontAttributes != null) {
						string fontStyleValue = fontAttributes.Attributes[GlobalXMLTags.FONT_STYLE].Value;

						Int32.TryParse(fontStyleValue, out resultFontStyle);
					}

				}
					
			}

			return resultFontStyle;
		}
		
		
		public void LoadTransitions (XmlDocument xmlDoc)
		{
			XmlNodeList objectsList = xmlDoc.GetElementsByTagName (GlobalXMLTags.OBJECT);
			
			foreach (XmlNode objectInfo in objectsList) {
				XmlAttribute objectType = objectInfo.Attributes [GlobalXMLTags.TYPE_ATTRIBUTE];
				
				if (objectType != null) {
					if (objectType.Value == GlobalXMLTags.TRANSITION_TYPE) {
						GetTransition(objectInfo);
					}


					
				}
			}
		}


		public void GetTransition (XmlNode transitionInfo) {
			string transitionID = "";
			XmlAttribute idAttribute = transitionInfo.Attributes [GlobalXMLTags.ID_ATTRIBUTE];
			
			if (idAttribute != null) {
				transitionID = idAttribute.Value;
				
				if (!m_diagramIDToTransitionDescription.ContainsKey (transitionID)) {
					m_diagramIDToTransitionDescription [transitionID] = new TransitionDescription ();
				} else {
					GenericLog.Log ("WARNING: One transition with the same diagram id " + transitionID + " already exists!");
					return;
				}
			} 
			
			else {
				GenericLog.Log ("WARNING: One transition in the diagram does not have a diagram id!");
				return;
			}
			
			XmlElement objectElement = (XmlElement) transitionInfo;

			/* Get transition handles */
			XmlNodeList connections = objectElement.GetElementsByTagName (GlobalXMLTags.CONNECTIONS);
			
			foreach (XmlNode connectionsInfo in connections) {
				XmlNodeList connectionList = connectionsInfo.ChildNodes;
				
				if (connectionList.Count == 2) {
					XmlNode firstNode = connectionList[0];
					XmlNode secondNode = connectionList[1];
					
					XmlAttribute firstHandle = firstNode.Attributes[GlobalXMLTags.HANDLE_ATTRIBUTE];
					XmlAttribute secondHandle = secondNode.Attributes[GlobalXMLTags.HANDLE_ATTRIBUTE];
					
					XmlAttribute firstTo = firstNode.Attributes[GlobalXMLTags.TO_ATTRIBUTE];
					XmlAttribute secondTo = secondNode.Attributes[GlobalXMLTags.TO_ATTRIBUTE];
					
					if (firstHandle != null && secondHandle != null && firstTo != null && secondTo != null) { 
						if (firstHandle.Value == "0") {
							m_diagramIDToTransitionDescription [transitionID].fromID = firstTo.Value;
							m_diagramIDToTransitionDescription [transitionID].toID = secondTo.Value;
						} else {
							m_diagramIDToTransitionDescription [transitionID].fromID = secondTo.Value;
							m_diagramIDToTransitionDescription [transitionID].toID = firstTo.Value;
						}
					} else {
						GenericLog.Log("WARNING! The transition with diagram id " + transitionID + " does not have correct handles and/or to information.");
					}
				} else {
					GenericLog.Log("WARNING! The transition with diagram id " + transitionID + " is not connected to 2 objects ! Please check your diagram.");
				}
			}

			/* Get transition type (immediate or not) */
			XmlNodeList attributesList = objectElement.GetElementsByTagName (GlobalXMLTags.ATTRIBUTE);
			
			foreach (XmlNode transitionAttributeInfo in attributesList) {
				XmlAttribute transitionAttributeName = transitionAttributeInfo.Attributes [GlobalXMLTags.NAME_ATTRIBUTE];
				
				if (transitionAttributeName != null) {
					if (transitionAttributeName.Value == GlobalXMLTags.IMMEDIATE_TRANSITION_ATTRIBUTE) {
						XmlNodeList immediateTransitionList = ((XmlElement)transitionAttributeInfo).GetElementsByTagName (GlobalXMLTags.ENUM);

						foreach (XmlNode immediateTransitionInfo in immediateTransitionList) {
							XmlAttribute immediateAttribute = immediateTransitionInfo.Attributes[GlobalXMLTags.VALUE_ATTRIBUTE];

							if (immediateAttribute.Value == GlobalXMLTags.IMMEDIATE_TRANSITION_VALUE) {
								//GenericLog.Log("IMMEDIATE");
								m_diagramIDToTransitionDescription [transitionID].isImmediateTransition = true;
							} else if (immediateAttribute.Value == GlobalXMLTags.REGULAR_TRANSITION_VALUE) {
								//GenericLog.Log("CROSSFADE");
								m_diagramIDToTransitionDescription [transitionID].isImmediateTransition = false;
							}
						}
					} 

					if (transitionAttributeName.Value == GlobalXMLTags.UNIQUE_TRANSITION_ATTRIBUTE) {
						XmlNodeList uniqueTransitionList = ((XmlElement)transitionAttributeInfo).GetElementsByTagName (GlobalXMLTags.ENUM);

						foreach (XmlNode uniqueTransitionInfo in uniqueTransitionList) {
							XmlAttribute uniqueAttribute = uniqueTransitionInfo.Attributes[GlobalXMLTags.VALUE_ATTRIBUTE];

							if (uniqueAttribute.Value == GlobalXMLTags.UNIQUE_TRANSITION_VALUE) {
								GenericLog.Log("UNIQUE " + transitionID);
								m_diagramIDToTransitionDescription [transitionID].isUniqueTransition = true;
							} else {
								//GenericLog.Log("CROSSFADE");
								m_diagramIDToTransitionDescription [transitionID].isUniqueTransition = false;
							}
						}
					} 
				}
			}

			GenericLog.Log(m_diagramIDToTransitionDescription [transitionID].ToString());
		}

		public void InclusionManagement() {
			foreach(KeyValuePair<string, RoomDescription> rooms in m_diagramIDToRoomDescription) {
				// DISPLACEMENT AREA IN ROOMS
				foreach(KeyValuePair<string, DisplacementDescription> displacements in m_diagramIDToDisplacementDescription) {
					if (rooms.Value.bb.PartiallyIncludes(displacements.Value.bb)) {
						displacements.Value.roomFromID = rooms.Key;
					}
				}

				//SOLUTIONS IN ROOMS
				/*
				foreach(KeyValuePair<string, SolutionDescription> solutions in m_diagramIDToSolutionDescription) {
					if (rooms.Value.bb.PartiallyIncludes(solutions.Value.bb)) {
						solutions.Value.includingRoomId = rooms.Key;
						GenericLog.Log(solutions.Value.ToString());
					}
				}
				*/

				//ITEMS IN ROOMS
				foreach(KeyValuePair<string, ItemDescription> items in m_diagramIDToItemDescription) {
					if (rooms.Value.bb.PartiallyIncludes(items.Value.absoluteBB)) {
						items.Value.includingRoomID = rooms.Key;
						items.Value.relativeBB = items.Value.absoluteBB.GetRelativeBB(rooms.Value.bb);

						rooms.Value.includedItems.Add(items.Key);
						//GenericLog.Log(solutions.Value.ToString());
					}
				}

				//CLICK TEXT IN ROOMS
				foreach(KeyValuePair<string, ClickTextDescription> clickText in m_diagramIDToClickTextDescription) {
					if (rooms.Value.bb.PartiallyIncludes(clickText.Value.absoluteBB)) {
						clickText.Value.includingRoomID = rooms.Key;
						clickText.Value.relativeBB = clickText.Value.absoluteBB.GetRelativeBB(rooms.Value.bb);
						
						rooms.Value.includedClickText.Add(clickText.Key);
						//GenericLog.Log(solutions.Value.ToString());
					}
				}
			}
		}

		public void LinkManagement () {

			foreach (KeyValuePair<string, TransitionDescription> transitionDescriptions in m_diagramIDToTransitionDescription) {
				string currentTransitionID = transitionDescriptions.Key;
				TransitionDescription currentTransitionDescription = transitionDescriptions.Value;

				// FROM DESCRIPTION TO ROOM
				if (m_diagramIDToInformationDescription.ContainsKey(currentTransitionDescription.fromID) 
				    && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {

					m_diagramIDToRoomDescription[currentTransitionDescription.toID].description = m_diagramIDToInformationDescription[currentTransitionDescription.fromID].information;
					m_diagramIDToRoomDescription[currentTransitionDescription.toID].descriptionMustBeRepeated = m_diagramIDToInformationDescription[currentTransitionDescription.fromID].mustRemain;
 				}

				// FROM SOUND TO ROOM
				else if (m_diagramIDToSoundDescription.ContainsKey(currentTransitionDescription.fromID) 
				    && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToRoomDescription[currentTransitionDescription.toID].backgroundMusicName = m_diagramIDToSoundDescription[currentTransitionDescription.fromID].soundName;
					m_diagramIDToRoomDescription[currentTransitionDescription.toID].backgroundMusicMustLoop = m_diagramIDToSoundDescription[currentTransitionDescription.fromID].mustLoop;
				}

				// FROM DIARY ITEM TO ROOM
				if (m_diagramIDToDiaryDescription.ContainsKey(currentTransitionDescription.fromID) 
					&& m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {

					m_diagramIDToRoomDescription[currentTransitionDescription.toID].diaryItem = m_diagramIDToDiaryDescription[currentTransitionDescription.fromID].diaryItemName;
					m_diagramIDToRoomDescription[currentTransitionDescription.toID].newDiaryItemMustBeHighlighted = m_diagramIDToDiaryDescription[currentTransitionDescription.fromID].mustBeHighlighted;
				}

				// FROM DISPLACEMENT AREA TO ROOM
				else if (m_diagramIDToDisplacementDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {

					m_diagramIDToDisplacementDescription[currentTransitionDescription.fromID].roomToID = currentTransitionDescription.toID;

					string roomFromID = m_diagramIDToDisplacementDescription[currentTransitionDescription.fromID].roomFromID;
					string roomToID = m_diagramIDToDisplacementDescription[currentTransitionDescription.fromID].roomToID;

					if (m_diagramIDToRoomDescription.ContainsKey(roomFromID)
					    && m_diagramIDToRoomDescription.ContainsKey(roomToID)) {

						//BoundingBox relativeBB = new BoundingBox();

						BoundingBox roomBB = m_diagramIDToRoomDescription[roomFromID].bb;
						BoundingBox areaBB = m_diagramIDToDisplacementDescription[currentTransitionDescription.fromID].bb;

						currentTransitionDescription.outgoingBoudingBox = areaBB.GetRelativeBB(roomBB);

						string areaID = currentTransitionDescription.fromID;

						currentTransitionDescription.fromID = roomFromID;
						currentTransitionDescription.isBackTransition = false;

						m_diagramIDToDisplacementDescription.Remove (areaID);
				
						GenericLog.Log ("COUNT " + m_diagramIDToDisplacementDescription.Count);
					}
				}

				// FROM SOUND TO TRANSITION
				else if (m_diagramIDToSoundDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToTransitionDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].soundNames.Add(m_diagramIDToSoundDescription[currentTransitionDescription.fromID].soundName);
				}

				// FROM ROOM TO ROOM
				else if (m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					//TODO : fonctionne en étant vide ? A vérifier sérieusement !

					/*currentTransitionDescription.fromID = currentTransitionDescription.fromID;
					currentTransitionDescription.toID = currentTransitionDescription.toID;*/
				}

				// FROM SOLUTION TO TRANSITION
				else if (m_diagramIDToSolutionDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToTransitionDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].solutions = m_diagramIDToSolutionDescription[currentTransitionDescription.fromID].expectedInputs;
					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].isPassword = m_diagramIDToSolutionDescription[currentTransitionDescription.fromID].isPassword;
				}

				// FROM DESCRIPTION (QUESTION) TO TRANSITION
				if (m_diagramIDToInformationDescription.ContainsKey(currentTransitionDescription.fromID) 
				    && m_diagramIDToTransitionDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].question = m_diagramIDToInformationDescription[currentTransitionDescription.fromID].information;
				}

				// FROM WRONG ANSWER TO TRANSITION
				if (m_diagramIDToWrongAnswersDescription.ContainsKey(currentTransitionDescription.fromID) 
				    && m_diagramIDToTransitionDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].wrongAnswersToHelp = m_diagramIDToWrongAnswersDescription[currentTransitionDescription.fromID].wrongAnswersToHelp;
				}

				// FROM OBJECT STATE SOLUTION TYPE TO TRANSITION
				if (m_ObjectStateSolutionIDs.Contains(currentTransitionDescription.fromID) 
					&& m_diagramIDToTransitionDescription.ContainsKey(currentTransitionDescription.toID)) {

					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].isAObjectStateTransition = true;
				}

				// FROM PUZZLE SOLUTION TYPE TO TRANSITION
				if (m_puzzleSolutionIDs.Contains(currentTransitionDescription.fromID) 
					&& m_diagramIDToTransitionDescription.ContainsKey(currentTransitionDescription.toID)) {

					m_diagramIDToTransitionDescription[currentTransitionDescription.toID].isAPuzzleTransition = true;
				}

				/*
				// FROM SOLUTION TO ROOM
				else if (m_diagramIDToSolutionDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					currentTransitionDescription.solutions = m_diagramIDToSolutionDescription[currentTransitionDescription.fromID].expectedInputs;
					currentTransitionDescription.fromID = m_diagramIDToSolutionDescription[currentTransitionDescription.fromID].includingRoomId;
				}
				*/
			

				// FROM DESCRIPTION TO ITEM
				else if (m_diagramIDToInformationDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToItemDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToItemDescription[currentTransitionDescription.toID].description = m_diagramIDToInformationDescription[currentTransitionDescription.fromID].information;
				}

				// FROM STOP STATE TO ITEM
				else if (m_diagramIDToStopStateDescription.ContainsKey(currentTransitionDescription.fromID) 
					&& m_diagramIDToItemDescription.ContainsKey(currentTransitionDescription.toID)) {

					m_diagramIDToItemDescription[currentTransitionDescription.toID].stopFrames.Add(m_diagramIDToStopStateDescription[currentTransitionDescription.fromID].stopFrameValue);

					if (m_diagramIDToStopStateDescription[currentTransitionDescription.fromID].isSolutionState) {
						m_diagramIDToItemDescription[currentTransitionDescription.toID].solutionFrames.Add(m_diagramIDToStopStateDescription[currentTransitionDescription.fromID].stopFrameValue);
					}
				}

				// FROM SOUND TO ITEM
				else if (m_diagramIDToSoundDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToItemDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToItemDescription[currentTransitionDescription.toID].soundName = m_diagramIDToSoundDescription[currentTransitionDescription.fromID].soundName;
				}

				// FROM SOUND TO CLICK AREA
				else if (m_diagramIDToSoundDescription.ContainsKey(currentTransitionDescription.fromID) 
				         && m_diagramIDToClickTextDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_diagramIDToClickTextDescription[currentTransitionDescription.toID].soundName = m_diagramIDToSoundDescription[currentTransitionDescription.fromID].soundName;
				}

				/*
				// FROM INITIAL STATE TO ROOM
				else if ((m_initialStateID == currentTransitionDescription.fromID) 
				         && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_inititalRoomID = currentTransitionDescription.toID;
				}
				*/

				// FROM INITIAL STATE TO ROOM
				else if ((m_initialStateIDs.Contains(currentTransitionDescription.fromID)) 
				         && m_diagramIDToRoomDescription.ContainsKey(currentTransitionDescription.toID)) {
					
					m_inititalRoomID = currentTransitionDescription.toID;
				}
			}

			// Computes the remaining displacements as BACK transition
			int backTransitionID = 0;
			foreach (KeyValuePair<string, DisplacementDescription> currentDisplacement in m_diagramIDToDisplacementDescription) {
				TransitionDescription currentTransitionDescription = new TransitionDescription();
				m_diagramIDToTransitionDescription ["BACK" + backTransitionID] = currentTransitionDescription;
				backTransitionID++;

				string roomFromID = currentDisplacement.Value.roomFromID;

				if (m_diagramIDToRoomDescription.ContainsKey(roomFromID)) {

				//	BoundingBox relativeBB = new BoundingBox();

					BoundingBox roomBB = m_diagramIDToRoomDescription[roomFromID].bb;
					BoundingBox areaBB = currentDisplacement.Value.bb;

					currentTransitionDescription.outgoingBoudingBox = areaBB.GetRelativeBB(roomBB);

					currentTransitionDescription.fromID = roomFromID;
					currentTransitionDescription.isBackTransition = true;

				}
			}

		}

	


		public void DisplayAllRooms() {
			GenericLog.Log("***** ALL ROOMS *****");

			foreach(KeyValuePair<string, RoomDescription> rooms in m_diagramIDToRoomDescription) {
				GenericLog.Log(rooms.Value.ToString());
			}
		}

		public void DisplayAllTransitions() {
			GenericLog.Log("***** ALL TRANSITIONS *****");
			
			foreach(KeyValuePair<string, TransitionDescription> transitions in m_diagramIDToTransitionDescription) {
				GenericLog.Log(transitions.Value.ToString());
			}
		}

		public void DisplayAllItems() {
			GenericLog.Log("***** ALL ITEMS *****");
			
			foreach(KeyValuePair<string, ItemDescription> items in m_diagramIDToItemDescription) {
				GenericLog.Log(items.Value.ToString());
			}
		}

		public void DisplayAllClickText() {
			GenericLog.Log("***** ALL CLICK TEXT *****");
			
			foreach(KeyValuePair<string, ClickTextDescription> clickText in m_diagramIDToClickTextDescription) {
				GenericLog.Log(clickText.Value.ToString());
			}
		}



		public void TranslateDiagrammeElementsToSEGMentEngineElements(Player playerToSet) {
			Dictionary<string, int> roomsFromDiagramIDToSEGMentID = new Dictionary<string, int>();

			GameStructureRooms createdGameStructureRooms = new GameStructureRooms(playerToSet.GetInformationManager());

			// Instantiation of SEGMent rooms
			foreach(KeyValuePair<string, RoomDescription> rooms in m_diagramIDToRoomDescription) {
				int currentRoomID = createdGameStructureRooms.CreateRoom();
				createdGameStructureRooms.SetRoomBackgroundImageURL(currentRoomID, rooms.Value.backgroundImageURL);
				createdGameStructureRooms.SetRoomBackgroundMusic(currentRoomID, rooms.Value.backgroundMusicName, rooms.Value.backgroundMusicMustLoop);
				createdGameStructureRooms.SetRoomDescription(currentRoomID, rooms.Value.description, rooms.Value.descriptionMustBeRepeated);

				createdGameStructureRooms.SetRoomDiaryEntry(currentRoomID, rooms.Value.diaryItem, rooms.Value.newDiaryItemMustBeHighlighted);

				roomsFromDiagramIDToSEGMentID[rooms.Key] = currentRoomID;

				foreach(string itemDiagramID in rooms.Value.includedItems) {
					ItemDescription currentItemToAdd = m_diagramIDToItemDescription[itemDiagramID];

					int itemSEGMentID = createdGameStructureRooms.CreateItem(currentRoomID, currentItemToAdd.relativeBB);

					if (itemSEGMentID != GameStructureRooms.CREATION_ERROR) {
						createdGameStructureRooms.SetItemBackgroundImageURL(itemSEGMentID, currentItemToAdd.imageURL);

						if (currentItemToAdd.soundName != "") {
							createdGameStructureRooms.SetItemSoundName(itemSEGMentID, currentItemToAdd.soundName);
						}

						if (currentItemToAdd.description != "") {
							createdGameStructureRooms.SetItemDescription(itemSEGMentID, currentItemToAdd.description);
						}

						if (currentItemToAdd.isPuzzlePiece) {
							createdGameStructureRooms.SetItemIsPuzzle(itemSEGMentID, true);
						}

						foreach (int frameID in currentItemToAdd.stopFrames) {
							createdGameStructureRooms.AddItemStopState(itemSEGMentID, frameID);
						}

						foreach (int frameID in currentItemToAdd.solutionFrames) {
							createdGameStructureRooms.AddItemSolutionState(itemSEGMentID, frameID);
						}
					}
				}

				foreach(string clickTextID in rooms.Value.includedClickText) {
					ClickTextDescription currentClickTextToAdd = m_diagramIDToClickTextDescription[clickTextID];
					
					int clickTextSEGMentID = createdGameStructureRooms.CreateClickText(currentRoomID, currentClickTextToAdd.relativeBB);
					
					if (clickTextSEGMentID != GameStructureRooms.CREATION_ERROR) {
						if (currentClickTextToAdd.soundName != "") {
							createdGameStructureRooms.SetClickTextSoundName(clickTextSEGMentID, currentClickTextToAdd.soundName);
						}
						
						//if (currentClickTextToAdd.text != "") {
						createdGameStructureRooms.SetClickTextText(clickTextSEGMentID, currentClickTextToAdd.text, currentClickTextToAdd.isURL);
						//}
					}
				}
			}

			// Instantiation of SEGMent transitions between rooms
			foreach(KeyValuePair<string, TransitionDescription> transitions in m_diagramIDToTransitionDescription) {

				if (transitions.Value.isBackTransition) {

					if (m_diagramIDToRoomDescription.ContainsKey(transitions.Value.fromID)) {
						createdGameStructureRooms.CreateBackTransition(roomsFromDiagramIDToSEGMentID[transitions.Value.fromID], 
							transitions.Value.outgoingBoudingBox,
							transitions.Value.isImmediateTransition);
					}
				} 
				// Only consider the transitions between rooms
				else if (m_diagramIDToRoomDescription.ContainsKey(transitions.Value.fromID) 
				    && m_diagramIDToRoomDescription.ContainsKey(transitions.Value.toID)) {

					int currentTransitionID = -1;

					if (transitions.Value.isAObjectStateTransition) {
						currentTransitionID = createdGameStructureRooms.CreateStateObjectSolutionTransition(roomsFromDiagramIDToSEGMentID[transitions.Value.fromID], 
							roomsFromDiagramIDToSEGMentID[transitions.Value.toID],
							transitions.Value.isImmediateTransition,
							transitions.Value.isUniqueTransition);
					} else if (transitions.Value.isAPuzzleTransition) {
						currentTransitionID = createdGameStructureRooms.CreatePuzzleSolutionTransition(roomsFromDiagramIDToSEGMentID[transitions.Value.fromID], 
							roomsFromDiagramIDToSEGMentID[transitions.Value.toID],
							transitions.Value.isImmediateTransition,
							transitions.Value.isUniqueTransition);
					}

					else if (transitions.Value.solutions.Count == 0) { 
						currentTransitionID = createdGameStructureRooms.CreateClickableTransition(roomsFromDiagramIDToSEGMentID[transitions.Value.fromID], 
						                                                                          roomsFromDiagramIDToSEGMentID[transitions.Value.toID],
						                                                                          transitions.Value.outgoingBoudingBox,
						                                                                          transitions.Value.isImmediateTransition, 
																								  transitions.Value.isUniqueTransition);
						
					} else {
						currentTransitionID = createdGameStructureRooms.CreateSolutionTransition(roomsFromDiagramIDToSEGMentID[transitions.Value.fromID], 
						                                                                         roomsFromDiagramIDToSEGMentID[transitions.Value.toID],
						                                                                         transitions.Value.question,
						                                                                         transitions.Value.solutions,
															                                     transitions.Value.wrongAnswersToHelp,
																								 transitions.Value.isPassword,    
																								 transitions.Value.isImmediateTransition,
																								 transitions.Value.isUniqueTransition);
						
					}

					if (currentTransitionID != -1) {
						foreach (string currentSoundName in transitions.Value.soundNames)
						{
							createdGameStructureRooms.SetTransitionSound(currentTransitionID, currentSoundName);
						}
					}
				}


			}

			createdGameStructureRooms.TeleportToRoom(roomsFromDiagramIDToSEGMentID[m_inititalRoomID]);

			playerToSet.SetMap(createdGameStructureRooms);
		}




	}

}

