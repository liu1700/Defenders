using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    /// <summary>
    /// Main Menu Controller.
    /// This class handles all touch events on menu buttons.
    /// </summary>

    /// <summary>
    /// Available game modes
    /// 1 = normal player vs computer
    /// 2 = bird hunting
    /// ...
    /// </summary>
    public int gameMode = 1;

    public AudioClip tapSfx;                    //tap sound for buttons click

    //public GameObject coinLabel;                //coin text on menu scene
    public Text coinText;
    private GameObject cam;                         //main camera

    public GameObject featurePanel;
    public GameObject equipmentPanel;
    //AdManager adManager;

    void Awake()
    {

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        featurePanel.GetComponent<CanvasRenderer>().SetAlpha(0f);
        Application.targetFrameRate = 30;

        //var AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
        //if (AdManagerObject != null)
        //{
        //    adManager = AdManagerObject.GetComponent<AdManager>();
        //}
    }

    void Start()
    {
        var ctrl = cam.GetComponent<CameraController>();
        ctrl.SetCameraProjectionSize(6f);
        ctrl.SetcameraCurrentPos(new Vector3(0, 0, -10));

        //show avilable player coins in menu scene
        // 如果coin為0則默認給100
        var coin = PlayerPrefs.GetInt("PlayerCoins", 0);
        if (coin <= 0)
        {
            coin = 100;
            PlayerPrefs.SetInt("PlayerCoins", coin);
            PlayerPrefs.Save();
        }
        coinText.text = coin.ToString();

        equipmentPanel.SetActive(false);

        //// preload ads
        //if (adManager)
        //{
        //    adManager.loadInterstitial();
        //    adManager.loadReward();
        //}
    }

    public void OnClickOpenEquipmentPanel()
    {
        equipmentPanel.SetActive(!equipmentPanel.activeSelf);
    }

    public void OnClickFeaturePanel()
    {
        if (featurePanel.activeInHierarchy)
        {
            featurePanel.GetComponent<Image>().CrossFadeAlpha(0, .25f, false);
            StartCoroutine(fadePanel());
        }
        else
        {
            featurePanel.SetActive(true);
            featurePanel.GetComponent<Image>().CrossFadeAlpha(1f, .25f, false);
        }
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    IEnumerator fadePanel()
    {
        yield return new WaitForSeconds(.25f);      //Wait for the animation to end
        featurePanel.SetActive(false);
        featurePanel.GetComponent<CanvasRenderer>().SetAlpha(0f);
    }

    //*****************************************************************************
    // This function monitors player touches on menu buttons.
    // detects both touch and clicks and can be used with editor, handheld device and 
    // every other platforms at once.
    //*****************************************************************************
    public void PressStart()
    {
        gameMode = 1;
        PlayerPrefs.SetInt("GAMEMODE", gameMode);
        playSfx(tapSfx);                            //play touch sound
        StartCoroutine(pressStart());
    }

    IEnumerator pressStart()
    {
        yield return new WaitForSeconds(.5f);      //Wait for the animation to end
        SceneManager.LoadScene("Game");             //Load the next scene
    }

    //*****************************************************************************
    // Play sound clips
    //*****************************************************************************
    void playSfx(AudioClip _clip)
    {
        GetComponent<AudioSource>().clip = _clip;
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }

}