using UnityEngine;
using System.Collections;

/* CreateBackground.cs
 * 
 * Written by: Aubrey Kilian <aubreykilian (at) gmail (dot) com
 * Date: 2014-08-08
 * See more: http://www.dociletree.co.za/
 * 
 * Purpose:
 * Tile a plane to be used as a background with a prefab, optionally allowing the background to rotate.
 * 
 * License: Apache 2.0
 * 
 * Use as you wish, please let me know if you do.
 * 
 * Usage:
 * 
 * 1) Create an empty game object and name is something like "Background"
 * 2) Place the object in the center of your camera (in 2D, Orthographic mode works best, camera at 0,0,-10, object at 0,0,0)
 * 3) Drag this script into your Project
 * 4) Drag the script onto the plane
 * 5) Drag your image you want to use into your project
 * 6) Create a prefab of the image (Drag image from Project into Hierarchy, Drag image from Hierarchy to Project, Delete image from Hierarchy)
 * 7) Select your background object
 * 8) Drag the newly created prefab into "Background Image" property of script in Inspector
 * 9) Adjust other optional values in Inspector for script
 * 
 */

public class CreateBackground : MonoBehaviour
{
	public GameObject BackgroundImage;
	public bool flipEveryOther;
	public bool equalUpAndDown;
	public float ZOffset;
	public int NumberExtraTiles; // The script always adds 1 until we get an uneven number, to make sure we have a tile in the middle always
	public bool RotateBackground;
	public bool RotateAntiClockwise;
	public float RotateSpeed;
	private int ReverseAdjuster = -1;

	void FixedUpdate () {
		if(RotateBackground && BackgroundImage && RotateSpeed > 0) 
			transform.RotateAround (new Vector3 (0, 0, 0), new Vector3 (0, 0, 2), RotateSpeed * Time.deltaTime * ReverseAdjuster);
	}

	void Start ()
	{
		if(BackgroundImage)
			doBackground ();

		if(RotateAntiClockwise)
			ReverseAdjuster = 1;
	}

	void doBackground ()
	{
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));
		Vector3 middle = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width/2, Screen.height/2, 0));
		Vector3 bgSize = BackgroundImage.renderer.bounds.size;

		int numWide = Mathf.CeilToInt((topRight.x * 2) / bgSize.x ) + 1 + NumberExtraTiles;
		int numHigh = Mathf.CeilToInt((topRight.y * 2) / bgSize.y ) + 1 + NumberExtraTiles;
		while(numWide % 2 < 1) { numWide += 1; }
		while(numHigh % 2 < 1) { numHigh += 1; }
		if(equalUpAndDown) {
			numHigh = (numHigh > numWide) ? numHigh : numWide;
			numWide = numHigh;
		}

		float endX = middle.x + (Mathf.Round(numWide / 2) * bgSize.x);
		float endY = middle.y + (Mathf.Round(numHigh / 2) * bgSize.y);
		float startX = endX * -1;
		float startY = endY * -1;

		for(int countX = 0; countX<numWide; countX++) {
			float i = countX * bgSize.x + startX;
			for(int countY = 0; countY < numHigh; countY++) {
				float j = countY * bgSize.y + startY;
				Quaternion quat = new Quaternion ();
				quat = Quaternion.identity;
				if(flipEveryOther && (countX + countY) % 2 < 1) {
					quat = Quaternion.Euler (new Vector3 (0, 0, 180));
				}
				Vector3 NewPosition = new Vector3(i, j, ZOffset);
				GameObject Go = Instantiate (BackgroundImage) as GameObject;
				Go.transform.parent = transform;
				Go.transform.position = NewPosition;
				Go.transform.rotation = quat;
			}
		}
	}
}
