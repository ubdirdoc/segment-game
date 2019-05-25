/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class PositionAndScale {
	public Vector3 position = Vector3.zero;
	public Vector3 scale = Vector3.zero;
}

public class PositionOnSprite : MonoBehaviour {

	public static PositionAndScale RelativePosOnSpriteToUnityCoordinates(SpriteRenderer containingSpriteRenderer, Sprite includedSprite, float relativeX1, float relativeY1, float relativeX2, float relativeY2) 
	{
		PositionAndScale result = new PositionAndScale();

		Vector3 spriteBottomLeftCorner = containingSpriteRenderer.sprite.bounds.min;
		Vector3 spriteTopRightCorner = containingSpriteRenderer.sprite.bounds.max;

		float relativeCenterX = (relativeX1 + relativeX2)/2.0f;
		float relativeCenterY = (relativeY1 + relativeY2)/2.0f;


		result.position.x = (spriteBottomLeftCorner.x + ((spriteTopRightCorner.x - spriteBottomLeftCorner.x) * relativeCenterX)) * containingSpriteRenderer.gameObject.transform.localScale.x;
		result.position.y = (spriteTopRightCorner.y + ((spriteBottomLeftCorner.y - spriteTopRightCorner.y) * relativeCenterY)) * containingSpriteRenderer.gameObject.transform.localScale.y;

		float containingSpriteWidth = (spriteTopRightCorner.x - spriteBottomLeftCorner.x) * containingSpriteRenderer.gameObject.transform.localScale.x;
		float containingSpriteHeigh = (spriteTopRightCorner.y - spriteBottomLeftCorner.y) * containingSpriteRenderer.gameObject.transform.localScale.y;

		float relativeWidth = (relativeX2 - relativeX1) * containingSpriteWidth;
		float relativeHeigh = (relativeY2 - relativeY1) * containingSpriteHeigh;

		result.scale.x = relativeWidth / includedSprite.bounds.size.x;
		result.scale.y = relativeHeigh / includedSprite.bounds.size.y;


	//	print (containingSpriteWidth + " ///// " + containingSpriteHeigh);

		return result;
	}


}
