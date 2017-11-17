//In this script we have dragger object which has spring joint component attached.
//When we move mouse or finger on screen if this runs on touch device and forTouchScreen variable is set to true(from inspector window)
//We are positioning dragger object to that position. If we click/tap on 2D object, connect to it. 
//So now when you move cursor/finger, dragger object follows it and unity's physics does everything else to make object follow

using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour {

	public GameObject dragger;
	public bool forTouchScreen = false;

	private SpringJoint2D springJoint ;
	private bool dragging = false;

	void Start()
	{
		//get springJoint2D component from dragger object
		springJoint = dragger.GetComponent<SpringJoint2D>();
	}

	void Update ()
	{
		// Make sure the user pressed the mouse down
		if (forTouchScreen ? Input.touchCount > 0 : Input.GetMouseButton(0))
		{
			Vector3 pos;

			//if forTouchScreen variable is true (set from inspector window) use touches, else use mouse for dragging
			if(forTouchScreen)
				pos = GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(0).position);//get position, where touch is detected
			else
				pos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition); //get position, where mouse cursor is

			//make dragger object's position same as 
			springJoint.transform.position = pos;


			//cast ray
			RaycastHit2D  hit;
			hit = Physics2D.Raycast(pos, Vector2.zero);

			//check if hit object has collider and we aren't still dragging object
			if(hit.collider && !dragging)
			{
				//change springjoint anchor & connectedAnchor positions and connect to hit object
				springJoint.anchor = springJoint.transform.InverseTransformPoint (hit.point);
				springJoint.connectedAnchor = hit.transform.InverseTransformPoint (hit.point);
				springJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();

				dragging = true;
			}
		}
		else
		{
			//if mouse 0 button or touch isn't detected make springJoint's connected body null. So we aren't dragging object, it'll be free to move
			if(springJoint)
				springJoint.connectedBody = null;

			dragging = false;
		}
	}
}
