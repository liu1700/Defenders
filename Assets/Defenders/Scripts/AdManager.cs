using UnityEngine;
using UnityEngine.SceneManagement;
using Beebyte.Obfuscator;
using Together;

public class AdManager : MonoBehaviour
{

    /// <summary>
    /// This is the main AdMob manager class that can be used/modified by you.
    /// You can set different IDs for different types of Ads (obtainable from Admob developer panel)
    /// And you can define new public functions here and call them later inside your game
    /// </summary>

    // real
    string appid = "9k18hIQ7q873K25dlt4A";
    string interstitialID = "RC83hJUonwIuNA5Y6m7";
    string videoID = "JuKofSkePpJxQxnP2hW";

    //// test
    //[Skip]
    //string admobId = "ca-app-pub-3940256099942544~3347511713";
    //[Skip]
    //string admobBannerID = "ca-app-pub-5176895987178305/3058017265";
    //[Skip]
    //string admobInterstitialID = "ca-app-pub-5176895987178305/1074975465";
    //[Skip]
    //string admobVideoID = "ca-app-pub-5176895987178305/5213531022";

    //[Skip]
    //string admobBannerID = "ca-app-pub-5176895987178305/1396727482";
    //[Skip]
    //string admobInterstitialID = "ca-app-pub-5176895987178305/1182062923";
    //[Skip]
    //string admobVideoID = "ca-app-pub-5176895987178305/3975747223";

    public delegate void CompleteEvent(bool isOk);

    public CompleteEvent rewardCB;
    //Admob ad;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        initAdMgr();
    }


    [Skip]
    void initAdMgr()
    {
        ////  isAdmobInited = true;
        //ad = Admob.Instance();
        //ad.bannerEventHandler += onBannerEvent;
        //ad.interstitialEventHandler += onInterstitialEvent;
        //ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        //ad.nativeBannerEventHandler += onNativeBannerEvent;
        //ad.initAdmob(admobBannerID, admobInterstitialID);
        //ad.setTesting(true);

        //TGSDK.SetDebugModel(true);
        TGSDK.Initialize(appid, "10040"); // 应用宝

        TGSDK.PreloadAd();

        Debug.Log("AdManger Inited.");

        TGSDK.AdRewardSuccessCallback = OnAdRewardSuccess;
        TGSDK.AdRewardFailedCallback = OnAdRewardFailed;
        //////showBannerAd (always)
        ////Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);

        //cache an Interstitial ad for later use
        //ad.loadInterstitial();
        //ad.loadRewardedVideo(admobVideoID);
    }

    //gets called from other classes inside the game
    public void showInterstitial()
    {
        //print("Request for Full AD.");
        //if (TGSDK.CouldShowAd(interstitialID))
        //{
        //    TGSDK.ShowTestView(interstitialID);
        //    //TGSDK.ShowAd(interstitialID);
        //}
    }

    public void showRewardVideo()
    {
        print("Show Reward AD.");

        if (TGSDK.CouldShowAd(videoID))
        {
            //TGSDK.ShowTestView(videoID);
            TGSDK.ShowAd(videoID);
        }
        //if (ad.isRewardedVideoReady())
        //{
        //    ad.showRewardedVideo();
        //}
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void loadReward()
    {
        //print("Request for Reward AD.");
        //ad.loadRewardedVideo(admobVideoID);
    }

    public bool isShowRewardVideoReady()
    {
        //return ad.isRewardedVideoReady();
        return TGSDK.CouldShowAd(videoID);
    }

    //void onRewardedVideoEvent(string eventName, string msg)
    //{
    //    Debug.Log("handler onRewardedVideoEvent---" + eventName + "  rewarded: " + msg);
    //    if (eventName == AdmobEvent.onRewarded)
    //    {
    //        if (rewardCB != null)
    //        {
    //            rewardCB(true);
    //        }
    //        loadReward();
    //    }
    //    else if (eventName == AdmobEvent.onAdClosed)
    //    {
    //        if (rewardCB != null)
    //        {
    //            rewardCB(false);
    //        }
    //        loadReward();
    //    }
    //    else if (eventName == AdmobEvent.onAdFailedToLoad)
    //    {
    //        loadReward();
    //    }
    //}

    [Skip]
    public void OnAdRewardSuccess(string ret)
    {
        Debug.Log("handler onRewardedVideoEvent---" + ret);
        if (rewardCB != null)
        {
            rewardCB(true);
        }
    }

    [Skip]
    public void OnAdRewardFailed(string ret)
    {
        Debug.Log("handler OnAdRewardFailed---" + ret);
        if (rewardCB != null)
        {
            rewardCB(false);
        }
    }


    // 上报
    public void UploadUserViewingVideoScene()
    {
        TGSDK.ShowAdScene(videoID);
    }

    public void UploadUserRejectViewingVideoScene()
    {
        TGSDK.ReportAdRejected(videoID);
    }
}
