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
    string appid = "8iiI93K213E6W5w5J4rH";
    string interstitialID = "k2zjcFEGwF53BJQ9RIW";
    string interstitialOpt = "p969ZaRL37AukvlQT7F";
    string videoID = "Dx37r6o8NSfrMQBbM3t";

    //// test
    //[Skip]
    //string admobId = "ca-app-pub-3940256099942544~3347511713";
    //[Skip]
    //string admobBannerID = "ca-app-pub-3940256099942544/6300978111";
    //[Skip]
    //string admobInterstitialID = "ca-app-pub-3940256099942544/1033173712";
    //[Skip]
    //string admobVideoID = "ca-app-pub-3940256099942544/5224354917";

    public delegate void CompleteEvent();

    public CompleteEvent rewardCB;

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
        TGSDK.Initialize(appid, "10053"); // taptap

        TGSDK.PreloadAd();

        Debug.Log("AdManger Inited.");

        TGSDK.AdRewardSuccessCallback = OnAdRewardSuccess;

        //////showBannerAd (always)
        ////Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);

        ////cache an Interstitial ad for later use
        //ad.loadInterstitial();
        //ad.loadRewardedVideo(admobVideoID);
    }

    //gets called from other classes inside the game
    public void showInterstitial()
    {
        print("Request for Full AD.");
        if (TGSDK.CouldShowAd(interstitialID))
        {
            //TGSDK.ShowTestView(interstitialID);
            TGSDK.ShowAd(interstitialID);
        }
    }

    public void showRewardVideo()
    {
        print("Request for Reward AD.");

        if (TGSDK.CouldShowAd(videoID))
        {
            //TGSDK.ShowTestView(videoID);
            TGSDK.ShowAd(videoID);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public bool isShowRewardVideoReady()
    {
        return TGSDK.CouldShowAd(videoID);
    }

    [Skip]
    public void OnAdRewardSuccess(string ret)
    {
        Debug.Log("handler onRewardedVideoEvent---" + ret);
        if (rewardCB != null)
        {
            rewardCB();
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
