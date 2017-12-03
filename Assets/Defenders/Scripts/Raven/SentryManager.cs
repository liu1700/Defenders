using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryManager : MonoBehaviour
{

    Unity3DRavenCS.Unity3DRavenCS client;
    Dictionary<string, string> tags;

    string dsn = "https://7001b017ca0c4b3697c1d27d111cc1ae:77a804dfe1cd422f8eb04a9b9d4e49a6@sentry.io/251815";

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Create a new Unity3DRavenCS instance before using it.
        Unity3DRavenCS.Unity3DRavenCS.NewInstance(dsn);

        client = Unity3DRavenCS.Unity3DRavenCS.instance;

        // Create some tags that need to be sent with log messages.
        tags = new Dictionary<string, string>();
        tags.Add("Device-Model", SystemInfo.deviceModel);
        tags.Add("Device-Name", SystemInfo.deviceName);
        tags.Add("OS", SystemInfo.operatingSystem);
        tags.Add("MemorySize", SystemInfo.systemMemorySize.ToString());

        Application.logMessageReceived += LogHandler;

    }

    public void LogHandler(string condition, string stackTrace, LogType type)
    {
        if (client == null)
        {
            return;
        }

        if (type == LogType.Exception)
        {
            client.CaptureException(condition, stackTrace, tags);
        }
    }
}
