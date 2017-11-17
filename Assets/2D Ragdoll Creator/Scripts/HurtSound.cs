using UnityEngine;
using System.Collections;

public class HurtSound : MonoBehaviour {
	public AudioClip[] hurtSounds;
	public float hitVelocityForHurtSound = 100;

	private AudioSource audioSource;


	void Start()
	{
		//if parent object doesn't have AudioSource component, add it
		if(!transform.parent.gameObject.GetComponent<AudioSource>())
			audioSource = transform.parent.gameObject.AddComponent<AudioSource>();
		else
			audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
	}


	//this function is called when collision occurs
	void OnCollisionEnter2D(Collision2D col)
	{
		//if this object is colliding with object which is tagged as "Obstacle"
		if(col.gameObject.tag == "Obstacle")
		{
			//if relative linear velocity of the two colliding objects is more than hitVelocityForHurtSound (set from inspector window)
			if(col.relativeVelocity.sqrMagnitude > hitVelocityForHurtSound)
			{
				//calls function below
				PlaySound();
			} 
		}
	}

	public void PlaySound()
	{
		//if sound isn't playing
		if (!audioSource.isPlaying)
		{
			//play random sound from hurtSounds array (set from inspector window)
			audioSource.clip = hurtSounds[Random.Range (0, hurtSounds.Length)];
			audioSource.Play ();
		}
	}
}
