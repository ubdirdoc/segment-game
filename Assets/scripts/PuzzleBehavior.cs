/* Authors : Raphaël Marczak - Arthur Desmesure - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class PuzzleBehavior : MonoBehaviour {

	public static float m_z = 0f;
	
	public Bounds m_foregroundBounds;//
	public bool m_isDragDropEnable = false;//
	public Vector3 m_foregroundMinBounds;//
	public Vector3 m_foregroundMaxBounds;//

	Vector3 m_puzzlePieceAbsoluteMinBounds = Vector3.zero;
	Vector3 m_puzzlePieceAbsoluteMaxBounds = Vector3.zero;

	UIManager m_UIManager = null;

	//.......................................................................................................................................................................................................

	public void SetUIManager(UIManager manager) {
		m_UIManager = manager;
	}


	// la fonction qui permet de reconnaitre si un objet est une pièce de puzzle ou non.
	public void EnableDragDrop(bool dragEnable) {
		if (dragEnable) {
			SpriteRenderer currentSpriteRenderer = this.GetComponent<SpriteRenderer>();

			if (currentSpriteRenderer != null) {
				currentSpriteRenderer.sortingOrder = 0;	
			}

			m_z -= 0.001f;
			this.transform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y, m_z);

			ComputeAbsolutePuzzlePieceBounds();
		}
		m_isDragDropEnable = dragEnable;	
	}

	public float GetForegroundHeight() {
		return m_foregroundMaxBounds.y - m_foregroundMinBounds.y;
	}

	public float GetForegroundWidth() {
		return m_foregroundMaxBounds.x - m_foregroundMinBounds.x;
	}

	public void RandomizePos() {
		Vector3 randomPos = Vector3.zero;

		int nbOfTest = 20;

		float marginPercent = 0.08f;

		float puzzleHeight = m_puzzlePieceAbsoluteMaxBounds.y - m_puzzlePieceAbsoluteMinBounds.y;
		float puzzleWidth = m_puzzlePieceAbsoluteMaxBounds.x - m_puzzlePieceAbsoluteMinBounds.x;

		float foregroundHeight = GetForegroundHeight();
		float foregroundWidth = GetForegroundWidth();

		//Debug.Log(puzzleHeight);
		//Debug.Log(puzzleWidth);

		while (nbOfTest > 0) {
			randomPos.x = Random.Range(m_foregroundMinBounds.x + puzzleWidth/2 + foregroundWidth * marginPercent, m_foregroundMaxBounds.x - puzzleWidth/2 - foregroundWidth * marginPercent);
			randomPos.y = Random.Range(m_foregroundMinBounds.y + puzzleHeight/2 + foregroundHeight * marginPercent, m_foregroundMaxBounds.y - puzzleHeight/2 - foregroundHeight * marginPercent);
			randomPos.z = 0;

			if (MovePuzzlePiece(randomPos)) {
				nbOfTest = 0;
			} /*else {
				Debug.Log("TRY AGAIN");
			}*/

			nbOfTest--;
		}
			

	}

	private bool MovePuzzlePiece (Vector3 newPos)
	{
		if (m_isDragDropEnable == true) {
			

			// on définit un vecteur qui va simplement calculer la position ou va atterir la pièce ...
			Vector3 translationVector = newPos - this.transform.position;
			// ... puis on vérifie si cette position est contenue dans notre foreground. Par ailleurs, on additionne les Bounds de la pièce, pour vérifier qu'elle ne sorte pas. 
			if (m_puzzlePieceAbsoluteMinBounds.x + translationVector.x > m_foregroundMinBounds.x && m_puzzlePieceAbsoluteMinBounds.y + translationVector.y > m_foregroundMinBounds.y) {
				if (m_puzzlePieceAbsoluteMaxBounds.x + translationVector.x < m_foregroundMaxBounds.x && m_puzzlePieceAbsoluteMaxBounds.y + translationVector.y < m_foregroundMaxBounds.y) {
					// enfin, si toutes les conditions sont respectées, alors on convertis le nouvel GOcenter de la pièce avec les nouveaux X et Y de la position de notre pièce.

					this.transform.position = new Vector3 (newPos.x, newPos.y, this.transform.position.z);
					ComputeAbsolutePuzzlePieceBounds();

					return true;
				}

			}

		}
		return false;
	}

	private void ComputeAbsolutePuzzlePieceBounds() {
		Bounds puzzlePieceBounds;
		Vector3 puzzleLocalScale;

		GameObject currentPuzzlePiece = this.gameObject;

		puzzleLocalScale = currentPuzzlePiece.transform.lossyScale;

	
		Vector3 currentPuzzlePiecePosition = currentPuzzlePiece.transform.position;

		float currentPuzzlePieceTransX = currentPuzzlePiece.transform.position.x;
		float currentPuzzlePieceTransY = currentPuzzlePiece.transform.position.y;
					
		SpriteRenderer currentPuzzleSpriteRenderer = currentPuzzlePiece.GetComponent<SpriteRenderer> ();



		if (currentPuzzleSpriteRenderer) {
			Sprite currentPuzzleSprite = currentPuzzleSpriteRenderer.sprite;

			puzzlePieceBounds = currentPuzzleSprite.bounds;

			m_puzzlePieceAbsoluteMaxBounds = puzzlePieceBounds.max;
			m_puzzlePieceAbsoluteMinBounds = puzzlePieceBounds.min;

			m_puzzlePieceAbsoluteMinBounds.x = m_puzzlePieceAbsoluteMinBounds.x * puzzleLocalScale.x;
			m_puzzlePieceAbsoluteMinBounds.y = m_puzzlePieceAbsoluteMinBounds.y * puzzleLocalScale.y;
			m_puzzlePieceAbsoluteMaxBounds.x = m_puzzlePieceAbsoluteMaxBounds.x * puzzleLocalScale.x;
			m_puzzlePieceAbsoluteMaxBounds.y = m_puzzlePieceAbsoluteMaxBounds.y * puzzleLocalScale.y;

			m_puzzlePieceAbsoluteMinBounds.x = m_puzzlePieceAbsoluteMinBounds.x + currentPuzzlePieceTransX;
			m_puzzlePieceAbsoluteMinBounds.y = m_puzzlePieceAbsoluteMinBounds.y + currentPuzzlePieceTransY;
			m_puzzlePieceAbsoluteMaxBounds.x = m_puzzlePieceAbsoluteMaxBounds.x + currentPuzzlePieceTransX;
			m_puzzlePieceAbsoluteMaxBounds.y = m_puzzlePieceAbsoluteMaxBounds.y + currentPuzzlePieceTransY;

		}
	}

	public bool IsDragDropEnabled() {
		return m_isDragDropEnable;
	}

	void Awake(){

		Vector3 localScale;
		Vector3 localPosition;

		// On récupère le foreground grace à son tag
		GameObject currentForeground = GameObject.FindGameObjectWithTag ("Foreground");
		// Si il est bien présent, alors : 
		if (currentForeground) {

			// je récupère le scale du foreground
			localScale = currentForeground.transform.localScale;
			// je récupère le transform.position
			localPosition = currentForeground.transform.position;
			//la je récupère le spriterenderer en lui meme
			SpriteRenderer currentSpriteRenderer = currentForeground.GetComponent<SpriteRenderer> ();
			if (currentSpriteRenderer)
			{
				//la je récupère le sprite
				Sprite currentSprite = currentSpriteRenderer.sprite;

				if (currentSprite != null) {
					// maintenant je récupère les bounds du sprite
					m_foregroundBounds = currentSprite.bounds;
					// a noter que le Bounds se décompose en 2 Vector3, un "center" et un "size". Il faut appeller l'un ou l'autre pour les modifier. 
					// remise à l'échelle avec localScale
					m_foregroundMinBounds.x = m_foregroundBounds.min.x * localScale.x;
					m_foregroundMinBounds.y = m_foregroundBounds.min.y * localScale.y;
					m_foregroundMaxBounds.x = m_foregroundBounds.max.x * localScale.x;
					m_foregroundMaxBounds.y = m_foregroundBounds.max.y * localScale.y;

					// initialisé dans le start, pour pouvoir recalculer en live, il faudra le mettre dans l'update, mais c'est relou !
					m_foregroundMinBounds.x = m_foregroundMinBounds.x + localPosition.x;
					m_foregroundMinBounds.y = m_foregroundMinBounds.y + localPosition.y;
					m_foregroundMaxBounds.x = m_foregroundMaxBounds.x + localPosition.x;
					m_foregroundMaxBounds.y = m_foregroundMaxBounds.y + localPosition.y;


					//	ComputeAbsolutePuzzlePieceBounds();
				}
			
			}
		
		}
	}



	Vector3 m_goCenter;
	Vector3 m_touchPosition;
	Vector3 m_offset;
	//Vector3 newGoCenter;

	//lorsque que l'on clique : 
	void OnMouseDown() {

		if (!m_isDragDropEnable) {
			return;
		}
		// Prevent click when the GUI is blocking
		/*if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}*/

		if (MyIsPointerOverGameObject.IsPointerOverGameObject()) {
			return;
		}

		if (m_UIManager != null) {
			if (m_UIManager.IsRoomDialogOnScreen()) {
				return;
			}
		}

		// on récupère la position du curseur et on l'assigne à l'objet sur lequel on clique
		m_goCenter = this.transform.position;
		//on prends la position de la souris
		m_touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// on calcule le "offset" qui est notre point de départ
		m_offset = m_touchPosition - m_goCenter;

		m_z -= 0.001f;
		this.transform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y, m_z);

	}


	// fonction pendant que on est en train de "drag"
	void OnMouseDrag ()
	{

		// Prevent click when the GUI is blocking
		/*if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}*/

		if (MyIsPointerOverGameObject.IsPointerOverGameObject()) {
			return;
		}

		if (m_UIManager != null) {
			if (m_UIManager.IsRoomDialogOnScreen()) {
				return;
			}
		}

	

		m_touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);


		MovePuzzlePiece (m_touchPosition - m_offset);

	}

	void OnMouseOver() {
		if (m_isDragDropEnable) {
			Cursor.SetCursor(MouseCursorGlobal.instance.GetInteractableTexture(), MouseCursorGlobal.instance.GetTopMiddleHotSpot() , CursorMode.Auto);
		}
	}

	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}




	void Update()
	{



	}
}