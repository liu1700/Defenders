using UnityEngine;
using System.Collections;
using google.service.game;
using admob;
using System.IO;
using System;  
public class playgamedemo : MonoBehaviour {

	// Use this for initialization
	GoogleGame game;
	void Start () {
        Debug.Log("start unity demo-------------");
		game = GoogleGame.Instance();
		game.gameEventHandler += onGameEvent;
        Admob.Instance().initAdmob("ca-app-pub-3940256099942544/2934735716", "ca-app-pub-3940256099942544/4411468910");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
	void OnGUI(){
		if (GUI.Button(new Rect(0, 0, 100, 60), "login"))
		{
			game.login (true, false);
		}
        if (GUI.Button(new Rect(120, 0, 100, 60), "loginout"))
        {
			game.loginOut ();
        }
        if (GUI.Button(new Rect(240, 0, 100, 60), "Leaderboards"))
        {
            game.showLeaderboards();
            //game.showLeaderboard("CgkItJ_UzNUHEAIQCQ");
        }
        if (GUI.Button(new Rect(360, 0, 100, 60), "Achieve"))
        {
			game.showAchievements ();
        }
        if (GUI.Button(new Rect(0, 80, 100, 60), "quests"))
        {
            game.showQuests(GameConst.SELECT_ALL_QUESTS);
        }
		if (GUI.Button(new Rect(120, 80, 100, 60), "playerinfo"))
        {
			Debug.Log (game.getCurrentUserInfo ());
        }
        if (GUI.Button(new Rect(240, 80, 100, 60), "loadAchi"))
        {
            game.loadAchievements(false);
        }
        if (GUI.Button(new Rect(360, 80, 100, 60), "leadermeta"))
        {
            game.loadLeaderboardsMetadata(false);
        }
        if (GUI.Button(new Rect(0, 160, 100, 60), "leaderscores"))
        {
            game.loadTopLeaderboardScores("CgkItJ_UzNUHEAIQCQ", GameConst.TIME_SPAN_ALL_TIME, GameConst.COLLECTION_PUBLIC, 10, false);
        }
        if (GUI.Button(new Rect(120, 160, 100, 60), "unlockachi"))
        {
            game.unlockAchievement("CgkItJ_UzNUHEAIQBA");
        }
        if (GUI.Button(new Rect(240, 160, 100, 60), "unlockachi"))
        {
            game.unlockAchievement("CgkItJ_UzNUHEAIQBA");
        }
        if (GUI.Button(new Rect(360, 160, 100, 60), "submitscore"))
        {
            game.submitLeaderboardScore("CgkItJ_UzNUHEAIQCQ", 1000L);
        }
        if (GUI.Button(new Rect(0, 240, 100, 60), "mroeScore"))
        {
            game.loadMoreLeaderboardScores();
        }
        if (GUI.Button(new Rect(120, 240, 100, 60), "loadevents"))
        {
            game.loadEvents(false);
        }
        if (GUI.Button(new Rect(240, 240, 100, 60), "showsnaps"))
        {
            game.showSnapshots("saved games", true, true, 10);
        }
        if (GUI.Button(new Rect(360, 240, 100, 60), "openSnap"))
        {
            game.openSnapshot("firstgamesnap", true, GameConst.RESOLUTION_POLICY_MOST_RECENTLY_MODIFIED);
        }
        if (GUI.Button(new Rect(0, 320, 100, 60), "writesnap"))
        {
            ScreenCapture.CaptureScreenshot("snapshot.png");
            string snapshotfilePath = Application.persistentDataPath + "/snapshot.png";
            game.writeSnapshot(snapshotfilePath, System.Text.Encoding.UTF8.GetBytes("{'score':20}"));
        }
        if (GUI.Button(new Rect(120, 320, 100, 60), "readsnap"))
        {
            byte[] snapcontent=game.readSnapshot();
            if (snapcontent != null)
            {
                string snapstring=System.Text.Encoding.UTF8.GetString(snapcontent);
                Debug.Log("saved game content:" + snapstring);
            }
        }
        if (GUI.Button(new Rect(240, 320, 100, 60), "Invite"))
        {
            game.showInvitePanel(2, 2, 0L, true);
        }
        if (GUI.Button(new Rect(360, 320, 100, 60), "Invitation"))
        {
            game.showInvitationInbox();
        }
        if (GUI.Button(new Rect(0, 400, 100, 60), "roomPanel"))
        {
            game.showRoomWaitingPanel(3);
        }
        if (GUI.Button(new Rect(120, 400, 100, 60), "createRoom"))
        {
            game.createAutoMatchRoom(1, 1, 0);
        }
        if (GUI.Button(new Rect(240, 400, 100, 60), "leaveRoom"))
        {
            game.leaveRoom();
        }
        if (GUI.Button(new Rect(360, 400, 100, 60), "showTBMatches"))
        {
            game.showTurnBasedMatches();
        }
        if (GUI.Button(new Rect(0, 480, 100, 60), "showTBInvitePanel"))
        {
            game.showTurnBasedInvitations(1, 1, 0, true);
        }
        if (GUI.Button(new Rect(120, 480, 100, 60), "createTBRoom"))
        {
            game.createTurnBasedMatch(1, 1, 0);
        }
        if (GUI.Button(new Rect(240, 480, 100, 60), "logEvent"))
        {
            FirebaseAnalytic.Instance().logEvent("appstart", "{\"time\":\"112222\",\"name\":\"demouser\"}");
        }
        if (GUI.Button(new Rect(360, 480, 100, 60), "admobBanner"))
        {
            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 30, "defaultBanner");
        }
        if (GUI.Button(new Rect(0, 580, 100, 60), "admobInstitial"))
        {
            if (Admob.Instance().isInterstitialReady())
            {
                Admob.Instance().showInterstitial();
            }
            else
            {
                Admob.Instance().loadInterstitial();
            }
        }
        if (GUI.Button(new Rect(120, 580, 100, 60), "NativeBanner"))
        {
            Admob.Instance().showNativeBannerRelative(new AdSize(320, 120), AdPosition.BOTTOM_CENTER, 0, "ca-app-pub-3940256099942544/2934735716");
        }
        if (GUI.Button(new Rect(240, 580, 100, 60), "hideBanner"))
        {
            Admob.Instance().removeBanner("defaultBanner");
            Admob.Instance().removeNativeBanner();
        }

	}
  

	void onGameEvent(int result_code,string eventName,string data){
		Debug.Log (eventName + "-----------" + data);
	}
}
