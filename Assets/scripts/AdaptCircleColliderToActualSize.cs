/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
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
