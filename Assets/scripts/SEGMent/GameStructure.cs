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
	public class GameStructure
	{
		protected GraphNode m_token = null;

		protected List<GraphTransition> m_transitions;

		protected InformationManager m_informationManager;

		protected List<Room> m_roomsPile;
		
		public GameStructure (InformationManager informationManager)
		{
			m_transitions = new List<GraphTransition>();
			m_roomsPile = new List<Room> ();
			m_informationManager = informationManager;
		}

		public void SetTransitionSound(int transitionID, string soundName) {
			if (transitionID >= m_transitions.Count) {
				return;
			}
			
			m_transitions[transitionID].AddTransitionSound(soundName);
		}

		public void FireTransition(GraphTransition transitionToFire) {
			transitionToFire.TransitionIsFired();

		
			if (transitionToFire.GetNodeFrom() == m_token) {
				m_token = transitionToFire.GetNodeTo();

				if (m_token.GetNodeType() == NODE_TYPE.ROOM_TYPE) {
					Room currentRoom = (Room) m_token;

					List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions();
                    
					foreach (GraphTransition currentTransition in outGoingTransitions) {
						if (currentTransition.ShouldBeAutomaticallyFired()) {
							//	GenericLog.Log("heyaaaaaaa2");
							FireTransition(currentTransition);
							return;

						}
				}



				foreach (string soundName in transitionToFire.GetTransitionSound()) {
					m_informationManager.AddTransitionSoundsToPlay(soundName);
				}
			}


		}
		}
		
	}
}

