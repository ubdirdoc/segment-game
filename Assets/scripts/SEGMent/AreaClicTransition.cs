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
	public class AreaClicTransition : GraphTransition {
		private BoundingBox m_relativeBB;

		public AreaClicTransition(GraphNode from, GraphNode to, BoundingBox relativeBox, bool isImmediate, bool isUnique = false):base (from, to, isImmediate, isUnique)
		{
			if (to != null) {
				SetTransitionType (TRANSITION_TYPE.CLIC_AREA_TYPE);
			} else {
				SetTransitionType (TRANSITION_TYPE.BACK_TRANSITION);
			}

			m_relativeBB = relativeBox;
		}

		public BoundingBox GetRelativeBoundingBox() {
			return m_relativeBB;
		}
		
	}
}
