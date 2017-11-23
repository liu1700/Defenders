using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    /// <summary>
    /// Main camera manager. Handles camera movement, smooth follow, starting demo, and limiters.
    /// Note that this is the game-play camera. All UI rendering is done by UICamera in another thread.
    /// </summary>

    float cps = 15;           //camera's projection size

    public bool performStartMove = true;    //should camera moves towards enemy and back to player, when we just start the game?1
    internal bool startMoveIsDoneFlag;      //flag to set when the starting animation has been performed

    internal Vector3 cameraCurrentPos;

    AudioSource bgm;

    void Awake()
    {

        //First of all, check if there is an enemy in this game mode.
        if (!GameModeController.isEnemyRequired())
        {
            performStartMove = false;
        }

        startMoveIsDoneFlag = false;

        bgm = GetComponent<AudioSource>();
        bgm.volume = 0;
    }

    public void SetCameraProjectionSize(float s)
    {
        cps = s;
        GetComponent<Camera>().orthographicSize = cps;
    }

    public void SetcameraCurrentPos(Vector3 pos)
    {
        cameraCurrentPos = pos;
        transform.position = pos;
    }


    void Start()
    {
        //no need to perform the demo. ativate the game immediately.
        startMoveIsDoneFlag = true;
        GameController.gameIsStarted = true;

        StartCoroutine(fadeInBgm());
    }

    IEnumerator fadeInBgm()
    {
        var t = 0.0f;
        while (t < 0.8)
        {
            t += Time.deltaTime;
            bgm.volume = t;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0);
    }


    /// <summary>
    /// Smooth follow the target object.
    /// </summary>
    [Range(0, 0.5f)]
    public float followSpeedDelay = 0.1f;
    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    IEnumerator smoothFollow(Vector3 p)
    {

        //Only follow weapons as target if there is an enemy in this game mode!
        if (!GameModeController.isEnemyRequired())
        {
            yield break;
        }

        float posX = Mathf.SmoothDamp(transform.position.x, p.x, ref xVelocity, followSpeedDelay);
        float posY = Mathf.SmoothDamp(transform.position.y, p.y - 2, ref yVelocity, followSpeedDelay);
        transform.position = new Vector3(posX, posY, transform.position.z);

        //always save camera's current pos in an external variable, for later use
        cameraCurrentPos = transform.position;

        yield return 0;
    }

    /// <summary>
    /// move the camera to a given position
    /// </summary>
    /// <returns>The to position.</returns>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="speed">Speed.</param>
    public IEnumerator goToPosition(Vector3 from, Vector3 to, float speed)
    {

        //Only change position if there is an enemy in this game mode!
        if (!GameModeController.isEnemyRequired())
        {
            yield break;
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.position = new Vector3(Mathf.SmoothStep(from.x, to.x, t),
                                                Mathf.SmoothStep(from.y, to.y, t),
                                                transform.position.z);
            yield return 0;
        }

        if (t >= 1)
        {
            //always save camera's current pos in an external variable, for later use
            cameraCurrentPos = transform.position;
        }
    }


}
