using System.Collections;
using System.Collections.Generic;
using Anima2D;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// Main player controller class.
    /// This class is responsible for player inputs, rotation, health-management, shooting arrows and helper-dots creation.
    /// </summary>

    [Header("Public GamePlay settings")]
    public bool useHelper = true;                   //use helper dots when player is aiming to shoot
    public int baseShootPower;                 //base power. edit with care.
    public int playerHealth = 100;                  //starting (full) health. can be edited.
    private int minShootPower = 15;                 //powers lesser than this amount are ignored. (used to cancel shoots)
    internal int playerCurrentHealth;               //real-time health. not editable.
    public static bool isPlayerDead;                //flag for gameover event

    [Header("Linked GameObjects")]
    //Reference to game objects (childs and prefabs)
    public GameObject arrow;
    public GameObject trajectoryHelper;
    public Control playerTurnPivot;
    public GameObject playerShootPosition;

    [Header("Audio Clips")]
    public AudioClip[] shootSfx;
    public AudioClip[] hitSfx;


    //private settings
    private Vector2 icp;                            //initial Click Position
    private Ray inputRay;
    private RaycastHit hitInfo;
    private float inputPosX;
    private float inputPosY;
    private Vector2 inputDirection;
    private float distanceFromFirstClick;
    private float shootPower;
    private float shootDirection;
    private Vector3 shootDirectionVector;

    //helper trajectory variables
    private float helperShowDelay = 0.2f;
    private float helperShowTimer;

    public Text hpText;


    /// <summary>
    /// Init
    /// </summary>
    void Awake()
    {
        icp = new Vector2(0, 0);
        //infoPanel.SetActive(false);
        shootDirectionVector = new Vector3(0, 0, 0);
        playerCurrentHealth = playerHealth;
        UpdateHp(playerHealth);

        isPlayerDead = false;

        //canCreateHelper = true;
        helperShowTimer = 0;
    }


    /// <summary>
    /// FSM
    /// </summary>
    void Update()
    {

        //if the game has not started yet, or the game is finished, just return
        if (!GameController.gameIsStarted || GameController.gameIsFinished)
            return;

        //Check if this object is dead or alive
        if (playerCurrentHealth <= 0)
        {
            print("Player is dead...");
            playerCurrentHealth = 0;
            isPlayerDead = true;
            return;
        }

        if (!PauseManager.enableInput)
            return;

        //Player pivot turn manager
        if (Input.GetMouseButton(0))
        {

            turnPlayerBody();

        }

        //register the initial Click Position
        if (Input.GetMouseButtonDown(0))
        {
            icp = new Vector2(inputPosX, inputPosY);
        }

        //clear the initial Click Position
        if (Input.GetMouseButtonUp(0))
        {

            //only shoot if there is enough power applied to the shoot
            if (shootPower >= minShootPower)
            {
                shootArrow();
            }
            else
            {
                //reset body rotation
                StartCoroutine(resetBodyRotation());
            }

            //reset variables
            icp = new Vector2(0, 0);
            helperShowTimer = 0;
        }
    }

    public void RefillPlayerHealth()
    {
        playerCurrentHealth = playerHealth;
        UpdateHp(playerCurrentHealth);
        isPlayerDead = false;
    }

    void UpdateHp(int hp)
    {
        hpText.text = hp.ToString();
    }

    public void AddHealth(int hpVal)
    {
        playerCurrentHealth += hpVal;
        if (playerCurrentHealth < 0)
        {
            playerCurrentHealth = 0;
        }
        UpdateHp(playerCurrentHealth);
    }

    /// <summary>
    /// When player is aiming, we need to turn the body of the player based on the angle of icp and current input position
    /// </summary>
    void turnPlayerBody()
    {

        inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out hitInfo, 50))
        {
            // determine the position on the screen
            inputPosX = this.hitInfo.point.x;
            inputPosY = this.hitInfo.point.y;

            // set the bow's angle to the arrow
            inputDirection = new Vector2(icp.x - inputPosX, icp.y - inputPosY);

            shootDirection = (Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg) + 90;
            if (shootDirection > 180)
            {
                shootDirection -= 180;
                if (shootDirection <= 90)
                {
                    shootDirection = 180;
                }
                else
                {
                    shootDirection = 0;
                }
            }

            if (shootDirection < 0)
            {
                shootDirection = 0;
            }

            //apply the rotation
            playerTurnPivot.bone.transform.rotation = Quaternion.Euler(0, 0, shootDirection);

            //calculate shoot power
            distanceFromFirstClick = inputDirection.magnitude / 4;
            shootPower = Mathf.Clamp(distanceFromFirstClick, 0, 1) * 1200;
        }
    }


    /// <summary>
    /// Shoot sequence.
    /// For the player controller, we just need to instantiate the arrow object, apply the shoot power to it, and watch is fly.
    /// There is no AI involved with player arrows. It just fly based on the initial power and angle.
    /// </summary>
    void shootArrow()
    {

        //set the unique flag for arrow in scene.
        GameController.isArrowInScene = true;

        //play shoot sound
        playSfx(shootSfx[Random.Range(0, shootSfx.Length)]);

        //add to shoot counter
        GameController.playerArrowShot++;

        GameObject arr = Instantiate(arrow, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1)) as GameObject;
        arr.name = "PlayerProjectile";
        var launcherCtrl = arr.GetComponent<MainLauncherController>();
        launcherCtrl.ownerID = 0;

        shootDirectionVector = Vector3.Normalize(inputDirection);
        launcherCtrl.playerShootVector = shootDirectionVector * ((shootPower + baseShootPower) / 50);

        //reset body rotation
        StartCoroutine(resetBodyRotation());
    }


    /// <summary>
    /// tunr player body to default rotation
    /// </summary>
    IEnumerator resetBodyRotation()
    {

        yield return new WaitForSeconds(0.25f);
        float currentRotationAngle = playerTurnPivot.transform.eulerAngles.z;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 3;
            playerTurnPivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(currentRotationAngle, 90, t));
            yield return 0;
        }
    }


    /// <summary>
    /// Plays the sfx.
    /// </summary>
    void playSfx(AudioClip _clip)
    {
        GetComponent<AudioSource>().clip = _clip;
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }


    /// <summary>
    /// Play a sfx when player is hit by an arrow
    /// </summary>
    public void playRandomHitSound()
    {
        int rndIndex = Random.Range(0, hitSfx.Length);
        playSfx(hitSfx[rndIndex]);
    }

}

