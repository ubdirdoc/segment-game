/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
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
