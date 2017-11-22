using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    //***************************************************************************//
    // This class manages pause and unpause states.
    //***************************************************************************//

    public static bool isGamePaused;
    public static bool enableInput;

    private float savedTimeScale;
    public GameObject pausePlane;

    AdManager adManager;

    enum Status { PLAY, PAUSE }
    private Status currentStatus = Status.PLAY;


    //*****************************************************************************
    // Init.
    //*****************************************************************************
    void Awake()
    {

        isGamePaused = false;
        enableInput = true;

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.002f;

        if (pausePlane)
            pausePlane.SetActive(false);

        var AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
        if (AdManagerObject)
        {
            adManager = AdManagerObject.GetComponent<AdManager>();
        }
    }

    public void PauseGame()
    {

        print("Game is Paused...");
        enableInput = false;

        //show an Interstitial Ad when the game is paused
        if (adManager)
            adManager.showInterstitial();

        isGamePaused = true;
        //uiCam.GetComponent<Camera>().enabled = false;

        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        AudioListener.volume = 0;
        if (pausePlane)
            pausePlane.SetActive(true);
        currentStatus = Status.PAUSE;
    }


    public void UnPauseGame()
    {

        print("Unpause");
        isGamePaused = false;
        //uiCam.GetComponent<Camera>().enabled = true;

        StartCoroutine(reactiveInput());

        Time.timeScale = savedTimeScale;
        AudioListener.volume = 1.0f;
        if (pausePlane)
            pausePlane.SetActive(false);
        currentStatus = Status.PLAY;
    }


    IEnumerator reactiveInput()
    {
        yield return new WaitForSeconds(0.25f);
        enableInput = true;
    }

}