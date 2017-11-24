using System.Collections;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using google.service.game;
using UnityEngine;

public class GooglePlayManager : MonoBehaviour
{

    GoogleGame gg;
    public static bool loginOk;

    void Start()
    {
        gg = GoogleGame.Instance();

        gg.login(true, false);
        gg.gameEventHandler += onGameEvent;
        DontDestroyOnLoad(gameObject);
    }

    [Skip]
    void onGameEvent(int result_code, string eventName, string data)
    {
        Debug.Log(eventName + "-----------" + data);
        if (result_code == -1 && eventName == GameEvent.onConnectSuccess)
        {
            loginOk = true;
        }
    }

    public void ViewHighScore()
    {
        if (loginOk)
        {
            gg.showLeaderboard("CgkImdG3uvMOEAIQAQ");
        }
    }
}
