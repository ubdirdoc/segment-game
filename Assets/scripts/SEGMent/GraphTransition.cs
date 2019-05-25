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
	public enum TRANSITION_TYPE {NO_TYPE, CLIC_AREA_TYPE, SOLUTION_TYPE, OBJECT_STATE_SOLUTION_TYPE, PUZZLE_SOLUTION_TYPE, BACK_TRANSITION};

	public class GraphTransition
	{
		private int m_transitionID = -1;
		private TRANSITION_TYPE m_transitionType = TRANSITION_TYPE.NO_TYPE;

		private GraphNode m_nodeFrom = null;
		private GraphNode m_nodeTo = null;

		private List<string> m_transitionSounds; 

		private bool m_isImmediate = false;
		private bool m_isUnique = false;

		private bool m_hasBeenFiredOnce = false;


		public GraphTransition (GraphNode from, GraphNode to, bool isImmediate = false, bool isUnique = false)
		{
			m_nodeFrom = from;
			m_nodeTo = to;

			m_nodeFrom.AddOutgoingTransition(this);

			if (m_nodeTo != null) {
				m_nodeTo.AddIngoingTransition(this);
			}

			m_transitionSounds = new List<string>();

			m_isImmediate = isImmediate;
			m_isUnique = isUnique;
		}

		public void SetTransitionType(TRANSITION_TYPE transitionType) {
			m_transitionType = transitionType;
		}

		public TRANSITION_TYPE GetTransitionType() {
			return m_transitionType;
		}

		public void SetTransitionID(int id) {
			m_transitionID = id;
		}
		
		public int GetTransitionID() {
			return m_transitionID;
		}

		public bool IsActive() {
			return true;
		}

		public void AddTransitionSound(string soundName) {
			m_transitionSounds.Add(soundName);
		}

		public List<string> GetTransitionSound() {
			return m_transitionSounds;
		}

		public GraphNode GetNodeFrom() {
			return m_nodeFrom;
		}

		public GraphNode GetNodeTo() {
			return m_nodeTo;
		}

		public bool IsImmediate() {
			return m_isImmediate;
		}

		public bool IsUnique() {
			return m_isUnique;
		}

		public void TransitionIsFired() {
			m_hasBeenFiredOnce = true;
		}

		public bool ShouldBeAutomaticallyFired() {
			return (m_isUnique && m_hasBeenFiredOnce);
		}
	}
}

