using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
namespace google.service.game
{
    public class GoogleGame
    {
        public delegate void GameEventHandler(int result_code,string eventName, string msg);

        public event GameEventHandler gameEventHandler;
        /*
        public event GameEventHandler interstitialEventHandler;
        public event GameEventHandler rewardedVideoEventHandler;
        public event GameEventHandler nativeBannerEventHandler;
        */
        private static GoogleGame _instance;

        public static GoogleGame Instance()
        {
            if (_instance == null)
            {
                _instance = new GoogleGame();
                _instance.preInitGame();
            }
            return _instance;
        }

#if UNITY_EDITOR
		private void preInitGame()
		{
		}

		public void login(bool driveAPI,bool plusAPI)
		{
		Debug.Log("calling login");
		}
		public void loginOut()
		{
			Debug.Log("calling loginOut");
		}
		public bool isConnected()
		{
			Debug.Log("calling isConnected");
			return false;
		}
		public bool isConnecting()
		{
			Debug.Log("calling isConnecting");
			return false;
		}
		public string getCurrentUserInfo()
		{
			Debug.Log("calling getCurrentUser");
			return null;
		}
		public string getUserInfo(string userID)
		{
			Debug.Log("calling getUserInfo");
			return null;
		}
		public void showAchievements()
		{
			Debug.Log("calling showAchievements");
		}
        public void unlockAchievement(string achievementId)
        {
            Debug.Log("calling unlockAchievements");
        }
        
        public void incrementAchievement(string achievementId,int step)
        {
            Debug.Log("calling incrementAchievements");
        }
        public void loadAchievements(bool forceReload)
        {
            Debug.Log("calling loadAchievements");
        }

        public void revealAchievement(string achievementId)
        {
            Debug.Log("calling revealAchievements");
        }

        public void setAchievementsSteps(string achievementId,int step)
        {
            Debug.Log("calling setAchievementsSteps");
        }
        public void loadLeaderboardsMetadata(bool forceReload)
        {
            Debug.Log("calling loadLeaderboardsMetadata");
        }
        public void loadTopLeaderboardScores(string leaderboardId, int span, int leaderboardCollection, int maxResults,
            bool forceReload)
        {
            Debug.Log("calling loadTopLeaderboardScores");
        }
        public void loadUserCenterLeaderboardScores(string leaderboardId, int span, int leaderboardCollection,
            int maxResults, bool forceReload)
        {
            Debug.Log("calling loadUserCenterLeaderboardScores");
        }
        public void loadMoreLeaderboardScores()
        {
            Debug.Log("calling loadMoreLeaderboardScores");
        }
        public void submitLeaderboardScore(string leaderboardID, long score)
        {
            Debug.Log("calling submitLeaderboardScore");
        }
		public void showLeaderboards()
		{
			Debug.Log("calling showLeaderboards");
		}
		public void showLeaderboard(string leaderboardID)
		{
			Debug.Log("calling showLeaderboard");
		}

       
       


    public void incrementEvent(string eventID, int incrementAmount) {
	}

	public void loadEvents(bool forceReload) {
	}

	public void loadEvent(string eventID, bool forceReload) {
	}

	public void loadQuest(string id, bool forceReload) {
	}

	public void loadQuests(int[] questSelectors, int sortOrder, bool forceReload) {
	}

	public void acceptQuest(string questId) {
	}

	public void claimQuest(string questId, string milestoneId) {
	}

	public void showQuests(int[] questSelectors) {
	}

	public void showQuest(string questID) {
		
	}

    public void showSnapshots(string title, bool allowAdd, bool allowDelete, int maxCount)
    {
    }
    public void loadSnapshots(bool forceReload)
    {
    }
    public void openSnapshot(string fileName, bool createIfNotFound, int conflictPolicy)
    {
    }
    public void writeSnapshot(string snapshotPath, byte[] gameContent)
    {
    }
    public byte[] readSnapshot()
    {
        return null;
    }
    public void deleteSnapshot(string uniqueName)
    {     
    }


