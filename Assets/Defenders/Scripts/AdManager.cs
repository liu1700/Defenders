using UnityEngine;
using admob;
using UnityEngine.SceneManagement;
using Beebyte.Obfuscator;

public class AdManager : MonoBehaviour
{

    /// <summary>
    /// This is the main AdMob manager class that can be used/modified by you.
    /// You can set different IDs for different types of Ads (obtainable from Admob developer panel)
    /// And you can define new public functions here and call them later inside your game
    /// </summary>

    //// real
    //[Skip]
    //string admobBannerID = "ca-app-pub-5176895987178305/1396727482";
    //[Skip]
    //string admobInterstitialID = "ca-app-pub-5176895987178305/1182062923";
    //[Skip]
    //string admobVideoID = "ca-app-pub-5176895987178305/3975747223";

    // test
    [Skip]
    string admobId = "ca-app-pub-3940256099942544~3347511713";
    [Skip]
    string admobBannerID = "ca-app-pub-3940256099942544/6300978111";
    [Skip]
    string admobInterstitialID = "ca-app-pub-3940256099942544/1033173712";
    [Skip]
    string admobVideoID = "ca-app-pub-3940256099942544/5224354917";

    public delegate void CompleteEvent();

    public CompleteEvent rewardCB;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        initAdmob();
    }

    [Skip]
    Admob ad;
    //bool isAdmobInited = false;

    [Skip]
    void initAdmob()
    {
        //  isAdmobInited = true;
        ad = Admob.Instance();
        ad.bannerEventHandler += onBannerEvent;
        ad.interstitialEventHandler += onInterstitialEvent;
        ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        ad.nativeBannerEventHandler += onNativeBannerEvent;
        ad.initAdmob(admobBannerID, admobInterstitialID);
        ad.setTesting(true);
        Debug.Log("Admob Inited.");

        ////showBannerAd (always)
        //Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);

        //cache an Interstitial ad for later use
        ad.loadInterstitial();
        ad.loadRewardedVideo(admobVideoID);
    }

    public void loadInterstitial()
    {
        print("Request load for Full AD.");
        ad.loadInterstitial();
    }

    public void loadReward()
    {
        print("Request load for Reward AD.");
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

    public void showRewardVideo()
    {
        print("Request for Reward AD.");
        if (ad.isRewardedVideoReady())
        {
            ad.showRewardedVideo();
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        ad.loadRewardedVideo(admobVideoID);
    }

    public bool isShowRewardVideoReady()
    {
        return ad.isRewardedVideoReady();
    }

    [Skip]
    void onInterstitialEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
        //if (eventName == AdmobEvent.onAdLoaded)
        //{
        //    Admob.Instance().showInterstitial();
        //}
    }

    [Skip]
    void onBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
    }

    [Skip]
    void onRewardedVideoEvent(string eventName, string msg)
    {
        Debug.Log("handler onRewardedVideoEvent---" + eventName + "   " + msg);
        if (eventName == AdmobEvent.onRewarded)
        {
            if (rewardCB != null)
            {
                rewardCB();
            }
        }
    }

    [Skip]
    void onNativeBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
    }
}
