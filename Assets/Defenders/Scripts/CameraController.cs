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

    internal Vector3 cameraCurrentPos;

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
        GameController.gameIsStarted = true;
    }
}
