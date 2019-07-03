/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;


[System.Serializable]
public class ClicPositionEvent : UnityEvent<GameObject, Vector3> { }


public class DetectClicOnSprite : MonoBehaviour {
	public ClicPositionEvent m_functionToSendTheClicInformationTo;
	public bool m_mustChangeCursor = true;

	bool m_isMouseOver = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount >= 2) {
			return;	
		}

		// Prevents the clic to be taken into account when the user is actually interacting with the GUI
		/*if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}*/

		if (MyIsPointerOverGameObject.IsPointerOverGameObject()) {
			return;
		}

		//if(Input.GetMouseButtonUp(0)){

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		Collider2D currentCollider = gameObject.GetComponent<Collider2D> ();

		if (currentCollider != null) {
			if (currentCollider.OverlapPoint (mousePosition)) {
				if (Input.GetMouseButtonDown (0)) {
					ComputeAndSendClic (Input.mousePosition);
				}
				m_isMouseOver = true;

				if (m_mustChangeCursor) {
					Cursor.SetCursor (MouseCursorGlobal.instance.GetInteractableTexture (), MouseCursorGlobal.instance.GetTopMiddleHotSpot (), CursorMode.Auto);
				}
			} else {
				if (m_isMouseOver) {
					if (m_mustChangeCursor) {
						Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
					}
					m_isMouseOver = false;
				}
			}

		}


		
	}

	void OnMouseDown() {
		//ComputeAndSendClic(Input.mousePosition);



		// Command to print a vector with a 4-decimal precision.
		//print (mousePositionRelativeToScreen.ToString("F4"));
		//print (Camera.main.ScreenToWorldPoint(Input.mousePosition));

		// The "UPDATE" solution, with if(currentCollider.OverlapPoint(mousePosition)), works far better (in detecting click) than the "OnMouseDown" solution.
		// I am not sure why, but this is why I don't use OnMouseDown.
	}

	public void ComputeAndSendClic(Vector3 mousePosition) {
		SpriteRenderer currentSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		
		if (currentSpriteRenderer != null) {
			float scale = gameObject.transform.localScale.x;
			float cameraHeight = Camera.main.orthographicSize*2f;
			float cameraWidth = cameraHeight/Screen.height*Screen.width;
			
			float spriteWidth = currentSpriteRenderer.sprite.bounds.size.x * scale;
			float spriteHeight = currentSpriteRenderer.sprite.bounds.size.y * scale;
			
			Vector3 mousePositionRelativeToCenter = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector3 mousePositionRelativeToScreenTopLeftCorner = new Vector3(cameraWidth/2.0f + mousePositionRelativeToCenter.x, cameraHeight/2.0f - mousePositionRelativeToCenter.y, mousePositionRelativeToCenter.z);
			
			Vector3 mousePositionRelativeToTheSprite = new Vector3(mousePositionRelativeToScreenTopLeftCorner.x - (cameraWidth - spriteWidth)/2.0f,
			                                                       mousePositionRelativeToScreenTopLeftCorner.y - (cameraHeight - spriteHeight)/2.0f,
			                                                       mousePositionRelativeToScreenTopLeftCorner.z);
			
			Vector3 normMousePositionRelativeToTheSprite = new Vector3(mousePositionRelativeToTheSprite.x / spriteWidth,
			                                                           mousePositionRelativeToTheSprite.y / spriteHeight,
			                                                           mousePositionRelativeToTheSprite.z);
			
			if (m_functionToSendTheClicInformationTo != null) {
				m_functionToSendTheClicInformationTo.Invoke(this.gameObject, normMousePositionRelativeToTheSprite);
			}
			
		}
	}

	void OnDestroy() {
		if (m_mustChangeCursor) {
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		}
	}
}
