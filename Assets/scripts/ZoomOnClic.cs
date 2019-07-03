/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using UnityEngine;
using System.Collections;

public class ZoomOnClic : MonoBehaviour {
	public float m_speed = 1f;
	public float m_mouseScroolMultiplier = 10f;
	public float m_maxZoomFactor = 2f;

	private float m_initOrthoSize = 0.0f;
	private Vector3 m_initPosition = Vector3.zero;

	//private Vector3 m_centerForZoomOut = Vector3.zero;
	//private float m_averageCount = 0f;

	private Vector3 m_initialTopLeftCorner = Vector3.zero;
	private Vector3 m_initialTopRightCorner = Vector3.zero;

	private bool m_isZooming = false;

	// Use this for initialization
	void Start () {
		Camera currentCamera = this.GetComponent<Camera>();

		if (currentCamera == null) {
			return;
		}

		m_initOrthoSize = currentCamera.orthographicSize;
		m_initPosition = currentCamera.transform.position;
	}

	public void reinit () {
		//m_centerForZoomOut = Vector3.zero;
		//m_averageCount = 0f;

		Camera currentCamera = this.GetComponent<Camera>();

		if (currentCamera == null) {
			return;
		}

		currentCamera.orthographicSize = m_initOrthoSize;
		currentCamera.transform.position = m_initPosition;

		m_initialTopLeftCorner = currentCamera.ViewportToWorldPoint(new Vector3(0,1,currentCamera.nearClipPlane));
		m_initialTopRightCorner = currentCamera.ViewportToWorldPoint(new Vector3(1,1,currentCamera.nearClipPlane));

	}

	void Update() {
		m_isZooming = false;

		Camera currentCamera = this.GetComponent<Camera>();

		if (currentCamera == null) {
			return;
		}

		bool mustZoomIn = false;
		bool mustZoomOut = false;

		float speed = m_speed;

		Vector3 pointToZoomIn = Vector3.zero;

		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
		//if (Input.GetMouseButton(0)) {
			mustZoomIn = true;
			speed = speed * m_mouseScroolMultiplier;
			pointToZoomIn = currentCamera.ScreenToWorldPoint(Input.mousePosition);
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			mustZoomOut = true;
			speed = speed * m_mouseScroolMultiplier;
		}

		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			if (deltaMagnitudeDiff < 0) {
				mustZoomIn = true;
				pointToZoomIn = currentCamera.ScreenToWorldPoint((touchZero.position + touchOne.position)/2.0f);
				speed = speed * -deltaMagnitudeDiff;
			} else if (deltaMagnitudeDiff > 0) {
				mustZoomOut = true;
				speed = speed * deltaMagnitudeDiff;
			}
		}

		// Scroll forward
		if (mustZoomIn)
		{
			ZoomOrthoCamera(pointToZoomIn, speed);
			m_isZooming = true;
			//m_averageCount += 1.0f;
			//m_centerForZoomOut = m_centerForZoomOut + (Input.mousePosition/m_averageCount - m_centerForZoomOut/m_averageCount);
		}

		// Scoll back
		if (mustZoomOut)
		{
			Vector3 currentCameraTopLeftCorner = currentCamera.ViewportToWorldPoint(new Vector3(0,1, currentCamera.nearClipPlane));
			Vector3 currentCameraTopRightCorner = currentCamera.ViewportToWorldPoint(new Vector3(1,1, currentCamera.nearClipPlane));

			currentCameraTopLeftCorner.z = m_initialTopLeftCorner.z;
			currentCameraTopRightCorner.z = m_initialTopRightCorner.z;

			Vector3 intersectionPoint = new Vector3();

			// The idea here is to find the "invariant point" when zooming out, so that the camera will not
			// zoom outside the game area. The "invariant point" is computing by finding the intersection
			// between the line from the TopLeft corner of the scene and the TopLeft corner of the camera
			// and the line from the TopRight corner of the scene and the TopRight corner of the camera
			// thus finding the best "homotecy" transformation.
			if (!LineLineIntersection(out intersectionPoint, 
				m_initialTopLeftCorner, m_initialTopLeftCorner-currentCameraTopLeftCorner,
				m_initialTopRightCorner, m_initialTopRightCorner-currentCameraTopRightCorner))
			{
				//print ("BAH ALORS2 !");
			}

			ZoomOrthoCamera(/*currentCamera.ScreenToWorldPoint(Input.mousePosition)*/ intersectionPoint, -speed);
			m_isZooming = true;
		}
	}

	// Based on MasterKelli's work http://answers.unity3d.com/questions/384753/ortho-camera-zoom-to-mouse-point.html
	void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
	{
		Camera currentCamera = this.GetComponent<Camera>();

		if (currentCamera == null) {
			return;
		}

		// Calculate how much we will have to move towards the zoomTowards position
		float multiplier = (1.0f / currentCamera.orthographicSize * amount);

		// Zoom camera
		currentCamera.orthographicSize -= amount;

		float min = m_initOrthoSize/m_maxZoomFactor;
		float max = m_initOrthoSize;

		// Limit zoom
		currentCamera.orthographicSize = Mathf.Clamp(currentCamera.orthographicSize, min, max);

		if ((currentCamera.orthographicSize == min) || (currentCamera.orthographicSize == max)) {
			if (currentCamera.orthographicSize == max) {
				reinit();	
			}

			return;
		}

		// Move camera
		transform.position += (zoomTowards - transform.position) * multiplier; 


	}

	//Based on Bit Barrel Media's work : http://wiki.unity3d.com/index.php/3d_Math_functions
	//Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
	//Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
	//same plane, use ClosestPointsOnTwoLines() instead.
	public bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){

		intersection = Vector3.zero;

		Vector3 lineVec3 = linePoint2 - linePoint1;
		Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

		float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

		//Lines are not coplanar. Take into account rounding errors.
		if((planarFactor >= 0.00001f) || (planarFactor <= -0.00001f)){
			return false;
		}

		//Note: sqrMagnitude does x*x+y*y+z*z on the input vector.
		float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;

		if((s >= 0.0f) && (s <= 1.0f)){

			intersection = linePoint1 + (lineVec1 * s);
			return true;
		}

		else{
			/* Raf : I don't understand this "check", but even if false, the intersection point is valid */
			intersection = linePoint1 + (lineVec1 * s);
			return false;       
		}
	}

	public bool IsZooming() {
		return m_isZooming;
	}


}
