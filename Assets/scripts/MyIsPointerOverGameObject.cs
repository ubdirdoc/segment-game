/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using UnityEngine.EventSystems;


public class MyIsPointerOverGameObject : MonoBehaviour
{
	public MyIsPointerOverGameObject ()
	{
	}

	public static bool IsPointerOverGameObject ()
	{
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return true;
		}

		for (int i = 0; i <= Input.touchCount; i++) {
			if (EventSystem.current.IsPointerOverGameObject(i)) {
				return true;
			}
		}

		return false;


	}
}


