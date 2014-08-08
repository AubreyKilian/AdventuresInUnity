using UnityEngine;
using System.Collections;

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
		Debug.Log ("topRight: " + topRight);
		Debug.Log ("Middle: " + middle);
		Debug.Log ("bgSize: " + bgSize);

		int numWide = Mathf.CeilToInt((topRight.x * 2) / bgSize.x ) + 1 + NumberExtraTiles;
		int numHigh = Mathf.CeilToInt((topRight.y * 2) / bgSize.y ) + 1 + NumberExtraTiles;
		while(numWide % 2 < 1) { numWide += 1; }
		while(numHigh % 2 < 1) { numHigh += 1; }
		if(equalUpAndDown) {
			numHigh = (numHigh > numWide) ? numHigh : numWide;
			numWide = numHigh;
		}
		Debug.Log (numWide + " wide, " + numHigh + " high");

		float endX = middle.x + (Mathf.Round(numWide / 2) * bgSize.x);
		float endY = middle.y + (Mathf.Round(numHigh / 2) * bgSize.y);
		float startX = endX * -1;
		float startY = endY * -1;
		Debug.Log (startX + " -> " + endX);
		Debug.Log (startY + " -> " + endY);

		for(int countX = 0; countX<numWide; countX++) {
			float i = countX * bgSize.x + startX;
//			Debug.Log (i);
			for(int countY = 0; countY < numHigh; countY++) {
				float j = countY * bgSize.y + startY;
				Debug.Log ("i: " + i + ", j: " + j);
				Quaternion quat = new Quaternion ();
				quat = Quaternion.identity;
				if(flipEveryOther && (countX + countY) % 2 < 1) {
					//Debug.Log ("Flipping / " + i + " / " + j + " / " + (i + j) % 2);
					quat = Quaternion.Euler (new Vector3 (0, 0, 180));
				}
				Vector3 NewPosition = new Vector3(i, j, ZOffset);
				Debug.Log ("Creating new clone at: " + NewPosition );
				GameObject Go = Instantiate (BackgroundImage) as GameObject;
				Go.transform.parent = transform;
				Go.transform.position = NewPosition;
				Go.transform.rotation = quat;
			}
		}
	}
}
