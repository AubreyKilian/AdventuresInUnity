using UnityEngine;
using System.Collections;

/* CreateBackground.cs
 * 
 * Written by: Aubrey Kilian <aubreykilian (at) gmail (dot) com
 * Date: 2014-08-08
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
 * 1) Create a plane
 * 2) Point your camera straight at the plane (in 2D, Orthographic mode works best)
 * 3) Drag this script into your Project
 * 4) Drag the script onto the plane
 * 5) Drag your image you want to use into your project
 * 6) Create a prefab of the image (Drag image from Project into Hierarchy, Draw image from Hierarchy to Project, Delete image from Hierarchy)
 * 7) Click plane
 * 8) Drag newly created prefab into "Background Image" property of script in Inspector
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
	public float RotateSpeed;

	void FixedUpdate () {
		if(RotateBackground && BackgroundImage && RotateSpeed > 0) 
			transform.RotateAround (new Vector3 (0, 0, 0), new Vector3 (0, 0, 2), RotateSpeed * Time.deltaTime);
	}

	void Start ()
	{
		if(BackgroundImage)
			doBackground ();
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
