/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepSpeed : MonoBehaviour
{
    public string ambiance_txt = "Il fait froid ici ...  Attention ! Quelque chose a bougé par là !! Je vais aller jeter un oeil. Ne bougez pas ... Là !! Il est parti par ici ! Courez !";
    public int size_zone, length_txt, index_of_exclamation, index_of_last_point, rectPosW, rectPosH, rectW, rectH;
    public int x = 0, index_of_space = 0, delay_turn = 0;
    public string slow_string, show_txt, part_txt;
    public string txt;
    public float delay;
    public float delay_exclamation;
    public float delay_point;
    public float[] delay_tab;
    public Rect my_rect;
    public bool end_txt;
    public bool start_coroutine = false, is_over = false, stop_coroutine = false, txt_full = false;
    public bool debut = true;
    public AudioClip my_audio_clip;
    public Font my_font;
    public int my_font_size;
    public float size_max_display;
    public float ratio;

    public void SetDelayExclamation(float add_delay_exclamation)
    {
        delay_exclamation = add_delay_exclamation;
    }

    public void SetDelayPoint (float add_delay_point)
    {
        delay_point = add_delay_point;
    }

    public void SetDelay(float add_delay)
    {
        delay = add_delay;
    }

    public float GetRatio (int fontSize)
    {
        return ratio = (13 - (fontSize - 13)) * 0.007F / 13; 
    }

    public int GetFontSize (Font font)
    {
        return my_font_size = my_font.fontSize;
    }

    public void SetTxt (string txt_use)
    {
        txt = txt_use;
    }

    public void SetRect(int posW, int posH, int w, int h)
    {
        rectPosW = posW;
        rectPosH = posH;
        rectW = w;
        rectH = h;
        my_rect = new Rect(rectPosW, rectPosH, rectW, rectH);
    }

    public void SetTabDelay(string str)
    {
        length_txt = str.Length;

        delay_tab = new float[length_txt];

        for (int i = 0; i < length_txt; i++)
        {
            delay_tab[i] = delay;
        }

        part_txt = str;
        index_of_exclamation = part_txt.LastIndexOf('!');

        while (index_of_exclamation != -1)
        {
            part_txt = part_txt.Substring(0, index_of_exclamation);
            index_of_last_point = part_txt.LastIndexOf('.');

            for (int i = index_of_last_point + 1; i < index_of_exclamation + 1; i++)
            {
                delay_tab[i] = delay_exclamation;
            }

            part_txt = part_txt.Substring(0, index_of_last_point);
            index_of_exclamation = part_txt.LastIndexOf('!');
        }

        part_txt = str;
        index_of_last_point = part_txt.LastIndexOf('.');

        while (index_of_last_point != -1)
        {
            delay_tab[index_of_last_point] += delay_point;
            part_txt = part_txt.Substring(0, index_of_last_point);
            index_of_last_point = part_txt.LastIndexOf('.');
        }

        part_txt = str;
        index_of_exclamation = part_txt.LastIndexOf('!');

        while (index_of_exclamation != -1)
        {
            delay_tab[index_of_exclamation] += delay_point;
            part_txt = part_txt.Substring(0, index_of_exclamation);
            index_of_exclamation = part_txt.LastIndexOf('!');
        }
    }

    public void SubTxt(string str, Rect rect)
    {
        length_txt = str.Length;
        size_zone = (int)(rect.width * rect.height);
        size_max_display = size_zone * ratio;
        List<string> my_list = new List<string>();
        my_list.Add(str);
        if (length_txt <= size_max_display)
        {
            txt_full = true;
            foreach (string string_in_list in my_list)
            {
                show_txt = string_in_list;
            }
        }
        else {
            if (!end_txt)
            {
                foreach (string string_in_list in my_list)
                {
                    show_txt = string_in_list.Substring(x, (int)size_max_display);
                    index_of_space = show_txt.LastIndexOf(' ');
                    show_txt = string_in_list.Substring(x, index_of_space);
                }
            }
            if (end_txt)
            {
                int last_char = length_txt - x;
                foreach (string string_in_list in my_list)
                {
                    show_txt = string_in_list.Substring(x, last_char);
                }
            }
        }
    }

    IEnumerator OnceAtTime(string str, float[] tabDelay)
    {
        if (txt_full)
        {
            int i = 0;
            slow_string = "";
            start_coroutine = true;
            while (i < str.Length && stop_coroutine == false)
            {
                slow_string += str[i];
                AudioSource.PlayClipAtPoint(my_audio_clip, transform.position);
                yield return new WaitForSeconds(tabDelay[i]);
                i++;
            }
            if (stop_coroutine)
            {
                while (i < str.Length)
                {
                    slow_string += str[i++];
                }
            }
            stop_coroutine = false;
            start_coroutine = false;
        }
        else
        {
            int i = 0;
            slow_string = "";
            start_coroutine = true;
            while (i < str.Length && stop_coroutine == false)
            {
                slow_string += str[i];
                AudioSource.PlayClipAtPoint(my_audio_clip, transform.position);
                yield return new WaitForSeconds(tabDelay[i + delay_turn]);
                i++;
            }
            if (stop_coroutine)
            {
                while (i < str.Length)
                {
                    slow_string += str[i++];
                }
            }
            stop_coroutine = false;
            start_coroutine = false;
            delay_turn += i;
        }
    }

    void OnGUI()
    {
        GUI.skin.font = my_font;
        GUI.contentColor = Color.white;
        GUI.Label(my_rect, slow_string);
    }

    void Start()
    {
        SetDelay(0.1F);
        SetDelayExclamation(0.05F);
        SetDelayPoint(0.15F);
        GetFontSize(my_font);
        GetRatio(my_font_size);
        SetTxt(ambiance_txt);
        SetRect(200, 200, 150, 150);
        SetTabDelay(txt);
        SubTxt(txt, my_rect);
        StartCoroutine(OnceAtTime(show_txt, delay_tab));
        x += index_of_space + 1;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (txt_full)
            {
                if (start_coroutine)
                {
                    stop_coroutine = true;
                }
            }

            else {
                if (start_coroutine)
                {
                    stop_coroutine = true;
                }

                else
                {
                    if (x + size_max_display > length_txt && !is_over)
                    {
                        end_txt = true;
                        SubTxt(txt, my_rect);
                        StartCoroutine(OnceAtTime(show_txt, delay_tab));
                        is_over = true;
                    }
                    if (!end_txt)
                    {
                        SubTxt(txt, my_rect);
                        StartCoroutine(OnceAtTime(show_txt, delay_tab));
                        x += index_of_space + 1;
                    }
                }
            }
        }
    }
}