/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent
{
	public enum NODE_TYPE {NO_TYPE, ROOM_TYPE, ITEM_TYPE, CLICK_TEXT_TYPE};

	public class GraphNode
	{
		private int m_nodeID = -1;

		public NODE_TYPE m_nodeType = NODE_TYPE.NO_TYPE;

		public List<GraphTransition> m_outgoingTransitions;
		public List<GraphTransition> m_ingoingTransitions;

		public GraphNode ()
		{
			m_outgoingTransitions = new List<GraphTransition>();
			m_ingoingTransitions = new List<GraphTransition>();
		}

		public void SetNodeID(int id) {
			m_nodeID = id;
		}
		
		public int GetNodeID() {
			return m_nodeID;
		}

		public void SetNodeType(NODE_TYPE nodeType) {
			m_nodeType = nodeType;
		}

		public NODE_TYPE GetNodeType() {
			return m_nodeType;
		}

		public void AddOutgoingTransition(GraphTransition outTransition) {
			m_outgoingTransitions.Add(outTransition);
		}

		public void AddIngoingTransition(GraphTransition inTransition) {
			m_ingoingTransitions.Add(inTransition);
		}

		public List<GraphTransition> GetOutgoingTransitions() {
			return m_outgoingTransitions;
		}
	}
}

