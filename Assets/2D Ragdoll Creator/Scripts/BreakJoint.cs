using UnityEngine;
using System.Collections;

public class BreakJoint : MonoBehaviour {
	public float velocityForBrake = 3000;
	public float angularVelocityForBrake = 2500;
	public GameObject blood;
	
	private HingeJoint2D joint;
	private Rigidbody2D rb;


	// Use this for initialization
	void Start () {
		//get hingejoint2D and rigidbody2D components from object on which this script is assigned
		joint = GetComponent<HingeJoint2D>();
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(rb.velocity.sqrMagnitude > velocityForBrake || rb.angularVelocity > angularVelocityForBrake)
		{
			//disable joint
			joint.enabled = false;

			//instantiate blood
			var bloodObject = Instantiate (blood, transform.TransformPoint(GetComponent<HingeJoint2D>().anchor), Quaternion.identity) as GameObject;
			Destroy (bloodObject,1);

			//play hurt sound
			if(GetComponent<HurtSound>())
				GetComponent<HurtSound>().PlaySound();
		}
	}
}
