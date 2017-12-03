using System.Collections;
using CnControls;
using Anima2D;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// Main player controller class.
    /// This class is responsible for player inputs, rotation, health-management, shooting arrows and helper-dots creation.
    /// </summary>

    [Header("Public GamePlay settings")]
    public float playerHealth = 1000;                  //starting (full) health. can be edited.
    private float minShootDistance = 0.05f;                 //powers lesser than this amount are ignored. (used to cancel shoots)
    internal float playerCurrentHealth;               //real-time health. not editable.
    public static bool isPlayerDead;                //flag for gameover event
    public int trajectoryPointCnt = 10;
    public float trajectoryPredictionDur = 0.4f;

    [Header("Linked GameObjects")]
    //Reference to game objects (childs and prefabs)
    public GameObject arrow;
    public GameObject trajectoryHelper;
    public Control playerTurnPivot, hand;
    public GameObject playerShootPosition;
    public GameObject topAnchor, lowerAnchor, handAnchor;
    public GameObject minPos, maxPos;
    public SimpleJoystick simpleJoystick;
    public Image cdImage;

    Timer weaponeTimer; // cd 计时器
    float unlockDuration = 0.6f;
    Rigidbody2D arrowRigid;


    [Header("Audio Clips")]
    public AudioClip[] shootSfx;
    public AudioClip[] hitSfx;
    public AudioClip[] hitTowerSfx;

    //private settings
    //private Vector2 icp;                            //initial Click Position
    private Ray inputRay;
    private RaycastHit hitInfo;
    //private float inputPosX;
    private float preInputPosX;
    //private float inputPosY;
    private Vector2 inputDirection;
    private float distanceFromFirstClick;
    private float shootPower;
    private float shootDirection;
    private Vector3 shootDirectionVector;

    public Text hpText;
    public Slider hpSlider;

    GameController gc;
    bool needUpdateHealth;
    //bool helperDelayIsDone, canCreateHelper;
    bool canCreateHelper;
    bool canShoot;

    //float helperCreationDelay = 0.2f, helperShowDelay = 0.2f;
    //float helperShowTimer;

    float inputH, inputV;

    string tapKey = "Fire1";
    string hKey = "Horizontal", vKey = "Vertical";

    LineRenderer bowString;
    Vector3 handSourceAnchor, topSourceAnchor, downSourceAnchor;
    float distance, powerPercent;

    private List<GameObject> trajectoryPoints;

    AudioSource audioSource;

    /// <summary>
    /// Init
    /// </summary>
    void Awake()
    {
        //icp = new Vector2(0, 0);
        //infoPanel.SetActive(false);
        shootDirectionVector = new Vector3(0, 0, 0);
        playerCurrentHealth = playerHealth;
        needUpdateHealth = true;
        isPlayerDead = false;
        //helperDelayIsDone = false;
        //canCreateHelper = true;
        inputDirection = new Vector2();
        var g = GameObject.FindGameObjectWithTag("GameController");
        gc = g.GetComponent<GameController>();
        bowString = hand.gameObject.GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        distance = Vector2.Distance(minPos.transform.position, maxPos.transform.position);

        canShoot = true;
        arrowRigid = arrow.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateBowString();
        weaponeTimer = Timer.createTimer("weaponCD");

        trajectoryPoints = new List<GameObject>();
        for (int i = 0; i < trajectoryPointCnt; i++)
        {
            GameObject t = Instantiate(trajectoryHelper, playerShootPosition.transform.position, Quaternion.identity, playerShootPosition.transform) as GameObject;
            t.SetActive(false);
            t.transform.localScale = new Vector3(t.transform.localScale.x - (0.02f * i), t.transform.localScale.y - (0.02f * i), t.transform.localScale.z);
            trajectoryPoints.Add(t);
        }
    }

    void UpdateBowString()
    {
        bowString.SetPosition(0, topAnchor.transform.position);
        bowString.SetPosition(1, handAnchor.transform.position);
        bowString.SetPosition(2, lowerAnchor.transform.position);
    }

    /// <summary>
    /// FSM
    /// </summary>
    void Update()
    {
        UpdateBowString();

        //if the game has not started yet, or the game is finished, just return
        if (!GameController.gameIsStarted || GameController.gameIsFinished)
            return;

        if (needUpdateHealth)
        {
            UpdateHp();
        }

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

        if (!canShoot)
        {
            return;
        }

        //Player pivot turn manager
        if (CnInputManager.GetButton(tapKey))
        {
            turnPlayerBody();
            //helperShowTimer += Time.deltaTime;
            //if (helperShowTimer >= helperShowDelay)
            //{
            //    helperDelayIsDone = true;
            //}
        }

        //register the initial Click Position
        if (CnInputManager.GetButtonDown(tapKey))
        {
            //icp = new Vector2(inputPosX, inputPosY);
            switchTrajectoryHelper(true);
        }

        //clear the initial Click Position
        if (CnInputManager.GetButtonUp(tapKey))
        {
            switchTrajectoryHelper(false);
            //only shoot if there is enough power applied to the shoot
            if (distanceFromFirstClick >= minShootDistance && gc.AddGold(-1))
            {
                shootArrow();
            }
            else
            {
                //reset body rotation
                StartCoroutine(resetBodyRotation());
            }

            //reset variables
            //icp = new Vector2(0, 0);
            //helperDelayIsDone = false;
            //helperShowTimer = 0;

            hand.transform.position = minPos.transform.position;
        }
    }

    public void RefillPlayerHealth()
    {
        playerCurrentHealth = playerHealth;
        isPlayerDead = false;
        needUpdateHealth = true;
    }

    public void UpdateHp()
    {
        hpText.text = (((playerCurrentHealth / playerHealth) * 100).ToString()) + "%";
        hpSlider.value = 1f - (playerCurrentHealth / playerHealth);
        needUpdateHealth = false;
    }

    public void AddHealth(int hpVal)
    {
        playerCurrentHealth += hpVal;
        if (playerCurrentHealth < 0)
        {
            playerCurrentHealth = 0;
        }
        needUpdateHealth = true;
    }

    public float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// When player is aiming, we need to turn the body of the player based on the angle of icp and current input position
    /// </summary>
    void turnPlayerBody()
    {
        inputH = CnInputManager.GetAxisRaw(hKey);
        inputV = CnInputManager.GetAxisRaw(vKey);

        if (inputH > 0)
        {
            inputH = 0;
        }

        inputDirection.Set(inputH, inputV);
        shootDirection = 360 - Angle(inputDirection);

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
        simpleJoystick.RotateFakeBtn(Quaternion.Euler(0, 0, shootDirection - 90));
        //calculate shoot power
        distanceFromFirstClick = Vector2.Distance(Vector2.zero, inputDirection);
        powerPercent = Mathf.Clamp(distanceFromFirstClick, 0, 1);
        shootPower = powerPercent * 45;

        hand.transform.position = minPos.transform.TransformPoint(minPos.transform.localPosition.x - (distance * powerPercent), minPos.transform.localPosition.y, 0);
        //if (distanceFromFirstClick >= minShootDistance && helperDelayIsDone)
        //if (distanceFromFirstClick >= minShootDistance)
        //{
        //    StartCoroutine(shootTrajectoryHelper());
        //}

        shootTrajectoryHelper();

        //if (canCreateHelper)
        //{
        //    shootTrajectoryHelper();
        //}
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
        launcherCtrl.playerShootVector = ((shootDirectionVector * -1) * shootPower);

        //reset body rotation
        StartCoroutine(resetBodyRotation());

        cdImage.fillAmount = 0f;
        cdImage.gameObject.SetActive(true);
        weaponeTimer.startTiming(unlockDuration, onComplete, Unlocking, true, false, false);
        canShoot = false;
    }

    void Unlocking(float p)
    {
        cdImage.fillAmount = p;
    }

    void onComplete()
    {
        cdImage.gameObject.SetActive(false);
        canShoot = true;
    }

    void shootTrajectoryHelper()
    {
        var fkTime = trajectoryPredictionDur;
        shootDirectionVector = Vector3.Normalize(inputDirection);
        Vector2 pos;
        Vector3 shootVector;
        float sqrT;
        for (int i = 0; i < trajectoryPointCnt; i++)
        {
            pos = new Vector2(playerShootPosition.transform.position.x, playerShootPosition.transform.position.y);
            shootVector = (shootDirectionVector * -1) * (shootPower / fkTime);
            sqrT = (fkTime * fkTime);
            pos.y = pos.y + sqrT * ((shootVector.y / arrowRigid.mass) + ((Physics2D.gravity.y * arrowRigid.gravityScale) / 2f));
            pos.x = pos.x + sqrT * (shootVector.x / arrowRigid.mass);

            trajectoryPoints[i].transform.position = pos;
            fkTime += trajectoryPredictionDur;
        }
        return;
    }

    void switchTrajectoryHelper(bool isOpen)
    {
        canCreateHelper = isOpen;
        for (int i = 0; i < trajectoryPointCnt; i++)
        {
            trajectoryPoints[i].SetActive(isOpen);
        }
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
        audioSource.clip = _clip;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }


    /// <summary>
    /// Play a sfx when player is hit by an arrow
    /// </summary>
    public void playRandomHitSound()
    {
        playSfx(hitSfx[Random.Range(0, hitSfx.Length)]);
    }

    public void playRandomHitTowerSound()
    {
        if (hitTowerSfx.Length > 0)
        {
            playSfx(hitTowerSfx[Random.Range(0, hitTowerSfx.Length)]);
        }
    }

}

