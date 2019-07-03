/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent {
	public class ObjectStateTransition : GraphTransition {

		public ObjectStateTransition(GraphNode from, GraphNode to, bool isImmediate, bool isUnique):base (from, to, isImmediate, isUnique)
		{
			if (to == null) {
				GenericLog.Log("Problem creating the object state transition ('to' not specified");
			}
			SetTransitionType (TRANSITION_TYPE.OBJECT_STATE_SOLUTION_TYPE);

		}

	}
}
