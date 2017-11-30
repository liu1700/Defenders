using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryManager : MonoBehaviour
{

    Unity3DRavenCS.Unity3DRavenCS client;
    Dictionary<string, string> tags;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Create a new Unity3DRavenCS instance before using it.
        Unity3DRavenCS.Unity3DRavenCS.NewInstance("https://d51acc7af2274dbcadbc0fa9ca7b43ff:09eac5f5288b4566acce95bcbc123f85@sentry.io/252613");

        client = Unity3DRavenCS.Unity3DRavenCS.instance;

        // Create some tags that need to be sent with log messages.
        tags = new Dictionary<string, string>();
        tags.Add("Device-Model", SystemInfo.deviceModel);
        tags.Add("Device-Name", SystemInfo.deviceName);
        tags.Add("OS", SystemInfo.operatingSystem);
        tags.Add("MemorySize", SystemInfo.systemMemorySize.ToString());
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogHandler;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogHandler;
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
