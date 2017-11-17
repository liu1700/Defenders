//What is this and how does it work

//This script creates editor window to create 2D ragdoll
//You can enter body parts manually or name all parts suitable and use auto assign button. You can see names in ragdoll prefabs, in prefab folder;
//It isn't necessary to assign all parts, just don't enter parts which you don't want. You can see ragdoll prefabs with different joint count and style(side, front view)
//You can use this also on 3D meshes. If they have assigned any 2D collider it'll be remained if not, polygon colliders will be added to them when creating ragdoll.


//How to use

// 1) Assign root object where all body parts are collected
// 2) Assign body parts manually or with auto assign(if body parts are named suitable)
// 3) Create joint positions
// 4) Arrange joint position objects where you want to create joints
// 5) Create ragdoll, have fun ;)

//For details read documentation


using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ragdoll_Creator_2D : EditorWindow
{
	//root object where all body parts are collected
	private Transform root;

	//variables for saving body parts
	private GameObject head;
	private GameObject neck;
	private GameObject body;
	private GameObject spine;
	private GameObject upperArm1;
	private GameObject upperArm2;
	private GameObject lowerArm1;
	private GameObject lowerArm2;
	private GameObject wrist1;
	private GameObject wrist2;
	private GameObject thigh1;
	private GameObject thigh2;
	private GameObject leg1;
	private GameObject leg2;
	private GameObject foot1;
	private GameObject foot2;

	//variables for saving joint positions
	private Transform headJointPosition;
	private Transform neckJointPosition;
	private Transform spineJointPosition;
	private Transform upperArm1JointPosition;
	private Transform upperArm2JointPosition;
	private Transform lowerArm1JointPosition;
	private Transform lowerArm2JointPosition;
	private Transform wrist1JointPosition;
	private Transform wrist2JointPosition;
	private Transform thigh1JointPosition;
	private Transform thigh2JointPosition;
	private Transform leg1JointPosition;
	private Transform leg2JointPosition;
	private Transform foot1JointPosition;
	private Transform foot2JointPosition;

	//arrays to hold all body parts and joint positions
	private List<Transform> bodyParts;
	private List<Transform> jointPositions;

	//booleans to use or not limits for body parts
	private bool headUseLimit = true;
	private bool neckUseLimit = true;
	private bool spineUseLimit = true;
	private bool upperArm1UseLimit = false;
	private bool upperArm2UseLimit = false;
	private bool lowerArm1UseLimit = true;
	private bool lowerArm2UseLimit = true;
	private bool wrist1UseLimit = true;
	private bool wrist2UseLimit = true;
	private bool thigh1UseLimit = true;
	private bool thigh2UseLimit = true;
	private bool leg1UseLimit = true;
	private bool leg2UseLimit = true;
	private bool foot1UseLimit = true;
	private bool foot2UseLimit = true;


	//lower and upper angles for body parts
	private string headLowerAngle = "-30";
	private string headUpperAngle = "30";

	private string neckLowerAngle = "-10";
	private string neckUpperAngle = "10";

	private string spineLowerAngle = "-20";
	private string spineUpperAngle = "20";

	private string upperArm1LowerAngle = "-45";
	private string upperArm1UpperAngle = "45";

	private string upperArm2LowerAngle = "-45";
	private string upperArm2UpperAngle = "45";

	private string lowerArm1LowerAngle = "-45";
	private string lowerArm1UpperAngle = "45";

	private string lowerArm2LowerAngle = "-45";
	private string lowerArm2UpperAngle = "45";

	private string wrist1LowerAngle = "-30";
	private string wrist1UpperAngle = "30";

	private string wrist2LowerAngle = "-30";
	private string wrist2UpperAngle = "30";

	private string thigh1LowerAngle = "-45";
	private string thigh1UpperAngle = "45";

	private string thigh2LowerAngle = "-45";
	private string thigh2UpperAngle = "45";

	private string leg1LowerAngle = "-45";
	private string leg1UpperAngle = "45";

	private string leg2LowerAngle = "-45";
	private string leg2UpperAngle = "45";

	private string foot1LowerAngle = "-30";
	private string foot1UpperAngle = "30";

	private string foot2LowerAngle = "-30";
	private string foot2UpperAngle = "30";


	private string rootTempName;


	private bool showJointPositions = true;
	private bool jointPositionsCreated = false;

	private string helpText ;

	 
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/2D Ragdoll Creator")]
	static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(Ragdoll_Creator_2D), false, "2D Ragdoll Creator", true);
	}


	void OnGUI()
	{
		GUILayout.Space(10);

		if(root)
			rootTempName = root.name;

		root = (Transform)EditorGUILayout.ObjectField ("Root", root, typeof(Transform), true);
		if(root && root.name != rootTempName)
		{
			jointPositionsCreated = false;

			//clear body parts
			body = null;
			head = null;
			neck = null;
			spine = null;
			upperArm1 = null;
			upperArm2 = null;
			lowerArm1 = null;
			lowerArm2 = null;
			wrist1 = null;
			wrist2 = null;
			thigh1 = null;
			thigh2 = null;
			leg1 = null;
			leg2 = null;
			foot1 = null;
			foot2 = null;
		}

		//create object assignment fields for body parts
		//create check box to controll use or not use angle limits on body parts
		//create text fields for body part's lower and upper angles and make imposible to enter 0 there

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		body = (GameObject)EditorGUILayout.ObjectField ("Body", body, typeof(GameObject), true);
		GUILayout.Space(20);
		GUILayout.Label ("Use Limits");
		GUILayout.Label ("Lower Angle");
		GUILayout.Label ("Upper Angle");
		GUILayout.EndHorizontal ();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		head = (GameObject)EditorGUILayout.ObjectField ("Head", head, typeof(GameObject), true);
		GUILayout.Space (10);
		headUseLimit = GUILayout.Toggle(headUseLimit,"");
		headLowerAngle = GUILayout.TextField (headLowerAngle,4);
		headLowerAngle = Regex.Replace(headLowerAngle, @"[^0-9-]", "");
		if(headLowerAngle == "") 
			headLowerAngle = "0";
		headUpperAngle = GUILayout.TextField (headUpperAngle,4);
		headUpperAngle = Regex.Replace(headUpperAngle, @"[^0-9-]", "");
		if(headUpperAngle == "")
			headUpperAngle = "0";
		GUILayout.EndHorizontal ();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		neck = (GameObject)EditorGUILayout.ObjectField ("Neck", neck, typeof(GameObject), true);
		GUILayout.Space (10);
		neckUseLimit = GUILayout.Toggle(neckUseLimit,"");
		neckLowerAngle = GUILayout.TextField (neckLowerAngle,4);
		neckLowerAngle = Regex.Replace(neckLowerAngle, @"[^0-9-]", "");
		if(neckLowerAngle == "") 
			neckLowerAngle = "0";
		neckUpperAngle = GUILayout.TextField (neckUpperAngle,4);
		neckUpperAngle = Regex.Replace(neckUpperAngle, @"[^0-9-]", "");
		if(neckUpperAngle == "")
			neckUpperAngle = "0";
		GUILayout.EndHorizontal ();
		
		GUILayout.Space(5);
		
		GUILayout.BeginHorizontal ();
		spine = (GameObject)EditorGUILayout.ObjectField ("Spine", spine, typeof(GameObject), true);
		GUILayout.Space (10);
		spineUseLimit = GUILayout.Toggle(spineUseLimit,"");
		spineLowerAngle = GUILayout.TextField (spineLowerAngle,4);
		spineLowerAngle = Regex.Replace(spineLowerAngle, @"[^0-9-]", "");
		if(spineLowerAngle == "") 
			spineLowerAngle = "0";
		spineUpperAngle = GUILayout.TextField (spineUpperAngle,4);
		spineUpperAngle = Regex.Replace(spineUpperAngle, @"[^0-9-]", "");
		if(spineUpperAngle == "")
			spineUpperAngle = "0";
		GUILayout.EndHorizontal ();
		
		GUILayout.Space(5);
		
		GUILayout.BeginHorizontal();
		upperArm1 = (GameObject)EditorGUILayout.ObjectField ("Upper Arm 1", upperArm1, typeof(GameObject), true);
		GUILayout.Space (10);
		upperArm1UseLimit = GUILayout.Toggle(upperArm1UseLimit,"");
		upperArm1LowerAngle = GUILayout.TextField (upperArm1LowerAngle,4);
		upperArm1LowerAngle = Regex.Replace (upperArm1LowerAngle, @"[^0-9-]", "");
		if(upperArm1LowerAngle == "") 
			upperArm1LowerAngle = "0";
		upperArm1UpperAngle = GUILayout.TextField (upperArm1UpperAngle,4);
		upperArm1UpperAngle = Regex.Replace (upperArm1UpperAngle, @"[^0-9-]", "");
		if(upperArm1UpperAngle == "") 
			upperArm1UpperAngle = "0";
		GUILayout.EndHorizontal ();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		upperArm2 = (GameObject)EditorGUILayout.ObjectField ("Upper Arm 2", upperArm2, typeof(GameObject), true);
		GUILayout.Space (10);
		upperArm2UseLimit = GUILayout.Toggle(upperArm2UseLimit,"");
		upperArm2LowerAngle = GUILayout.TextField (upperArm2LowerAngle,4);
		upperArm2LowerAngle = Regex.Replace (upperArm2LowerAngle, @"[^0-9-]", "");
		if(upperArm2LowerAngle == "") 
			upperArm2LowerAngle = "0";
		upperArm2UpperAngle = GUILayout.TextField (upperArm2UpperAngle,4);
		upperArm2UpperAngle = Regex.Replace (upperArm2UpperAngle, @"[^0-9-]", "");
		if(upperArm2UpperAngle == "") 
			upperArm2UpperAngle = "0";
		GUILayout.EndHorizontal ();
		
		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		lowerArm1 = (GameObject)EditorGUILayout.ObjectField ("Lower Arm 1", lowerArm1, typeof(GameObject), true);
		GUILayout.Space (10);
		lowerArm1UseLimit = GUILayout.Toggle(lowerArm1UseLimit,"");
		lowerArm1LowerAngle = GUILayout.TextField (lowerArm1LowerAngle,4);
		lowerArm1LowerAngle = Regex.Replace(lowerArm1LowerAngle, @"[^0-9-]", "");
		if(lowerArm1LowerAngle == "") 
			lowerArm1LowerAngle = "0";
		lowerArm1UpperAngle = GUILayout.TextField (lowerArm1UpperAngle,4);
		lowerArm1UpperAngle = Regex.Replace(lowerArm1UpperAngle, @"[^0-9-]", "");
		if(lowerArm1UpperAngle == "") 
			lowerArm1UpperAngle = "0";
		GUILayout.EndHorizontal();
		
		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		lowerArm2 = (GameObject)EditorGUILayout.ObjectField ("Lower Arm 2", lowerArm2, typeof(GameObject), true);
		GUILayout.Space (10);
		lowerArm2UseLimit = GUILayout.Toggle(lowerArm2UseLimit,"");
		lowerArm2LowerAngle = GUILayout.TextField (lowerArm2LowerAngle,4);
		lowerArm2LowerAngle = Regex.Replace(lowerArm2LowerAngle, @"[^0-9-]", "");
		if(lowerArm2LowerAngle == "") 
			lowerArm2LowerAngle = "0";
		lowerArm2UpperAngle = GUILayout.TextField (lowerArm2UpperAngle,4);
		lowerArm2UpperAngle = Regex.Replace(lowerArm2UpperAngle, @"[^0-9-]", "");
		if(lowerArm2UpperAngle == "") 
			lowerArm2UpperAngle = "0";
		GUILayout.EndHorizontal();

		GUILayout.Space(5);
		
		GUILayout.BeginHorizontal ();
		wrist1 = (GameObject)EditorGUILayout.ObjectField ("Wrist 1", wrist1, typeof(GameObject), true);
		GUILayout.Space (10);
		wrist1UseLimit = GUILayout.Toggle(wrist1UseLimit,"");
		wrist1LowerAngle = GUILayout.TextField (wrist1LowerAngle,4);
		wrist1LowerAngle = Regex.Replace(wrist1LowerAngle, @"[^0-9-]", "");
		if(wrist1LowerAngle == "") 
			wrist1LowerAngle = "0";
		wrist1UpperAngle = GUILayout.TextField (wrist1UpperAngle,4);
		wrist1UpperAngle = Regex.Replace(wrist1UpperAngle, @"[^0-9-]", "");
		if(wrist1UpperAngle == "") 
			wrist1UpperAngle = "0";
		GUILayout.EndHorizontal();
		
		GUILayout.Space(5);
		
		GUILayout.BeginHorizontal ();
		wrist2 = (GameObject)EditorGUILayout.ObjectField ("Wrist 2", wrist2, typeof(GameObject), true);
		GUILayout.Space (10);
		wrist2UseLimit = GUILayout.Toggle(wrist2UseLimit,"");
		wrist2LowerAngle = GUILayout.TextField (wrist2LowerAngle,4);
		wrist2LowerAngle = Regex.Replace(wrist2LowerAngle, @"[^0-9-]", "");
		if(wrist2LowerAngle == "") 
			wrist2LowerAngle = "0";
		wrist2UpperAngle = GUILayout.TextField (wrist2UpperAngle,4);
		wrist2UpperAngle = Regex.Replace(wrist2UpperAngle, @"[^0-9-]", "");
		if(wrist2UpperAngle == "") 
			wrist2UpperAngle = "0";
		GUILayout.EndHorizontal();
		
		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		thigh1 = (GameObject)EditorGUILayout.ObjectField ("Thigh 1", thigh1, typeof(GameObject), true);
		GUILayout.Space (10);
		thigh1UseLimit = GUILayout.Toggle(thigh1UseLimit,"");
		thigh1LowerAngle = GUILayout.TextField (thigh1LowerAngle,4);
		thigh1LowerAngle = Regex.Replace(thigh1LowerAngle, @"[^0-9-]", "");
		if(thigh1LowerAngle == "") 
			thigh1LowerAngle = "0";
		thigh1UpperAngle = GUILayout.TextField (thigh1UpperAngle,4);
		thigh1UpperAngle = Regex.Replace(thigh1UpperAngle, @"[^0-9-]", "");
		if(thigh1UpperAngle == "") 
			thigh1UpperAngle = "0";
		GUILayout.EndHorizontal();
		
		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		thigh2 = (GameObject)EditorGUILayout.ObjectField ("Thigh 2", thigh2, typeof(GameObject), true);
		GUILayout.Space (10);
		thigh2UseLimit = GUILayout.Toggle(thigh2UseLimit,"");
		thigh2LowerAngle = GUILayout.TextField (thigh2LowerAngle,4);
		thigh2LowerAngle = Regex.Replace(thigh2LowerAngle, @"[^0-9-]", "");
		if(thigh2LowerAngle == "") 
			thigh2LowerAngle = "0";
		thigh2UpperAngle = GUILayout.TextField (thigh2UpperAngle,4);
		thigh2UpperAngle = Regex.Replace(thigh2UpperAngle, @"[^0-9-]", "");
		if(thigh2UpperAngle == "") 
			thigh2UpperAngle = "0";
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		leg1 = (GameObject)EditorGUILayout.ObjectField ("Leg 1", leg1, typeof(GameObject), true);
		GUILayout.Space (10);
		leg1UseLimit = GUILayout.Toggle(leg1UseLimit,"");
		leg1LowerAngle = GUILayout.TextField (leg1LowerAngle,4);
		leg1LowerAngle = Regex.Replace(leg1LowerAngle, @"[^0-9-]", "");
		if(leg1LowerAngle == "") 
			leg1LowerAngle = "0";
		leg1UpperAngle = GUILayout.TextField (leg1UpperAngle,4);
		leg1UpperAngle = Regex.Replace(leg1UpperAngle, @"[^0-9-]", "");
		if(leg1UpperAngle == "") 
			leg1UpperAngle = "0";
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		leg2 = (GameObject)EditorGUILayout.ObjectField ("Leg 2", leg2, typeof(GameObject), true);
		GUILayout.Space (10);
		leg2UseLimit = GUILayout.Toggle(leg2UseLimit,"");
		leg2LowerAngle = GUILayout.TextField (leg2LowerAngle,4);
		leg2LowerAngle = Regex.Replace(leg2LowerAngle, @"[^0-9-]", "");
		if(leg2LowerAngle == "") 
			leg2LowerAngle = "0";
		leg2UpperAngle = GUILayout.TextField (leg2UpperAngle,4);
		leg2UpperAngle = Regex.Replace(leg2UpperAngle, @"[^0-9-]", "");
		if(leg2UpperAngle == "") 
			leg2UpperAngle = "0";
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		foot1 = (GameObject)EditorGUILayout.ObjectField ("Foot 1", foot1, typeof(GameObject), true);
		GUILayout.Space (10);
		foot1UseLimit = GUILayout.Toggle(foot1UseLimit,"");
		foot1LowerAngle = GUILayout.TextField (foot1LowerAngle,4);
		foot1LowerAngle = Regex.Replace(foot1LowerAngle, @"[^0-9-]", "");
		if(foot1LowerAngle == "") 
			foot1LowerAngle = "0";
		foot1UpperAngle = GUILayout.TextField (foot1UpperAngle,4);
		foot1UpperAngle = Regex.Replace(foot1UpperAngle, @"[^0-9-]", "");
		if(foot1UpperAngle == "") 
			foot1UpperAngle = "0";
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		foot2 = (GameObject)EditorGUILayout.ObjectField ("Foot 2", foot2, typeof(GameObject), true);
		GUILayout.Space (10);
		foot2UseLimit = GUILayout.Toggle(foot2UseLimit,"");
		foot2LowerAngle = GUILayout.TextField (foot2LowerAngle,4);
		foot2LowerAngle = Regex.Replace(foot2LowerAngle, @"[^0-9-]", "");
		if(foot2LowerAngle == "") 
			foot2LowerAngle = "0";
		foot2UpperAngle = GUILayout.TextField (foot2UpperAngle,4);
		foot2UpperAngle = Regex.Replace(foot2UpperAngle, @"[^0-9-]", "");
		if(foot2UpperAngle == "") 
			foot2UpperAngle = "0";
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		//create help label, which'll give help messages
		GUIStyle guiStyle = new GUIStyle();
		guiStyle.normal.textColor = Color.green;
		guiStyle.fontSize = 14;
		guiStyle.alignment = TextAnchor.MiddleCenter;
		EditorGUILayout.LabelField (helpText, guiStyle, GUILayout.Height (50), GUILayout.MinHeight(10), GUILayout.MinHeight (20), GUILayout.MaxHeight (50)); 

		GUILayout.Space(5);


		//create auto assign button
		//when this is clicked it checks if root object is assigned, if yes it goes to its children and checks their names and assigns to appropriate body part field and saves them in variables 
		//if this object is already ragdoll, it gets and saves joint position objects, fills bodyParts and jointPositions arrays, 
		//sets lower and upper angles for body part field and saves them in varibales too
		if(GUILayout.Button ("Auto Assign", GUILayout.ExpandHeight(true), GUILayout.MinHeight (30), GUILayout.MaxHeight(50)))
		{
			if(root)
			{
				jointPositions = new List<Transform>();
				bodyParts = new List<Transform>();

				var allParts = root.GetComponentsInChildren<Transform>();
				
				foreach(var child in allParts)
				{
					if(child.name == "Joint Position")
					{
						jointPositions.Add(child);

						var parentName = child.parent.name;

						if(parentName == "head" || parentName == "spine" || parentName == "neck" || parentName == "upper arm 1" || parentName == "upper arm 2" || parentName == "lower arm 1" || 
						   parentName == "lower arm 2" ||  parentName == "wrist 1" || parentName == "wrist 2" || parentName == "thigh 1" || 
						   parentName == "thigh 2" || parentName == "leg 1" ||
						   parentName == "leg 2" || parentName == "foot 1" || parentName == "foot 2")
									bodyParts.Add(child.parent);


						if(parentName == "head")
						{
							headJointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								headLowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								headUpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "neck")
						{
							neckJointPosition = child;
							
							if(child.GetComponent<Rigidbody2D>())
							{
								neckLowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								neckUpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "spine")
						{
							spineJointPosition = child;
							
							if(child.GetComponent<Rigidbody2D>())
							{
								spineLowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								spineUpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "upper arm 1")
						{
							upperArm1JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								upperArm1LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								upperArm1UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "upper arm 2")
						{
							upperArm2JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								upperArm2LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								upperArm2UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "lower arm 1")
						{
							lowerArm1JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								lowerArm1LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								lowerArm1UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "lower arm 2")
						{
							lowerArm2JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								lowerArm2LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								lowerArm2UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "wrist 1")
						{
							wrist1JointPosition = child;
							
							if(child.GetComponent<Rigidbody2D>())
							{
								wrist1LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								wrist1UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "wrist 2")
						{
							wrist2JointPosition = child;
							
							if(child.GetComponent<Rigidbody2D>())
							{
								wrist2LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								wrist2UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "thigh 1")
						{
							thigh1JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								thigh1LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								thigh1UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "thigh 2")
						{
							thigh2JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								thigh2LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								thigh2UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "leg 1")
						{
							leg1JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								leg1LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								leg1UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "leg 2")
						{
							leg2JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								leg2LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								leg2UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "foot 1")
						{
							foot1JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								foot1LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								foot1UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}

						if(parentName == "foot 2")
						{
							foot2JointPosition = child;

							if(child.GetComponent<Rigidbody2D>())
							{
								foot2LowerAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.min;
								foot2UpperAngle = "" + child.parent.GetComponent<HingeJoint2D>().limits.max;
							}
						}


						if(jointPositions.Count == 1)
						{
							jointPositionsCreated = true;

							if(child.GetComponent<Renderer>().enabled)
						   		showJointPositions = true;
						   	else
						   		showJointPositions = false;
						}
					}

					else if(child.name == "head")
						head = child.gameObject;

					else if(child.name == "neck")
						neck = child.gameObject;

					else if(child.name == "spine")
						spine = child.gameObject;

					else if(child.name == "body")
						body = child.gameObject;
					
					else if(child.name == "upper arm 1")
						upperArm1 = child.gameObject;
					
					else if(child.name == "upper arm 2")
						upperArm2 = child.gameObject;
					
					else if(child.name == "lower arm 1")
						lowerArm1 = child.gameObject;
					
					else if(child.name == "wrist 1")
						wrist1 = child.gameObject;
					
					else if(child.name == "wrist 2")
						wrist2 = child.gameObject;

					else if(child.name == "lower arm 2")
						lowerArm2 = child.gameObject;
					
					else if(child.name == "thigh 1")
						thigh1 = child.gameObject;
					
					else if(child.name == "thigh 2")
						thigh2 = child.gameObject;
					
					else if(child.name == "leg 1")
						leg1 = child.gameObject;
					
					else if(child.name == "leg 2")
						leg2 = child.gameObject;
					
					else if(child.name == "foot 1")
						foot1 = child.gameObject;
					
					else if(child.name == "foot 2")
						foot2 = child.gameObject;
				}
				helpText = "BODY PARTS ARE ASSIGNED";
		}
		else helpText = "ROOT OBJECT ISN'T ASSIGNED";
		}

		GUILayout.Space (5);


		//create joint position objects and fill "bodyParts" and "jointPositions" arrays
		if(GUILayout.Button ((!jointPositionsCreated? "Create" : "Delete") +" Joint Positions", GUILayout.ExpandHeight(true), GUILayout.MinHeight (30), GUILayout.MaxHeight(50)))
		{
			if(!jointPositionsCreated)
			{
				bodyParts = new List<Transform>();
				jointPositions = new List<Transform>();
				
				if(head)
				{
					bodyParts.Add (head.transform);
					headJointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (headJointPosition);
				}

				if(neck)
				{
					bodyParts.Add (neck.transform);
					neckJointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (neckJointPosition);
				}

				if(spine)
				{
					bodyParts.Add (spine.transform);
					spineJointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (spineJointPosition);
				}

				if(upperArm1)
				{
					bodyParts.Add (upperArm1.transform);
					upperArm1JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (upperArm1JointPosition);
				}

				if(upperArm2)
				{
					bodyParts.Add (upperArm2.transform);
					upperArm2JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (upperArm2JointPosition);
				}

				if(lowerArm1)
				{
					bodyParts.Add (lowerArm1.transform);
					lowerArm1JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (lowerArm1JointPosition);
				}

				if(lowerArm2)
				{
					bodyParts.Add (lowerArm2.transform);
					lowerArm2JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (lowerArm2JointPosition);
				}

				if(wrist1)
				{
					bodyParts.Add (wrist1.transform);
					wrist1JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (wrist1JointPosition);
				}

				if(wrist2)
				{
					bodyParts.Add (wrist2.transform);
					wrist2JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (wrist2JointPosition);
				}

				if(thigh1)
				{
					bodyParts.Add (thigh1.transform);
					thigh1JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (thigh1JointPosition);
				}

				if(thigh2)
				{
					bodyParts.Add (thigh2.transform);
					thigh2JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (thigh2JointPosition);
				}

				if(leg1)
				{
	              	bodyParts.Add (leg1.transform);
					leg1JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (leg1JointPosition);
				}

				if(leg2)
				{
	               	bodyParts.Add (leg2.transform);
					leg2JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (leg2JointPosition);
				}

				if(foot1)
				{
					bodyParts.Add (foot1.transform);
					foot1JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (foot1JointPosition);
				}

				if(foot2)
				{
					bodyParts.Add (foot2.transform);
					foot2JointPosition = GameObject.CreatePrimitive (PrimitiveType.Quad).transform;
					jointPositions.Add (foot2JointPosition);
				}

				for(int i=0; i< jointPositions.Count; i++)
				{
					DestroyImmediate(jointPositions[i].GetComponent<Collider>());
					jointPositions[i].name = "Joint Position";
					
					jointPositions[i].parent = bodyParts[i];
					jointPositions[i].localPosition = new Vector3(0,0,-0.01f);
					jointPositions[i].localScale = bodyParts[i].localScale / 7;
				}

				if(jointPositions.Count > 0)
				{
					jointPositionsCreated = true;
					helpText = "JOINT POSITIONS ARE CREATED";
				}
				else helpText = "BODY PARTS AREN'T ASSIGNED";
			}
			else 
			{
				foreach(var child in jointPositions)
					DestroyImmediate (child.gameObject);

				jointPositionsCreated = false;
				helpText = "JOINT POSITIONS ARE DELETED";
			}
		}

		GUILayout.Space (5);

		//shows or hides joint position objects
		if(GUILayout.Button ((showJointPositions == false? "Show" : "Hide") + " Joint Positions", GUILayout.ExpandHeight(true), GUILayout.MinHeight (30), GUILayout.MaxHeight(50)))
		{
			if(jointPositionsCreated)
			{
				showJointPositions = !showJointPositions;

				foreach(var jointPos in jointPositions)
				{
					jointPos.GetComponent<Renderer>().enabled = showJointPositions == true? true : false;
				}

				if(showJointPositions)
					helpText = "JOINT POSITIONS ARE SHOWN";
				else
					helpText = "JOINT POSITIONS ARE HIDDEN";
			}
			else helpText = "JOINT POSITIONS AREN'T CREATED YET";
		}

		GUILayout.Space(5);

		//creates ragdoll
		//checks if body part have collider2D component, if it doesn't have it, adds it
		//checks for hingeJoint2D component, if it doesn't have it, adds it
		//sets connectedBody object, anchor and connectedAnchor positions and sets angle limits also use or not use those limits
		if(GUILayout.Button ("Create Ragdoll", GUILayout.ExpandHeight(true), GUILayout.MinHeight (30), GUILayout.MaxHeight(50)))
		{
			if(jointPositionsCreated)
			{
				if(!body)
				{
					helpText = "BODY MUST BE ASSIGNED";
					return;
				}

				if(!body.GetComponent<Rigidbody2D>())
				{
					body.AddComponent<Rigidbody2D>();
					if(!body.GetComponent<Collider2D>())
						body.AddComponent<PolygonCollider2D>();
				}


				var limit = new JointAngleLimits2D();

				if(neck)
				{
					//add collider 
					if(!neck.GetComponent<Collider2D>())
						neck.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var neckJoint = neck.GetComponent<HingeJoint2D>()? neck.GetComponent<HingeJoint2D>() : neck.AddComponent<HingeJoint2D>();
					neckJoint.connectedBody = body.GetComponent<Rigidbody2D>();
					neckJoint.anchor = neckJointPosition.localPosition;
					neckJoint.connectedAnchor = body.transform.InverseTransformPoint (neckJointPosition.position);
					
					//add lower and upper angle limits
					limit.min = int.Parse (neckLowerAngle);
					limit.max = int.Parse (neckUpperAngle);
					neckJoint.limits = limit;
					neckJoint.useLimits = neckUseLimit;
				}


				if(spine)
				{
					//add collider 
					if(!spine.GetComponent<Collider2D>())
						spine.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var spineJoint = spine.GetComponent<HingeJoint2D>()? spine.GetComponent<HingeJoint2D>() : spine.AddComponent<HingeJoint2D>();
					spineJoint.connectedBody = body.GetComponent<Rigidbody2D>();
					spineJoint.anchor = spineJointPosition.localPosition;
					spineJoint.connectedAnchor = body.transform.InverseTransformPoint (spineJointPosition.position);
					
					//add lower and upper angle limits
					limit.min = int.Parse (spineLowerAngle);
					limit.max = int.Parse (spineUpperAngle);
					spineJoint.limits = limit;
					spineJoint.useLimits = spineUseLimit;
				}


				if(head)
				{
					//add collider 
					if(!head.GetComponent<Collider2D>())
						head.AddComponent<PolygonCollider2D>();

					//add hinge joint 
					var headJoint = head.GetComponent<HingeJoint2D>()? head.GetComponent<HingeJoint2D>() : head.AddComponent<HingeJoint2D>();
					headJoint.connectedBody = neck? neck.GetComponent<Rigidbody2D>() : body.GetComponent<Rigidbody2D>();
					headJoint.anchor = headJointPosition.localPosition;
					headJoint.connectedAnchor = neck? neck.transform.InverseTransformPoint(headJointPosition.position) : body.transform.InverseTransformPoint (headJointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (headLowerAngle);
					limit.max = int.Parse (headUpperAngle);
					headJoint.limits = limit;
					headJoint.useLimits = headUseLimit;
				}
				


				if(upperArm1)
				{
					//add collider 
					if(!upperArm1.GetComponent<Collider2D>())
						upperArm1.AddComponent<PolygonCollider2D>();

					//add hinge joint 
					var upperArm1Joint =  upperArm1.GetComponent<HingeJoint2D>()? upperArm1.GetComponent<HingeJoint2D>() : upperArm1.AddComponent<HingeJoint2D>();
					upperArm1Joint.connectedBody = body.GetComponent<Rigidbody2D>();
					upperArm1Joint.anchor = upperArm1JointPosition.localPosition;
					upperArm1Joint.connectedAnchor = body.transform.InverseTransformPoint (upperArm1JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (upperArm1LowerAngle);
					limit.max = int.Parse (upperArm1UpperAngle);
					upperArm1Joint.limits = limit;
					upperArm1Joint.useLimits = upperArm1UseLimit;
				}
				

				if(upperArm2)
				{
					//add collider 
					if(!upperArm2.GetComponent<Collider2D>())
						upperArm2.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var upperArm2Joint = upperArm2.GetComponent<HingeJoint2D>()? upperArm2.GetComponent<HingeJoint2D>() : upperArm2.AddComponent<HingeJoint2D>();
					upperArm2Joint.connectedBody = body.GetComponent<Rigidbody2D>();
					upperArm2Joint.anchor = upperArm2JointPosition.localPosition;
					upperArm2Joint.connectedAnchor = body.transform.InverseTransformPoint (upperArm2JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (upperArm2LowerAngle);
					limit.max = int.Parse (upperArm2UpperAngle);
					upperArm2Joint.limits = limit;
					upperArm2Joint.useLimits = upperArm2UseLimit;
				}
				

				if(lowerArm1 && upperArm1)
				{
					//add collider 
					if(!lowerArm1.GetComponent<Collider2D>())
						lowerArm1.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var lowerArm1Joint = lowerArm1.GetComponent<HingeJoint2D>()? lowerArm1.GetComponent<HingeJoint2D>() : lowerArm1.AddComponent<HingeJoint2D>();
					lowerArm1Joint.connectedBody = upperArm1.GetComponent<Rigidbody2D>();
					lowerArm1Joint.anchor = lowerArm1JointPosition.localPosition;
					lowerArm1Joint.connectedAnchor = upperArm1.transform.InverseTransformPoint (lowerArm1JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (lowerArm1LowerAngle);
					limit.max = int.Parse (lowerArm1UpperAngle);
					lowerArm1Joint.limits = limit;
					lowerArm1Joint.useLimits = lowerArm1UseLimit;
				}
					

				if(lowerArm2 && upperArm2)
				{
					//add collider 
					if(!lowerArm2.GetComponent<Collider2D>())
						lowerArm2.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var lowerArm2Joint = lowerArm2.GetComponent<HingeJoint2D>()? lowerArm2.GetComponent<HingeJoint2D>() : lowerArm2.AddComponent<HingeJoint2D>();
					lowerArm2Joint.connectedBody = upperArm2.GetComponent<Rigidbody2D>();
					lowerArm2Joint.anchor = lowerArm2JointPosition.localPosition;
					lowerArm2Joint.connectedAnchor = upperArm2.transform.InverseTransformPoint (lowerArm2JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (lowerArm2LowerAngle);
					limit.max = int.Parse (lowerArm2UpperAngle);
					lowerArm2Joint.limits = limit;
					lowerArm2Joint.useLimits = lowerArm2UseLimit;
				}
				

				if(wrist1 && lowerArm1 && upperArm1)
				{
					//add collider 
					if(!wrist1.GetComponent<Collider2D>())
						wrist1.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var wrist1Joint = wrist1.GetComponent<HingeJoint2D>()? wrist1.GetComponent<HingeJoint2D>() : wrist1.AddComponent<HingeJoint2D>();
					wrist1Joint.connectedBody = lowerArm1.GetComponent<Rigidbody2D>();
					wrist1Joint.anchor = wrist1JointPosition.localPosition;
					wrist1Joint.connectedAnchor = lowerArm1.transform.InverseTransformPoint (wrist1JointPosition.position);
					
					//add lower and upper angle limits
					limit.min = int.Parse (wrist1LowerAngle);
					limit.max = int.Parse (wrist1UpperAngle);
					wrist1Joint.limits = limit;
					wrist1Joint.useLimits = wrist1UseLimit;
				}


				if(wrist2 && lowerArm2 && upperArm2)
				{
					//add collider 
					if(!wrist2.GetComponent<Collider2D>())
						wrist2.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var wrist2Joint = wrist2.GetComponent<HingeJoint2D>()? wrist2.GetComponent<HingeJoint2D>() : wrist2.AddComponent<HingeJoint2D>();
					wrist2Joint.connectedBody = lowerArm2.GetComponent<Rigidbody2D>();
					wrist2Joint.anchor = wrist2JointPosition.localPosition;
					wrist2Joint.connectedAnchor = lowerArm2.transform.InverseTransformPoint (wrist2JointPosition.position);
					
					//add lower and upper angle limits
					limit.min = int.Parse (wrist2LowerAngle);
					limit.max = int.Parse (wrist2UpperAngle);
					wrist2Joint.limits = limit;
					wrist2Joint.useLimits = wrist2UseLimit;
				}


				if(thigh1)
				{
					//add collider 
					if(!thigh1.GetComponent<Collider2D>())
						thigh1.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var thigh1Joint = thigh1.GetComponent<HingeJoint2D>()? thigh1.GetComponent<HingeJoint2D>() : thigh1.AddComponent<HingeJoint2D>();
					thigh1Joint.connectedBody = spine? spine.GetComponent<Rigidbody2D>() : body.GetComponent<Rigidbody2D>();
					thigh1Joint.anchor = thigh1JointPosition.localPosition;
					thigh1Joint.connectedAnchor = spine? spine.transform.InverseTransformPoint(thigh1JointPosition.position) : body.transform.InverseTransformPoint (thigh1JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (thigh1LowerAngle);
					limit.max = int.Parse (thigh1UpperAngle);
					thigh1Joint.limits = limit;
					thigh1Joint.useLimits = thigh1UseLimit;
				}
				

				if(thigh2)
				{
					//add collider 
					if(!thigh2.GetComponent<Collider2D>())
						thigh2.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var thigh2Joint = thigh2.GetComponent<HingeJoint2D>()? thigh2.GetComponent<HingeJoint2D>() : thigh2.AddComponent<HingeJoint2D>();
					thigh2Joint.connectedBody = spine? spine.GetComponent<Rigidbody2D>() : body.GetComponent<Rigidbody2D>();
					thigh2Joint.anchor = thigh2JointPosition.localPosition;
					thigh2Joint.connectedAnchor = spine? spine.transform.InverseTransformPoint(thigh2JointPosition.position) : body.transform.InverseTransformPoint (thigh2JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (thigh2LowerAngle);
					limit.max = int.Parse (thigh2UpperAngle);
					thigh2Joint.limits = limit;
					thigh2Joint.useLimits = thigh2UseLimit;
				}
				


				if(leg1 && thigh1)
				{
					//add collider 
					if(!leg1.GetComponent<Collider2D>())
						leg1.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var leg1Joint = leg1.GetComponent<HingeJoint2D>()? leg1.GetComponent<HingeJoint2D>() : leg1.AddComponent<HingeJoint2D>();
					leg1Joint.connectedBody = thigh1.GetComponent<Rigidbody2D>();
					leg1Joint.anchor = leg1JointPosition.localPosition;
					leg1Joint.connectedAnchor = thigh1.transform.InverseTransformPoint (leg1JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (leg1LowerAngle);
					limit.max = int.Parse (leg1UpperAngle);
					leg1Joint.limits = limit;
					leg1Joint.useLimits = leg1UseLimit;
				}
				



				if(leg2 && thigh2)
				{
					//add collider 
					if(!leg2.GetComponent<Collider2D>())
						leg2.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var leg2Joint = leg2.GetComponent<HingeJoint2D>()? leg2.GetComponent<HingeJoint2D>() : leg2.AddComponent<HingeJoint2D>();
					leg2Joint.connectedBody = thigh2.GetComponent<Rigidbody2D>();
					leg2Joint.anchor = leg2JointPosition.localPosition;
					leg2Joint.connectedAnchor = thigh2.transform.InverseTransformPoint (leg2JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (leg2LowerAngle);
					limit.max = int.Parse (leg2UpperAngle);
					leg2Joint.limits = limit;
					leg2Joint.useLimits = leg2UseLimit;
				}
				

				if(foot1 && leg1 && thigh1)
				{
					//add collider 
					if(!foot1.GetComponent<Collider2D>())
						foot1.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var foot1Joint = foot1.GetComponent<HingeJoint2D>()? foot1.GetComponent<HingeJoint2D>() : foot1.AddComponent<HingeJoint2D>();
					foot1Joint.connectedBody = leg1.GetComponent<Rigidbody2D>();
					foot1Joint.anchor = foot1JointPosition.localPosition;
					foot1Joint.connectedAnchor = leg1.transform.InverseTransformPoint (foot1JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (foot1LowerAngle);
					limit.max = int.Parse (foot1UpperAngle);
					foot1Joint.limits = limit;
					foot1Joint.useLimits = foot1UseLimit;
				}

				if(foot2 && leg2 && thigh2)
				{
					//add collider 
					if(!foot2.GetComponent<Collider2D>())
						foot2.AddComponent<PolygonCollider2D>();
					
					//add hinge joint 
					var foot2Joint = foot2.GetComponent<HingeJoint2D>()? foot2.GetComponent<HingeJoint2D>() : foot2.AddComponent<HingeJoint2D>();
					foot2Joint.connectedBody = leg2.GetComponent<Rigidbody2D>();
					foot2Joint.anchor = foot2JointPosition.localPosition;
					foot2Joint.connectedAnchor = leg2.transform.InverseTransformPoint (foot2JointPosition.position);

					//add lower and upper angle limits
					limit.min = int.Parse (foot2LowerAngle);
					limit.max = int.Parse (foot2UpperAngle);
					foot2Joint.limits = limit;
					foot2Joint.useLimits = foot2UseLimit;
				}

				helpText = "RAGDOLL WAS CREATED";
			}
			else
				helpText = "AT FIRST CREATE JOINT POSITIONS";
		}


		GUILayout.Space (5);

		//clears all assigned body parts for root object
		if(GUILayout.Button ("Reset", GUILayout.ExpandHeight(true), GUILayout.MinHeight (30), GUILayout.MaxHeight(50)))
		{
			showJointPositions = true;

			//clear body parts
			body = null;
			head = null;
			neck = null;
			spine = null;
			upperArm1 = null;
			upperArm2 = null;
			lowerArm1 = null;
			lowerArm2 = null;
			wrist1 = null;
			wrist2 = null;
			thigh1 = null;
			thigh2 = null;
			leg1 = null;
			leg2 = null;
			foot1 = null;
			foot2 = null;

			if(root)
			{
				var allParts = root.GetComponentsInChildren<Transform>();
				
				foreach(Transform child in allParts)
				{
					if(child.name == "Joint Position")
					{
						DestroyImmediate(child.gameObject);
					}
					else 
					{
						DestroyImmediate(child.GetComponent<Collider2D>());
						DestroyImmediate(child.GetComponent<HingeJoint2D>());
						DestroyImmediate(child.GetComponent<Rigidbody2D>());
					}
				}

				jointPositionsCreated = false;

				helpText = "RESETED";
			}
			else
				helpText = "ROOT OBJECT ISN'T ASSIGNED";
		}

		GUILayout.Space(10);
	}
}