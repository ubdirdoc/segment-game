/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SEGMent
{
	public class Room: GraphNode
	{
		private string m_backgroundImageURL = "";
		private string m_backgroundSoundName = "";
		private string m_roomDescription = "";

		private bool m_descriptionMustBeRepeated = false;

		private bool m_backgroundMusicMustLoop = true;

		private string m_diaryEntryName = "";
		private bool m_diaryEntryMustBeHighlighted;

		private List<Item> m_includedItems;
		private List<ClickText> m_includedClickText;


		public Room (): base()
		{
			SetNodeType(NODE_TYPE.ROOM_TYPE);

			m_includedItems = new List<Item>();
			m_includedClickText = new List<ClickText>();
		}

		public bool IsAnAnswerRoom() 
		{
			foreach (GraphTransition transition in m_outgoingTransitions) {
				if ((transition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) && (transition.IsActive())) {
					return true;
				}
			}

			return false;
		}

		public string GetQuestion() 
		{
			foreach (GraphTransition transition in m_outgoingTransitions) {
				if ((transition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) && (transition.IsActive())) {
					return ((SolutionTransition) transition).GetQuestion();
				}
			}
			
			return "";
		}

		public bool GetIsSolutionPasswordType() {
			foreach (GraphTransition transition in m_outgoingTransitions) {
				if ((transition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) && (transition.IsActive())) {
					return ((SolutionTransition) transition).IsPassword();
				}
			}

			return false;
		}

		public void SetBackgroundImageURL (string url) {
			m_backgroundImageURL = url;
		}

		public string GetBackgroundImageURL() {
			return m_backgroundImageURL;
		}

		public void SetBackgroundSoundName(string soundName, bool mustLoop) {
			m_backgroundSoundName = soundName;
			m_backgroundMusicMustLoop = mustLoop;
		}

		public string GetBackgroundMusicName() {
			return m_backgroundSoundName;
		}

		public void SetDescription(string description, bool mustBeRepeated = false) {
			m_roomDescription = description;
			m_descriptionMustBeRepeated = mustBeRepeated;
		}
		
		public string GetDescription() {
			return m_roomDescription;
		}

		public string PopDescription() {
			string result = m_roomDescription;

			if (!m_descriptionMustBeRepeated) {
				m_roomDescription = "";
			}

			return result;
		}

		public void SetDiaryEntry(string diaryName, bool mustBeHighlighted = false) {
			m_diaryEntryName = diaryName;
			m_diaryEntryMustBeHighlighted = mustBeHighlighted;
		}

		public string PopDiaryEntry() {
			string result = m_diaryEntryName;

			m_diaryEntryName = "";

			return result;
		}

		public bool ShouldDiaryEntryBeHighlighted() {
			return m_diaryEntryMustBeHighlighted;
		}

		public void AddItem(Item itemToAdd) {
			m_includedItems.Add(itemToAdd);
		}

		public bool MusicMustLoop() {
			return m_backgroundMusicMustLoop;
		}

		public List<Item> GetIncludedItems() {
			return m_includedItems;
		}

		public void AddClickText(ClickText clickTextToAdd) {
			m_includedClickText.Add(clickTextToAdd);
		}
		
		public List<ClickText> GetIncludedClickText() {
			return m_includedClickText;
		}



	}
}
