/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class ScrollingUp : MonoBehaviour {

    public string scroll_txt = "Le chien est la première espèce animale à avoir été domestiquée par l'Homme pour l'usage de la chasse dans une société humaine paléolithique qui ne maitrise alors ni l'agriculture ni l'élevage. La lignée du chien s'est différenciée génétiquement de celle du loup gris il y a environ 100 000 ans, et les plus anciens restes confirmés de chien domestique sont vieux, selon les sources, de 33 000 ans, ou de 12 000 ans, donc antérieurs de plusieurs dizaines de milliers d'années à ceux de toute autre espèce domestique connue. Depuis la Préhistoire, le chien a accompagné l'homme durant toute sa phase de sédentarisation, marquée par l'apparition des premières civilisations agricoles. C'est à ce moment qu'il a acquis la capacité de digérer l'amidon, et que ses fonctions d'auxiliaire de l'homme se sont étendues. Ces nouvelles fonctions ont entrainé une différenciation accrue de la sous-espèce et l'apparition progressive de races canines identifiables. Le chien est aujourd'hui utilisé à la fois comme animal de travail et comme animal de compagnie. Son instinct de meute, sa domestication précoce et les caractéristiques comportementales qui en découlent lui valent familièrement le surnom de « meilleur ami de l'Homme ».";
    public Rect my_rect = new Rect(100, 100, 300, 150);
    public int txt_length, index_space = 0;
    public float size_max_display_line;
    public float ratio = 0.18F;
    public string part_txt;
    public bool end_txt = false;

    public float GetSizeMaxDisplayLine (string str)
    {
        return size_max_display_line = my_rect.width * ratio; 
    }

    void OnGUI()
    {
        GUI.Label(new Rect(200, 100, 200, 200), scroll_txt);
    }

	// Use this for initialization
	void Start () {
        GetSizeMaxDisplayLine(scroll_txt);
	}
	
	// Update is called once per frame
	void Update () {

        txt_length = scroll_txt.Length;

        if (txt_length <= (int)size_max_display_line)
        {
            end_txt = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (!end_txt)
            {
                part_txt = scroll_txt;
                part_txt = part_txt.Substring(0, (int)size_max_display_line + 1);
                index_space = part_txt.LastIndexOf(' ');
                scroll_txt = scroll_txt.Substring(index_space + 1, txt_length - (index_space + 1));
            }
        }
	}
}