    public void createAutoMatchRoom(int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitMask) {
		
	}

	public void showInvitePanel(int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitMask,
			bool allowAutomatch) {
        
	}

	public void showRoomWaitingPanel(int minParticipantsToStart) {
		
	}
    public void showInvitationInbox()
    {
         
    }
	public void declineInvitation(string invitationId) {
		
	}

	public void dismissInvitation(string invitationId) {
		
	}

	public void acceptInviteToRoom(string invitationId) {
		
	}

	public void leaveRoom() {
		
	}

	public int sendReliableMessage(byte[] messageData, string roomId, string recipientParticipantId) {
        return 0;
	}

	public int sendUnreliableMessage(byte[] messageData, string roomId, string[] recipientParticipantIds) {
        return 0;
	}


         public void acceptTurnBasedInvitation(string invitationId)
        {
            
        }

        public void cancelTurnBasedMatch(string matchId)
        {
            
        }

        public void createTurnBasedMatch(int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitMask)
        {
            
        }

        public void declineTurnBasedInvitation(string invitationId)
        {
            
        }

        public void dismissTurnBasedInvitation(string invitationId)
        {
            
        }

        public void dismissTurnBasedMatch(string matchId)
        {
            
        }

        public void finishTurnBasedMatch(string matchId, byte[] matchData)
        {
            
        }

        public void showTurnBasedMatches()
        {
            
        }

        public int getMaxTurnBasedMatchDataSize()
        {
         return 128;
        }

        public void leaveTurnBasedMatch(string matchID)
        {
         
        }

        public void leaveTurnBasedMatchDuringTurn(string matchId, string pendingParticipantId)
        {
         
        }

        public void showTurnBasedInvitations(int minPlayers, int maxPlayers, long exclusiveBitMask, bool allowAutomatch)
        {
         
        }

        public void loadTurnBasedMatch(string matchId)
        {
         
        }

        public void loadTurnBasedMatchByStatus(int[] matchTurnStatuses, int invitationSortOrder)
        {
         
        }

        public void rematchTurnBasedGame(string matchID)
        {
         
        }

        public void takeTurnBasedTurn(string matchId, byte[] matchData, string pendingParticipantId)
        {
         
        }


#elif UNITY_ANDROID
        private AndroidJavaObject jgame;
        private void preInitGame()
        {
            if (jgame == null)
            {
                AndroidJavaClass gameUnityPluginClass = new AndroidJavaClass("com.google.service.game.GameUnityPlugin");
                jgame = gameUnityPluginClass.CallStatic<AndroidJavaObject>("getInstance");
                InnerGameListener innerlistener = new InnerGameListener();
                innerlistener.gameInstance = this;
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activy = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jgame.Call("setContext", new object[] { activy, new GameListenerProxy(innerlistener) });
            }
        }

        public void login(bool driveAPI, bool plusAPI)
        {
            jgame.Call("login", new object[] { driveAPI, plusAPI });
        }
        public void loginOut()
        {
            jgame.Call("loginOut");
        }
        public bool isConnected()
        {
            bool isReady = jgame.Call<bool>("isConnected");
            return isReady;
        }
        public bool isConnecting()
        {
            bool isReady = jgame.Call<bool>("isConnecting");
            return isReady;
        }
        public string getCurrentUserInfo()
        {
            return jgame.Call<string>("getCurrentUserInfo");
        }
        
        public void showAchievements()
        {
            jgame.Call("showAchievements");
        }


        public void unlockAchievement(string achievementId)
        {
            jgame.Call("unlockAchievement", achievementId);
        }

        public void incrementAchievement(string achievementId, int step)
        {
            jgame.Call("incrementAchievement", new object[] { achievementId, step });
        }
        public void loadAchievements(bool forceReload)
        {
            jgame.Call("loadAchievements", forceReload);
        }

        public void revealAchievement(string achievementId)
        {
            jgame.Call("revealAchievement", achievementId);
        }

