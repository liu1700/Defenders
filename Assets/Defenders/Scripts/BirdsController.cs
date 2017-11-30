using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsController : MonoBehaviour
{

    /// <summary>
    /// This class will move the birds inside the game.
    /// </summary>

    [Range(1, 8)]
    public float birdSpeed = 5;
    private float speedRandomness;
    private Vector3 startingPosition;

    public GameObject featherFx;
    public AudioClip hitSfx;

    AudioSource source;

    void Start()
    {
        speedRandomness = Random.Range(1, 4);
        startingPosition = transform.position;
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Move the bird
        move();
    }

    void move()
    {
        if (startingPosition.x > 0)
        {
            transform.Translate(new Vector3(birdSpeed * speedRandomness * Time.deltaTime, 0, 0));
        }
        else
        {
            transform.Translate(new Vector3(-birdSpeed * speedRandomness * Time.deltaTime, 0, 0));
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (transform.position.x > startingPosition.x || transform.position.x < -startingPosition.x)
            Destroy(gameObject);
    }


    public void die()
    {

        //create particle fx
        GameObject f = Instantiate(featherFx, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        f.name = "FeatherFX";

        //make noise
        playSfx();

        ////if this is "BirdHunt" game mode, update counter
        //if (PlayerPrefs.GetInt ("GAMEMODE") == 2) {
        //	GameController.birdsHit += 1;
        //	GameController.addBonusTime ();
        //}
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        ////destroy
        //GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, 1f);

    }


    void playSfx()
    {
        source.clip = hitSfx;
        if (!source.isPlaying)
        {
            source.Play();
        }
    }
}
