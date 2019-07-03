/* Author : Aymeric Labbé - 2016-2018 / DEPRECATED
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

/*using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(GUITexture))]

public class AudioWave : MonoBehaviour
{
    public int width, height;
    public float x = 0;
    public float z = 0;
    public int size = 1;
    public float fps;
    public float ratio, ratioCursor;
    public float cpt;
    public float length_audio;
    public int sample_length;
    public Color backgroundColor = Color.black;
    public Color waveColor = Color.blue;
    public Color cursorColor = Color.white;
    public Color clicColor = Color.cyan;
    public Color transparent;
    public Vector3 clicPosition;

    private Color[] image_array;
    private Texture2D texture;
    private Texture2D cursor_texture;
    private Texture2D clic_texture;
    private float[] sample;

	private Texture2D saved_transparent_texture;

    // Use this for initialization
    void Start()
    {
        AudioSource my_audio = GetComponent<AudioSource>();

        width = Screen.width;
        height = Screen.height;
        sample = new float[my_audio.clip.samples * my_audio.clip.channels];
        texture = new Texture2D(width, height);
        GetComponent<GUITexture>().texture = texture;
        image_array = new Color[width * height];

        cursor_texture = new Texture2D(width, height);
        GetComponent<GUITexture>().texture = cursor_texture;
		saved_transparent_texture = new Texture2D(width, height);

        clic_texture = new Texture2D(width, height);
        GetComponent<GUITexture>().texture = clic_texture;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                clic_texture.SetPixel(i, j, transparent);
            }
        }

        length_audio = GetComponent<AudioSource>().clip.length;
        ratioCursor = Screen.width / length_audio;

        clicPosition = transform.position;

        transparent = new Color(1, 1, 1, 0);

        for (int i = 0; i < image_array.Length; i++)
        {
            image_array[i] = backgroundColor;
        }
        
        texture.SetPixels(image_array, 0);
        DrawWaveSound(my_audio);

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				cursor_texture.SetPixel(i, j, transparent);
			}
		}
        
    }

    public void DrawWaveSound(AudioSource audio)
    {
        sample_length = sample.Length;

        ratio = sample_length / width;

        audio.clip.GetData(sample, 0);

        for (int i = 0; i < width; i++)
        {
            cpt = 0;

            for (int j = (int)x; j < ratio + x; j++)
            {
                cpt += sample[j] * 10000;
            }

            cpt /= ratio;

            if (cpt + Screen.height / 2 >= Screen.height / 2)
            {
                for (int k = (int)cpt + Screen.height / 2; k >= (Screen.height - (cpt + Screen.height / 2)); k--)
                {
                    texture.SetPixel(i, k, waveColor);
                }
            }
            else
            {
                texture.SetPixel(i, (int)cpt + Screen.height / 2, waveColor);
            }

            x += ratio;
        }
    }

    void DrawCross (Texture2D texture, Vector3 position, Color color)
    {
        texture.SetPixel((int)position.x, (int)position.y, color);
        texture.SetPixel((int)position.x + 1, (int)position.y + 1, color);
        texture.SetPixel((int)position.x + 1, (int)position.y - 1, color);
        texture.SetPixel((int)position.x + 2, (int)position.y + 2, color);
        texture.SetPixel((int)position.x + 2, (int)position.y - 2, color);
        texture.SetPixel((int)position.x + 3, (int)position.y + 3, color);
        texture.SetPixel((int)position.x + 3, (int)position.y - 3, color);
        texture.SetPixel((int)position.x - 1, (int)position.y + 1, color);
        texture.SetPixel((int)position.x - 1, (int)position.y - 1, color);
        texture.SetPixel((int)position.x - 2, (int)position.y + 2, color);
        texture.SetPixel((int)position.x - 2, (int)position.y - 2, color);
        texture.SetPixel((int)position.x - 3, (int)position.y + 3, color);
        texture.SetPixel((int)position.x - 3, (int)position.y - 3, color);
    }

    // Update is called once per frame
    void Update()
    {
        fps = 1.0f / Time.deltaTime;

	//	cursor_texture = new Texture2D(saved_transparent_texture);

		


        if (GetComponent<AudioSource>().isPlaying)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cursor_texture.SetPixel((int)(width * i + z), j, cursorColor);
					cursor_texture.SetPixel((int)(width * i + (z-1)), j , transparent);
                }
                if (!(z >= width))
                    z += ratioCursor / fps;
            }
        }

        texture.Apply();
        cursor_texture.Apply();

        if (Input.GetMouseButton(0))
        {
            clicPosition = Input.mousePosition;

            DrawCross(clic_texture, clicPosition, clicColor);                
        }
        if (Input.GetMouseButton(1))
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    clic_texture.SetPixel(i, j, transparent);
                }
            }
        }

        clic_texture.Apply();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cursor_texture);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), clic_texture);
    }
}*/