        public void setAchievementsSteps(string achievementId, int step)
        {
            jgame.Call("setAchievementsSteps", new object[] { achievementId, step });
        }
        public void loadLeaderboardsMetadata(bool forceReload)
        {
            jgame.Call("loadLeaderboardsMetadata", forceReload);
        }
        public void loadTopLeaderboardScores(string leaderboardId, int span, int leaderboardCollection, int maxResults,
            bool forceReload)
        {
            jgame.Call("loadTopLeaderboardScores", new object[] { leaderboardId, span, leaderboardCollection, maxResults, forceReload });
        }
        public void loadUserCenterLeaderboardScores(string leaderboardId, int span, int leaderboardCollection,
            int maxResults, bool forceReload)
        {
            jgame.Call("loadUserCenterLeaderboardScores", new object[] { leaderboardId, span, leaderboardCollection, maxResults, forceReload });
        }
        public void loadMoreLeaderboardScores()
        {
            jgame.Call("loadMoreLeaderboardScores");
        }
        public void submitLeaderboardScore(string leaderboardID, long score)
        {
            jgame.Call("submitLeaderboardScore", new object[] { leaderboardID, score });
        }


        public void showLeaderboards()
        {
            jgame.Call("showLeaderboards");
        }
        public void showLeaderboard(string leaderboardID)
        {
            jgame.Call("showLeaderboard", leaderboardID);
        }


        public void incrementEvent(string eventID, int incrementAmount)
        {
            jgame.Call("incrementEvent", new object[] { eventID, incrementAmount });
        }

        public void loadEvents(bool forceReload)
        {
            jgame.Call("loadEvents", forceReload);
        }

        public void loadEvent(string eventID, bool forceReload)
        {
            jgame.Call("loadEvent", new object[] { eventID, forceReload });
        }

        public void loadQuest(string id, bool forceReload)
        {
            jgame.Call("loadQuest", new object[] { id, forceReload });
        }

        public void loadQuests(int[] questSelectors, int sortOrder, bool forceReload)
        {
            jgame.Call("loadQuests", new object[] { questSelectors, sortOrder, forceReload });
        }

        public void acceptQuest(string questId)
        {
            jgame.Call("acceptQuest", questId);
        }

        public void claimQuest(string questId, string milestoneId)
        {
            jgame.Call("clainQuest", new object[] { questId, milestoneId });
        }

        public void showQuests(int[] questSelectors)
        {
            jgame.Call("showQuests", new object[] { questSelectors });
        }

        public void showQuest(string questID)
        {
            jgame.Call("showQuest", questID);
        }

        public void showSnapshots(string title, bool allowAdd, bool allowDelete, int maxCount)
        {
            jgame.Call("showSnapshots", new object[] { title, allowAdd, allowDelete, maxCount });
        }
        public void loadSnapshots(bool forceReload)
        {
            jgame.Call("loadSnapshots", forceReload);
        }
        public void openSnapshot(string snapName, bool createIfNotFound, int conflictPolicy)
        {
            jgame.Call("openSnapshot", new object[] { snapName, createIfNotFound, conflictPolicy });
        }
        public void writeSnapshot(string screenshotImagePath, byte[] gameContent)
        {
            jgame.Call("writeSnapshot", new object[] { screenshotImagePath, gameContent });
        }

        public byte[] readSnapshot()
        {
            return jgame.Call<byte[]>("readSnapshot");
        }

        public void deleteSnapshot(string uniqueName)
        {
            jgame.Call("deleteSnapshot", uniqueName);
        }


        public void createAutoMatchRoom(int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitMask)
        {
            jgame.Call("createAutoMatchRoom", new object[] { minAutoMatchPlayers, maxAutoMatchPlayers, exclusiveBitMask });
        }

        public void showInvitePanel(int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitMask,
                bool allowAutomatch)
        {
            jgame.Call("showInvitePanel", new object[] { minAutoMatchPlayers, maxAutoMatchPlayers, exclusiveBitMask, allowAutomatch });
        }

        public void showRoomWaitingPanel(int minParticipantsToStart)
        {
            jgame.Call("showRoomWaitingPanel", minParticipantsToStart);
        }
        public void showInvitationInbox()
        {
            jgame.Call("showInvitationInbox");
        }

