using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CompleteEvent();
public delegate void UpdateEvent(float t);

public class Timer : MonoBehaviour
{
    bool isLog = true;

    UpdateEvent updateEvent;

    CompleteEvent onCompleted;

    float timeTarget;   // 计时时间/  

    float timeStart;    // 开始计时时间/  

    float timeNow;     // 现在时间/  

    float offsetTime;   // 计时偏差/  

    bool isTimer;       // 是否开始计时/  

    bool isDestory = true;     // 计时结束后是否销毁/  

    bool isEnd;         // 计时是否结束/  

    bool isIgnoreTimeScale = true;  // 是否忽略时间速率  

    bool isRepeate;

    float Time_
    {
        get { return isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time; }
    }

    public bool IsTimer
    {
        get
        {
            return isTimer;
        }

        set
        {
            isTimer = value;
        }
    }

    float now;
    // Update is called once per frame  
    void Update()
    {
        if (isTimer)
        {
            timeNow = Time_ - offsetTime;
            now = timeNow - timeStart;
            if (updateEvent != null)
                updateEvent(Mathf.Clamp01(now / timeTarget));
            if (now > timeTarget)
            {
                if (onCompleted != null)
                    onCompleted();
                if (!isRepeate)
                    destory();
                else
                    reStartTimer();
            }
        }
    }
    public float GetLeftTime()
    {
        return Mathf.Clamp(timeTarget - now, 0, timeTarget);
    }

    void OnApplicationPause(bool isPause_)
    {
        if (isPause_)
        {
            pauseTimer();
        }
        else
        {
            connitueTimer();
        }
    }

    /// <summary>  
    /// 计时结束  
    /// </summary>  
    public void destory()
    {
        isTimer = false;
        isEnd = true;
        if (isDestory)
            Destroy(gameObject);
    }
    float _pauseTime;
    /// <summary>  
    /// 暂停计时  
    /// </summary>  
    public void pauseTimer()
    {
        if (isEnd)
        {
            if (isLog) Debug.LogWarning("计时已经结束！");
        }
        else
        {
            if (isTimer)
            {
                isTimer = false;
                _pauseTime = Time_;
            }
        }
    }
    /// <summary>  
    /// 继续计时  
    /// </summary>  
    public void connitueTimer()
    {
        if (isEnd)
        {
            if (isLog) Debug.LogWarning("计时已经结束！请从新计时！");
        }
        else
        {
            if (!isTimer)
            {
                offsetTime += (Time_ - _pauseTime);
                isTimer = true;
            }
        }
    }
    public void reStartTimer()
    {
        timeStart = Time_;
        offsetTime = 0;
    }

    public void changeTargetTime(float time_)
    {
        timeTarget += time_;
    }
    /// <summary>  
    /// 开始计时 :   
    /// </summary>  
    public void startTiming(float time_, CompleteEvent onCompleted_, UpdateEvent update = null, bool isIgnoreTimeScale_ = true, bool isRepeate_ = false, bool isDestory_ = true)
    {
        timeTarget = time_;
        if (onCompleted_ != null)
            onCompleted = onCompleted_;
        if (update != null)
            updateEvent = update;
        isDestory = isDestory_;
        isIgnoreTimeScale = isIgnoreTimeScale_;
        isRepeate = isRepeate_;

        timeStart = Time_;
        offsetTime = 0;
        isEnd = false;
        isTimer = true;

    }
    /// <summary>  
    /// 创建计时器:名字  
    /// </summary>  
    public static Timer createTimer(string gobjName = "Timer")
    {
        GameObject g = new GameObject(gobjName);
        Timer timer = g.AddComponent<Timer>();
        return timer;
    }
}
