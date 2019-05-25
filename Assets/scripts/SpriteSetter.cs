/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public struct GifSprite {
	public Sprite sprite;
	public float delaySec;
	public int orderInLayer;
}
	

public class SpriteSetter : MonoBehaviour {
	private bool m_spriteIsLoaded = false;
	private bool m_gifIsLoaded = false;
	List<GifSprite> m_gifSpriteList;
	// we use this bool for skipping to next sprite. Default is = false, and on click will pass on true 

	public bool toNextSprite = false;
	//isMultipleStateObject was used in an ancient version of this script. Kept it just in case
	//public bool isMultipleStateObject = false;
	public List<bool> m_isSpriteStopList;
	public bool clickByClick = false;

	private int m_currentGIFFrameIndex = 0;

	private bool m_mustResetCoroutine = false;

	private bool m_preventClic = false;

	private string m_url = "";

	private UIManager m_UIManager = null;

	private bool m_isReadyToBeClicked = false;

	void Awake() {
		m_gifSpriteList = new List<GifSprite>();
		m_isSpriteStopList = new List<bool> ();
	}

	public void SetUIManager(UIManager manager) {
		m_UIManager = manager;
	}

	public int GetCurrentSpriteFrameNum() {
		return m_currentGIFFrameIndex + 1;
	}

	public void setFrameIndex(int frameIndex) {
		m_currentGIFFrameIndex = frameIndex - 1;
	}

	public bool IsSpriteLoaded() {
		return m_spriteIsLoaded;
	}

	public void PreventClic() {
		m_preventClic = true;
	}

	public void SetSprite(Sprite sprite, int orderInLayer = 0) {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		if (spriteRenderer != null) {
			spriteRenderer.sprite = sprite;
			spriteRenderer.sortingOrder = orderInLayer;
		}

		Destroy(GetComponent<PolygonCollider2D>());
		gameObject.AddComponent<PolygonCollider2D>();
	}

