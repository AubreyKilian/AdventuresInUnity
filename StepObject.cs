using UnityEngine;
using System.Collections;

/* StepObject.cs
 * 
 * Written by: Aubrey Kilian <aubreykilian (at) gmail (dot) com
 * Date: 2014-08-10
 * See more: http://www.dociletree.co.za/
 * 
 * Purpose:
 * Making an object move one step at a time - Tetris-style
 * 
 * License: Apache 2.0
 * 
 * Use as you wish, please let me know if you do.
 * 
 * Usage:
 * 
 * 1) Drag this script into your Project
 * 2) Attach this script to any object that you want to move
 * 3) Select the object in your hierarchy and look at the script in the Inspector
 * 4) Specify how many seconds will pass between each "step" the object will take.  1=1second
 * 5) Specify the size per step, and direction, in the Move Direction property
 * 6) Optionally specify how many steps the object should take before it stops moving
 * 7) Tick the "Moving" tickbox to make the object start moving as soon as the scene starts
 * 
 */

public class StepObject : MonoBehaviour {
	
	public Vector3 MoveDirection;
	public float SecondsPerStep;
	public int NumberOfStepsBeforeStopping;
	public bool StartMoving;

	float Timer = 0;
	int Counter = 0;
	private bool Moving;

	void Start () {
		if(SecondsPerStep > 0 && MoveDirection != Vector3.zero)
			Moving = StartMoving;
	}
	
	void Update () {
		Timer += Time.deltaTime;
		if(Moving && Timer >= SecondsPerStep) {
			Vector3 pos = transform.position + MoveDirection;
			transform.position = pos;
			Timer = 0;
			Counter++;
			if(NumberOfStepsBeforeStopping > 0 && Counter >= NumberOfStepsBeforeStopping)
				Moving = false;
		}
	}
}
