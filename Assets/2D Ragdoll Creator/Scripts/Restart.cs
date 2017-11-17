using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	//if mouse is clicked on this object on which this script is attached
	void OnMouseDown()
	{
		//load last loaded level. in logic it's restart
		Application.LoadLevel(Application.loadedLevel);
	}

	void Update()
	{
		//restart if more than 2 touch is detected
		if(Input.touchCount > 2)
			Application.LoadLevel (Application.loadedLevel);
	}
}
