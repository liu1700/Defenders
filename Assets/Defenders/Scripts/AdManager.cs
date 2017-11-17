using UnityEngine;
using System.Collections;
using admob;

public class AdManager : MonoBehaviour
{

    /// <summary>
    /// This is the main AdMob manager class that can be used/modified by you.
    /// You can set different IDs for different types of Ads (obtainable from Admob developer panel)
    /// And you can define new public functions here and call them later inside your game
    /// </summary>

    //string admobBannerID = "ca-app-pub-5176895987178305/1396727482";
    //string admobInterstitialID = "ca-app-pub-5176895987178305/1182062923";
    //string admobVideoID = "ca-app-pub-5176895987178305/3975747223";

    // test
    string admobBannerID = "ca-app-pub-3940256099942544/6300978111";
    string admobInterstitialID = "ca-app-pub-3940256099942544/1033173712";
    string admobVideoID = "ca-app-pub-3940256099942544/5224354917";

    public delegate void CompleteEvent();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        initAdmob();
    }

    Admob ad;
    //bool isAdmobInited = false;

    void initAdmob()
    {

        //  isAdmobInited = true;
        ad = Admob.Instance();
        ad.bannerEventHandler += onBannerEvent;
        ad.interstitialEventHandler += onInterstitialEvent;
        ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        ad.nativeBannerEventHandler += onNativeBannerEvent;
        ad.initAdmob(admobBannerID, admobInterstitialID);
        //ad.setTesting(true);
        Debug.Log("Admob Inited.");

        ////showBannerAd (always)
        //Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);

        //cache an Interstitial ad for later use
        ad.loadInterstitial();
        ad.loadRewardedVideo(admobVideoID);
    }

    //gets called from other classes inside the game
    public void showInterstitial()
    {
        print("Request for Full AD.");
        if (ad.isInterstitialReady())
        {
            ad.showInterstitial();
        }
        ad.loadInterstitial();
    }

    public void showRewardVideo(CompleteEvent cb)
    {
        print("Request for Reward AD.");
        if (ad.isRewardedVideoReady())
        {
            ad.showRewardedVideo();
        }
        cb();
        ad.loadRewardedVideo(admobVideoID);
    }


    void onInterstitialEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
        if (eventName == AdmobEvent.onAdLoaded)
        {
            Admob.Instance().showInterstitial();
        }
    }
    void onBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
    }
    void onRewardedVideoEvent(string eventName, string msg)
    {
        Debug.Log("handler onRewardedVideoEvent---" + eventName + "   " + msg);
    }
    void onNativeBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
    }
}
