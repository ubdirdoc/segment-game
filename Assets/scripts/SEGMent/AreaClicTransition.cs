/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
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
