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

	public class BoundingBox
	{
		public float x1 = 0.0f;
		public float y1 = 0.0f; 
		public float x2 = 0.0f;
		public float y2 = 0.0f;

        public static BoundingBox FromRect(float x, float y, float w, float h)
        {
            return new BoundingBox {
                x1 = x,
                y1 = y,
                x2 = x + w,
                y2 = y + h
            };
        }

        public static BoundingBox FromRect(float[] pos, float[] size)
        {
            return new BoundingBox
            {
                x1 = pos[0],
                y1 = pos[1],
                x2 = pos[0] + size[0],
                y2 = pos[1] + size[1]
            };
        }
        public override string ToString() {
			return ("[(" + x1 + ";" + y1 + ") (" + x2 + ";" + y2 + ")]");
		}
		
		public bool PartiallyIncludes(BoundingBox other) {
			bool isFistPointIncluded = (((other.x1) >= x1) && ((other.x1) <= x2)
			                            && ((other.y1) >= y1) && ((other.y1) <= y2));
			
			bool isSecondPointIncluded = (((other.x2) >= x1) && ((other.x2) <= x2)
			                              && ((other.y2) >= y1) && ((other.y2) <= y2));
			
			return (isFistPointIncluded || isSecondPointIncluded);
		}
		
		public bool TotallyIncludes(BoundingBox other) {
			bool isFistPointIncluded = (((other.x1) >= x1) && ((other.x1) <= x2)
			                            && ((other.y1) >= y1) && ((other.y1) <= y2));
			
			bool isSecondPointIncluded = (((other.x2) >= x1) && ((other.x2) <= x2)
			                              && ((other.y2) >= y1) && ((other.y2) <= y2));
			
			return (isFistPointIncluded && isSecondPointIncluded);
		}

		public BoundingBox GetRelativeBB(BoundingBox relativeFrom) {
			BoundingBox relativeBB = new BoundingBox();
			
			relativeBB.x1 = (this.x1 - relativeFrom.x1) / (relativeFrom.x2 - relativeFrom.x1);
			relativeBB.y1 = (this.y1 - relativeFrom.y1) / (relativeFrom.y2 - relativeFrom.y1);
			relativeBB.x2 = (this.x2 - relativeFrom.x1) / (relativeFrom.x2 - relativeFrom.x1);
			relativeBB.y2 = (this.y2 - relativeFrom.y1) / (relativeFrom.y2 - relativeFrom.y1);
			
			return relativeBB;
		} 
	}

}