	public bool IsAGifFile(string url) {
		return (string.Compare(url.Substring(url.Length - 3, 3), "gif", CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0);
	}

	public IEnumerator SetSpriteByURLRelativelyToAnotherOneCoroutine(string url, SpriteRenderer parentSpriteRenderer, float relativex1, float relativey1, float relativex2, float relativey2, int orderInLayer = 0, UIManager manager = null) {
		//GenericLog.Log(url.Substring(url.Length - 4, 3));

		m_spriteIsLoaded = false;

		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		url = url.Replace("\\", "/");
		#endif

		m_url = url;

		/* test if the file is a gif */
		//if (url.Substring(url.Length - 3, 3) == "gif") {
		if(IsAGifFile(url)) {

			
			GenericLog.Log("THIS IS A GIF " + url);
			StartCoroutine(GifLoader(url, parentSpriteRenderer, relativex1, relativey1, relativex2, relativey2, orderInLayer));

			while (!m_gifIsLoaded) {
				yield return null;
			}

			m_spriteIsLoaded = true;
		} else {
			FileToSprite currentFileToSprite = gameObject.AddComponent<FileToSprite>();
			currentFileToSprite.CreateSpriteFromFile(url);

			while (currentFileToSprite.GetSprite() == null) {
				yield return null;
			}

			Sprite sprite = currentFileToSprite.GetSprite();

			SetSpriteRelativelyToAnotherOne(sprite, parentSpriteRenderer, relativex1, relativey1, relativex2, relativey2, orderInLayer);
			m_spriteIsLoaded = true;

			Destroy(currentFileToSprite);

			yield return null;
		}


	}

	public void SetSpriteRelativelyToAnotherOne(Sprite sprite, SpriteRenderer parentSpriteRenderer, float relativex1, float relativey1, float relativex2, float relativey2, int orderInLayer = 0) {
		if (parentSpriteRenderer == null) {
			return;
		}

		PositionAndScale transformInScene = PositionOnSprite.RelativePosOnSpriteToUnityCoordinates(parentSpriteRenderer,
		                                                                                           sprite,
		                                                                                           relativex1,
		                                                                                           relativey1,
		                                                                                           relativex2,
		                                                                                           relativey2);

		transform.localScale = transformInScene.scale;
		transform.localPosition = transformInScene.position;

		SetSprite(sprite, orderInLayer);
	}

	IEnumerator GifLoader(string url, SpriteRenderer parentSpriteRenderer, float relativex1, float relativey1, float relativex2, float relativey2, int orderInLayer = 0) {
		StartCoroutine(GifSetterCoroutine(url, orderInLayer));

		while (!m_gifIsLoaded) {
			yield return null;
		}

		if (m_gifSpriteList.Count > 0) {
			SetSpriteRelativelyToAnotherOne(m_gifSpriteList[0].sprite, parentSpriteRenderer, relativex1, relativey1, relativex2, relativey2, orderInLayer);

		}

		yield return null;
		//gifTextureList = ;
	}

	public void StartGifLoop() {
		if (m_gifIsLoaded) {
			StartCoroutine(GifLoopCoroutine(m_gifSpriteList));
		}
	}

	IEnumerator GifSetterCoroutine (string url, int orderInLayer = 0)
	{
		m_gifIsLoaded = false;

		m_gifSpriteList.Clear ();

		if (GifPreLoader.instance.MustPreload ()) {
			List<GifSprite> currentGifSprite = GifPreLoader.instance.GetSpriteList (url);
			if (currentGifSprite != null) {
				m_gifSpriteList = currentGifSprite;
			}
		}

		if (m_gifSpriteList.Count == 0) {

			List<UniGif.GifTexture> gifTextureList = new List<UniGif.GifTexture> ();

			int loop;
			int width;
			int height;

            Debug.Log("Gif URL: " + url);
#if UNITY_WEBGL
			WWW www = new WWW(url);
#else
			WWW www = new WWW (/*"file:///" +*/ url);
#endif

			while (!www.isDone) {
				yield return null;
			}

			if (www.error != null) {
				GenericLog.Log (www.error);
			}

			gifTextureList = UniGif.GetTextureList (www.bytes, out loop, out width, out height, FilterMode.Bilinear);

			//List<GifSprite> gifSpriteList = new List<GifSprite>();

			foreach (UniGif.GifTexture gifTexture in gifTextureList) {
				GifSprite currentGifSprite = new GifSprite ();

				Rect rect = new Rect ();
				rect.center = new Vector2 (0, 0);
				rect.height = height;
				rect.width = width;

				currentGifSprite.delaySec = gifTexture.delaySec;
				currentGifSprite.sprite = Sprite.Create (gifTexture.texture2d, rect, new Vector2 (0.5f, 0.5f), 1.0f);
				currentGifSprite.orderInLayer = orderInLayer;

				m_gifSpriteList.Add (currentGifSprite);

				yield return null;
			}

			if (GifPreLoader.instance.MustPreload()) {
				GifPreLoader.instance.SetNewGifSpriteList(url, m_gifSpriteList);
			}
		}

		m_isSpriteStopList.Clear();

		for (int i = 0; i < m_gifSpriteList.Count; ++i) {
			m_isSpriteStopList.Add (false);
		}

	//	UnableClickByClick();

		m_gifIsLoaded = true;
	}


	public void UnableClickByClick ()
	{
		for (int j = 0; j < (m_isSpriteStopList.Count); j++) {
			m_isSpriteStopList [j] = true;	
		}
	}

	public void SetStopFrame(int frame)
	{
		if (frame >= 1 && frame <= m_isSpriteStopList.Count) {
			m_isSpriteStopList [frame - 1] = true;
		}
	}


	IEnumerator GifLoopCoroutine (List<GifSprite> gifSpriteList)
	{
		while (true) {
			m_mustResetCoroutine = false;
			m_isReadyToBeClicked = false;


			//for (int i = 0; i < (m_gifSpriteList.Count); i++) {
			m_currentGIFFrameIndex = m_currentGIFFrameIndex % m_gifSpriteList.Count;

			if (m_isSpriteStopList [m_currentGIFFrameIndex] == false) {
				SetSprite (m_gifSpriteList [m_currentGIFFrameIndex].sprite, m_gifSpriteList [m_currentGIFFrameIndex].orderInLayer);
				float delayedTime = Time.time + m_gifSpriteList [m_currentGIFFrameIndex].delaySec;
				while (delayedTime > Time.time && !m_mustResetCoroutine) {
						
					toNextSprite = false;
					//GenericLog.Log("RUN");
					yield return null;
				}
			} else if (m_isSpriteStopList [m_currentGIFFrameIndex] == true) {
				toNextSprite = false;
				SetSprite (m_gifSpriteList [m_currentGIFFrameIndex].sprite, m_gifSpriteList [m_currentGIFFrameIndex].orderInLayer);
				/*	float delayedTime = Time.time + m_gifSpriteList [i].delaySec;*/
				m_isReadyToBeClicked = true;

				while (toNextSprite == false && !m_mustResetCoroutine) {
					yield return null; 
				}
			
				if (!m_mustResetCoroutine) {
					PlaySpriteSound ();
				}						
			} 

			if (!m_mustResetCoroutine) {
				m_currentGIFFrameIndex++;
			} 


			yield return null;

			//}

		}
		//toNextSprite = false; 
	}

	void PlaySpriteSound() {
		AudioSource currentItemAudioSource = gameObject.GetComponent<AudioSource>();

		if (currentItemAudioSource != null) {
			AudioClip currentItemAudioClip = currentItemAudioSource.clip;

			if (currentItemAudioClip != null) {
				AudioManager.instance.PlaySound(currentItemAudioClip);
			}
		}
	}


			/*	//foreach (GifSprite currentGifSprite in gifSpriteList) {
			for (int i = 0; i < (m_gifSpriteList.Count); i++) {
				if (isMultipleStateObject) {
					
					SetSprite (m_gifSpriteList [i].sprite, m_gifSpriteList [i].orderInLayer);
					float delayedTime = Time.time + m_gifSpriteList [i].delaySec;
					
					
					
					while (toNextSprite == false) {
						
						yield return null;
					}
				} else {
					
					while (toNextSprite == true  )
					{
						//for (int i = 0; i < (m_gifSpriteList.Count); i++)
						{
							//foreach()
						}
					}
					SetSprite (m_gifSpriteList [i].sprite, m_gifSpriteList [i].orderInLayer);
					float delayedTime = Time.time + m_gifSpriteList [i].delaySec;
					
					while (delayedTime > Time.time) {
						
						toNextSprite = false;
						//GenericLog.Log("RUN");
						yield return null;
					}
				}
			}
			toNextSprite = false;
			
		}*/

	public void ReleaseSprite ()
	{
		if (GifPreLoader.instance.MustPreload ()) {
			if (GifPreLoader.instance.GetSpriteList (m_url) != null) {
				return;
			}
		}
		foreach (GifSprite currentGifSprite in m_gifSpriteList) {
			Destroy (currentGifSprite.sprite.texture);
		}

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();

		if (spriteRenderer != null) {
			Destroy (spriteRenderer.sprite.texture);
		}
		
	}

	/*public void SetStateObject(bool changeToStatedObject)
	{
		isMultipleStateObject = changeToStatedObject;
	}
*/

/*	public void DefineAsStateObject ()
	{
		isMultipleStateObject = true;

	}

	public void DefineAsRegularGif()
	{
		isMultipleStateObject = false;
	}
*/
	void OnMouseDown ()
	{
		// Prevent click when the GUI is blocking
		/*if (EventSystem.current.IsPointerOverGameObject())
	{
		return;
	}*/

		if (MyIsPointerOverGameObject.IsPointerOverGameObject ()) {
			return;
		}

		if (m_UIManager != null) {
			if (m_UIManager.IsRoomDialogOnScreen ()) {
				return;
			}
		}

		if (!m_preventClic) {
			toNextSprite = true; 
		}


	}

	void OnMouseOver ()
	{
		if (MyIsPointerOverGameObject.IsPointerOverGameObject ()) {
			return;
		}

		if (!m_preventClic && m_isReadyToBeClicked) {
		Cursor.SetCursor (MouseCursorGlobal.instance.GetInteractableTexture (), MouseCursorGlobal.instance.GetTopMiddleHotSpot (), CursorMode.Auto);
		}
	}

	void OnMouseExit ()
	{
		
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		
	}
	





}
