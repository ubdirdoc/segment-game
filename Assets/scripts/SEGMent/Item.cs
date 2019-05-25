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
	public class Item: GraphNode
	{
		private bool m_isPuzzlePiece = false;

		private List<int> m_stopFrames = new List<int>();
		private List<int> m_solutionFrames = new List<int>();

		private int m_startFrame = 1;

		private string m_imageURL = "";
		private string m_soundName = "";
		private string m_objectDescription = "";

		private BoundingBox m_position;

		private Room m_containingRoom;
		
		
		public Item (Room containingRoom, BoundingBox relativeSizeAndPosition): base()
		{
			SetNodeType(NODE_TYPE.ITEM_TYPE);

			m_position = relativeSizeAndPosition;
			containingRoom.AddItem(this);

			m_containingRoom = containingRoom;
		}
		
		public void SetImageURL (string url) {
			m_imageURL = url;
		}
		
		public string GetImageURL() {
			return m_imageURL;
		}
		
		public void SetSoundName(string soundName) {
			m_soundName = soundName;
		}
		
		public string GetSoundName() {
			return m_soundName;
		}

		public void SetDescription(string description) {
			m_objectDescription = description;
		}
		
		public string GetDescription() {
			return m_objectDescription;
		}

		public BoundingBox GetRelativePosition() {
			return m_position;
		}

		public void SetPuzzlePiece(bool isPuzzlePiece) {
			m_isPuzzlePiece = isPuzzlePiece;
		}

		public bool IsPuzzlePiece() {
			return m_isPuzzlePiece;
		}

		public Room GetContainingRoom() {
			return m_containingRoom;
		}

		public void AddStopFrame(int frame) {
			m_stopFrames.Add(frame);
		}

		public List<int> GetStopFrames() {
			return m_stopFrames;
		}

		public void AddSolutionFrame(int frame) {
			m_solutionFrames.Add(frame);
		}

		public List<int> GetSolutionFrames() {
			return m_solutionFrames;
		}

		public void SetStartFrame(int frameIndex) {
			if (frameIndex >= 1) {
				m_startFrame = frameIndex;
			}
		}

		public int GetStartFrame() {
			return m_startFrame;
		}
		
	}
}
