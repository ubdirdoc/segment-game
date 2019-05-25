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
	public class ClickText: GraphNode
	{
		private string m_soundName = "";
		private string m_text = "";
		private bool m_isURL = false;
		
		private BoundingBox m_position;
		
		private Room m_containingRoom;
		
		
		public ClickText (Room containingRoom, BoundingBox relativeSizeAndPosition): base()
		{
			SetNodeType(NODE_TYPE.CLICK_TEXT_TYPE);
			
			m_position = relativeSizeAndPosition;
			containingRoom.AddClickText(this);
			
			m_containingRoom = containingRoom;
		}
		
		public void SetSoundName(string soundName) {
			m_soundName = soundName;
		}
		
		public string GetSoundName() {
			return m_soundName;
		}
		
		public void SetText(string text) {
			m_text = text;
		}

		public string GetText() {
			return m_text;
		}
		
		public BoundingBox GetRelativePosition() {
			return m_position;
		}

		public void SetIsURL(bool isURL) {
			m_isURL = isURL;
		}

		public bool IsURL() {
			return m_isURL;
		}

		public Room GetContainingRoom() {
			return m_containingRoom;
		}
		
	}
}
