using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    /// <summary>
    /// Main game controller class.
    /// Game controller is responsible for assigning turns to player and opponent (thus making this game a turn-based one!), 
    /// setting ground types (to curved or flat types), managing UI elements including health bars and info panels, managing player inputs (on UI), 
    /// checking for gameover events and post gameover settings.
    /// </summary>

    //[Header("Background Type")]
    public enum groundTypes { flat, curved }
    public groundTypes groundType = groundTypes.flat;   //we have two options here. default is flat ground.
    public GameObject flatBg;                           //reference to flag ground object.
    public EnemyPool enemies;
    public LevelUI levelUI;
    public GameOverManager gameOverManager;
    public PauseManager pauseManager;

    // Static variables //
    public static int gameMode;                     //current game mode
    public static bool isArrowInScene;              //have any body shot an arrow? (is there an arrow inside the game?)
    public static bool gameIsStarted;               //global flag for game start state
    public static bool gameIsFinished;              //global flag for game finish state
    public static bool noMoreShooting;              //We use this to stop the shoots when someone has been killed but the game is yet to finish
    public static int round;                        //internal counter to assign turn to player and AI
    int playerCoins;                  //available player coins
    int addedPlayerCoins;                  //available player coins
    int playerKilled;                  //available player coins
    public static int playerArrowShot;              //how many arrows player shot in this game
                                                    // Static variables //

    public static int reviveUseGold = 100;

    // Private vars //
    private bool canTap;
    AdManager admgr;

    [Header("AudioClips")]
    public AudioClip tapSfx;
    public AudioClip endSfx;


    [Header("Public GameObjects")]
    //Reference to scene game objects		
    private GameObject player;
    private GameObject enemy;
    private GameObject cam;
    private GameObject uiCam;

    ///Game timer vars
    public int availableTime = 30;                  //total gameplay time
    public static int bonusTime = 3;                        //additional time when we hit a bird
    public static float gameTimer;
    private string remainingTime;
    private int seconds;
    private int minutes;

    int reviveTime = 20;

    private float playerHealthScale;                //player health bar real-time scale
    private float enemyHealthScale;                 //enemy health bar real-time scale

    // collision layer
    public static int enemyLayer;
    public static int environmentLayer;
    public static int playerLayer;

    int maxKilled;
    PlayerController playerController;

    bool rewardOk;
    /// <summary>
    /// INIT
    /// </summary>
    void Awake()
    {

        //get game mode
        gameMode = PlayerPrefs.GetInt("GAMEMODE");

        // JUST TO PREVENT BUGS WHEN LOADING GAME MODES DIRECTLY FROM EDITOR
        // -- REMEMBER: You need to always start the game from the menu or init scenes --
        if (SceneManager.GetActiveScene().name == "Game" && gameMode == 2)
        {
            //This is bad
            print("You need to run this game from menu scene.");
            SceneManager.LoadScene("Menu");
        }

        if (SceneManager.GetActiveScene().name == "BirdHunt" && gameMode == 1)
        {
            //This is bad
            print("You need to run this game from menu scene.");
            SceneManager.LoadScene("Menu");

        }

        //set ground type with high priority
        switch (groundType)
        {
            case groundTypes.flat:
                flatBg.SetActive(true);
                break;
            case groundTypes.curved:
                flatBg.SetActive(false);
                break;
        }

        //cache main objects
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("enemy");
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        playerController = player.GetComponent<PlayerController>();

        isArrowInScene = false;

        canTap = true;
        gameIsStarted = false;
        gameIsFinished = false;
        noMoreShooting = false;
        round = 0;
        playerArrowShot = 0;
        playerCoins = PlayerPrefs.GetInt("PlayerCoins");
        playerKilled = 0;

        gameTimer = availableTime;
        seconds = 0;
        minutes = 0;

        var AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
        if (AdManagerObject != null)
        {
            admgr = AdManagerObject.GetComponent<AdManager>();
        }

        var ctrl = cam.GetComponent<CameraController>();
        ctrl.SetCameraProjectionSize(15f);
        ctrl.SetcameraCurrentPos(new Vector3(3, 9, -10));

        enemyLayer = LayerMask.NameToLayer("enemy");
        environmentLayer = LayerMask.NameToLayer("environment");
        playerLayer = LayerMask.NameToLayer("player");
    }

    void Start()
    {
        StartCoroutine(activateTap());
        levelUI.goldNum.text = playerCoins.ToString();
    }

    private void Update()
    {
        //we no longer need to loop into gameController if the game is already finished.
        if (gameIsFinished)
            return;

        manageGameTimer();

        if (rewardOk)
        {
            gameTimer = reviveTime + Time.timeSinceLevelLoad;
            gameOverManager.gameObject.SetActive(false);
            rewardOk = false;
        }

        //fast game finish checking...
        if (enemies.AllEnemiesDead())
        {
            //next turn
            nextTurn();
        }
        else if (PlayerController.isPlayerDead)
        {
            //we have lost
            StartCoroutine(finishTheGame(0));
        }

        // DEBUG COMMANDS //
        //Force restart
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //Fake damage to player
        if (Input.GetKeyUp(KeyCode.D))
        {
            playerController.AddHealth(-10);
        }
        //Fake damage to enemy
        if (Input.GetKeyUp(KeyCode.E))
        {
            enemy.GetComponent<EnemyController>().enemyCurrentHealth -= 10;
        }
    }

    public void TapReduceGold()
    {
        if (!canTap)
            return;

        if (!AddGold(-reviveUseGold))
        {
            return;
        }

        playSfx(tapSfx);                            //play touch sound
        canTap = false;                             //prevent double touch
        StartCoroutine(waitAnimation());
        OnPlayerReviveOver();
        StartCoroutine(activateTap());
    }

    public void TapToMenu()
    {
        if (!canTap)
            return;

        playSfx(tapSfx);                            //play touch sound
        canTap = false;                             //prevent double touch
        StartCoroutine(waitAnimation());

        //show an Interstitial Ad when the game is paused
        if (admgr)
            admgr.showInterstitial();
        SceneManager.LoadScene("Menu");
    }

    public void TapToRetry()
    {
        if (!canTap)
            return;

        playSfx(tapSfx);                            //play touch sound
        canTap = false;                             //prevent double touch
        StartCoroutine(waitAnimation());

        //show an Interstitial Ad when the game is paused
        if (admgr)
            admgr.showInterstitial();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TapViewVideo(int typ)
    {
        if (!canTap)
            return;

        playSfx(tapSfx);                            //play touch sound
        canTap = false;                             //prevent double touch
        StartCoroutine(waitAnimation());

        if (admgr)
        {
            if (typ == 1)
            {
                admgr.rewardCB = OnPlayerReviveOver;
                admgr.showRewardVideo();
            }
            else if (typ == 2)
            {
                admgr.showRewardVideo();

                if (addedPlayerCoins > 0 && playerCoins > 0)
                {
                    AddGoldInstant(addedPlayerCoins + playerCoins);
                }

                SceneManager.LoadScene("Menu");
            }
        }

        StartCoroutine(activateTap());
    }

    public void OnPlayerReviveOver()
    {
        playerController.RefillPlayerHealth();
        reviveFinished();
        rewardOk = true;
    }

    public void OnBackToMain()
    {
        if (admgr)
        {
            admgr.showInterstitial();
        }
    }

    IEnumerator waitAnimation()
    {
        yield return new WaitForSeconds(1.2f);      //Wait for the animation to end
    }

    /// <summary>
    /// Assign turns to player and AI.
    /// </summary>
    public IEnumerator roundTurnManager()
    {

        //1. first check if the game is already finished
        if (gameIsFinished)
        {
            yield break;
        }

        //2. then check if the situation meets a game over
        //check for game finish state
        if (enemies.AllEnemiesDead())
        {

            //next turn
            nextTurn();
            yield break;

        }
        else if (PlayerController.isPlayerDead)
        {

            //we have lost
            StartCoroutine(finishTheGame(0));
            yield break;

        }

    }


    /// <summary>
    /// Next turn
    /// </summary>
    void nextTurn()
    {
        round++;
        enemies.ReGenerateEnemies(round);
    }

    void reviveFinished()
    {
        gameIsFinished = false;
    }

    /// <summary>
    /// Gameover sequence.
    /// </summary>
    IEnumerator finishTheGame(int res)
    {

        //finish the game
        gameIsFinished = true;
        print("Game Is Finished");

        //play sfx
        playSfx(endSfx);

        //wait a little
        yield return new WaitForSeconds(1.0f);

        gameOverManager.gameObject.SetActive(true);

        //Save new coin amount
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);

        maxKilled = PlayerPrefs.GetInt("PlayerKilled", 0);
        if (playerKilled > maxKilled)
        {
            maxKilled = playerKilled;
            PlayerPrefs.SetInt("PlayerKilled", playerKilled);
        }

        gameOverManager.killText.text = playerKilled.ToString();
        gameOverManager.bestText.text = maxKilled.ToString();
        gameOverManager.addGoldText.text = "+" + addedPlayerCoins.ToString();
        gameOverManager.ActivatePanel(playerCoins);
    }

    /// <summary>
    /// enable touch commands again
    /// </summary>
    IEnumerator activateTap()
    {
        yield return new WaitForSeconds(1.0f);
        canTap = true;
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
    /// Game timer manager
    /// </summary>
    void manageGameTimer()
    {
        if (gameIsFinished)
            return;

        seconds = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) % 60;
        minutes = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) / 60;

        if (seconds == 0 && minutes == 0)
        {
            StartCoroutine(finishTheGame(0));
        }

        remainingTime = string.Format("{0:00} : {1:00}", minutes, seconds);
        levelUI.levelTime.text = remainingTime;
    }


    /// <summary>
    /// Adds the bonus time.
    /// </summary>
    public void addBonusTime(int bounus)
    {
        gameTimer += bounus;

        var s = Mathf.CeilToInt(bounus) % 60;
        var m = Mathf.CeilToInt(bounus) / 60;

        var addTime = string.Format("+{0:00} : {1:00}", m, s);
        levelUI.performAddTimeAnim(addTime);
    }

    public bool AddGold(int count)
    {
        if (playerCoins + count < 0)
        {
            return false;
        }

        playerCoins += count;
        if (count > 0)
        {
            addedPlayerCoins += count;
        }
        levelUI.performAddGoldAnim(count);
        levelUI.goldNum.text = playerCoins.ToString();

        return true;
    }

    public void AddGoldInstant(int count)
    {
        PlayerPrefs.SetInt("PlayerCoins", count);
        PlayerPrefs.Save();
    }

    public void KillEnemies(int count, int bounus)
    {
        playerKilled += count;
        addBonusTime(bounus - 2);
        AddGold(bounus);
    }

    public void OnClickPause()
    {
        if (PauseManager.isGamePaused)
        {
            return;
        }
        pauseManager.PauseGame();
    }

    public void OnClickUnPause()
    {
        pauseManager.UnPauseGame();
    }

    public void OnClickBackToMenu()
    {
        pauseManager.UnPauseGame();

        if (playerCoins < 0)
        {
            playerCoins = 0;
        }

        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Menu");
    }
}
