/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using UnityEngine;
using System.Collections;

public class AdaptCircleColliderToActualSize : MonoBehaviour {
	public CircleCollider2D m_collider;
	public SpriteRenderer m_spriteRenderer;


	void Update() {
		float diameter = m_spriteRenderer.sprite.bounds.size.x;

		m_collider.radius = diameter/2.0f;
	}
}