        public void declineInvitation(string invitationId)
        {
            jgame.Call("declineInvitation", invitationId);
        }

        public void dismissInvitation(string invitationId)
        {
            jgame.Call("dismissInvitation", invitationId);
        }

        public void acceptInviteToRoom(string invitationId)
        {
            jgame.Call("acceptInviteToRoom", invitationId);
        }

        public void leaveRoom()
        {
            jgame.Call("leaveRoom");
        }

        public int sendReliableMessage(byte[] messageData, string roomId, string recipientParticipantId)
        {
            return jgame.Call<int>("sendReliableMessage", new object[] { messageData, roomId, recipientParticipantId });
        }

        public int sendUnreliableMessage(byte[] messageData, string roomId, string[] recipientParticipantIds)
        {
            return jgame.Call<int>("sendUnreliableMessage", new object[] { messageData, roomId, recipientParticipantIds });
        }

        public void acceptTurnBasedInvitation(string invitationId)
        {
            jgame.Call("acceptTurnBasedInvitation", invitationId);
        }

        public void cancelTurnBasedMatch(string matchId)
        {
            jgame.Call("cancelTurnBasedMatch", matchId);
        }

        public void createTurnBasedMatch(int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitMask)
        {
            jgame.Call("createTurnBasedMatch", new object[] { minAutoMatchPlayers, maxAutoMatchPlayers, exclusiveBitMask });
        }

        public void declineTurnBasedInvitation(string invitationId)
        {
            jgame.Call("declineTurnBasedInvitation", invitationId);
        }

        public void dismissTurnBasedInvitation(string invitationId)
        {
            jgame.Call("dismissTurnBasedInvitation", invitationId);
        }

        public void dismissTurnBasedMatch(string matchId)
        {
            jgame.Call("dismissTurnBasedMatch", matchId);
        }

        public void finishTurnBasedMatch(string matchId, byte[] matchData)
        {
            jgame.Call("finishTurnBasedMatch", new object[] { matchId, matchData });
        }

        public void showTurnBasedMatches()
        {
            jgame.Call("showTurnBasedMatches");
        }

        public int getMaxTurnBasedMatchDataSize()
        {
            return jgame.Call<int>("getMaxTurnBasedMatchDataSize");
        }

        public void leaveTurnBasedMatch(string matchID)
        {
            jgame.Call("leaveTurnBasedMatch", matchID);
        }

        public void leaveTurnBasedMatchDuringTurn(string matchId, string pendingParticipantId)
        {
            jgame.Call("leaveTurnBasedMatchDuringTurn", new object[] { matchId, pendingParticipantId });
        }

        public void showTurnBasedInvitations(int minPlayers, int maxPlayers, long exclusiveBitMask, bool allowAutomatch)
        {
            jgame.Call("showTurnBasedInvitations", new object[] { minPlayers, maxPlayers, exclusiveBitMask, allowAutomatch });
        }

        public void loadTurnBasedMatch(string matchId)
        {
            jgame.Call("loadTurnBasedMatch", matchId);
        }

        public void loadTurnBasedMatchByStatus(int[] matchTurnStatuses, int invitationSortOrder)
        {
            jgame.Call("loadTurnBasedMatchByStatus", new object[] { matchTurnStatuses, invitationSortOrder });
        }

        public void rematchTurnBasedGame(string matchID)
        {
            jgame.Call("rematchTurnBasedGame", matchID);
        }

        public void takeTurnBasedTurn(string matchId, byte[] matchData, string pendingParticipantId)
        {
            jgame.Call("takeTurnBasedTurn", new object[] { matchId, matchData, pendingParticipantId });
        }

        class InnerGameListener : IGameListener
        {
            internal GoogleGame gameInstance;
            public void onGameEvent(int result_code, string eventName, string paramString)
            {
                if (gameInstance.gameEventHandler != null)
                    gameInstance.gameEventHandler(result_code,eventName, paramString);
            }
        }
#endif

    }
}
