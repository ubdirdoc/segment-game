/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

public class FileToSprite : MonoBehaviour {
	private Sprite m_sprite = null;
	private string m_fileName;

	//private Texture2D m_texture = null;


	private byte[] m_data = null;

	private bool m_isLoadingSprite = false;

	private bool m_hasLoadingFailed = false;

	public bool IsSpriteLoading () {
		return m_isLoadingSprite;
	}




	public void CreateSpriteFromFile(string fileName) {
		m_sprite = null;

		m_fileName = fileName;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (m_fileName.StartsWith("file:///"))
            m_fileName = m_fileName.Remove(0, 8);
#endif
        Debug.Log(m_fileName);

        StartCoroutine(CreateSpriteFromFileCoroutine());

	}

	void GenerateSprite() {
		if (m_data == null) {
			return;
		}

		Texture2D texture = new Texture2D(2048, 2048, TextureFormat.ARGB32, true);
		texture.filterMode = FilterMode.Trilinear;
		texture.anisoLevel = 1;

		texture.LoadImage(m_data);
		texture.name = Path.GetFileNameWithoutExtension(m_fileName);

		Rect rect = new Rect();
		rect.center = new Vector2(0,0);
		rect.height = texture.height;
		rect.width = texture.width;

		Sprite createdSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 1.0f);
		createdSprite.name = m_fileName;

		m_sprite = createdSprite;
	}

	IEnumerator CreateSpriteFromFileCoroutine() {
		m_isLoadingSprite = true;
        /*#if !UNITY_WEBGL
                if (System.IO.File.Exists(m_fileName)) {
                    m_data = File.ReadAllBytes(m_fileName);
                }

                WWW imageWWW = new WWW(m_fileName);
                while (!imageWWW.isDone) {
                    //GenericLog.Log("YIELD IMAGE");
                    yield return null;
                }

                m_data = imageWWW.bytes;




#else*/
        WWW imageWWW = new WWW(m_fileName);
		while (!imageWWW.isDone) {
			//GenericLog.Log("YIELD IMAGE");
			yield return null;
		}

		if (imageWWW.error != null) {
			m_hasLoadingFailed = true;
		}

		m_data = imageWWW.bytes;


		/*xmlDoc.LoadXml(xmlwww.text);

		string url = m_fileName;
		WebClient client = new WebClient();
		client.DownloadDataCompleted += DownloadDataCompleted;
		client.DownloadDataAsync(new System.Uri(url));*/

//#endif

		GenerateSprite();
		m_isLoadingSprite = false;
		yield return null;

	} 

		
	public Sprite GetSprite() {
		return m_sprite;
	}

	public bool HasLoadingFailed() {
		return m_hasLoadingFailed;
	}

	/*public void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
	{
		if (e.Error == null)
		{
			m_data = e.Result;
			m_isReadyToGenerate = true;
		}
	}*/
}
