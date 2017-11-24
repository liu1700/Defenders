
namespace google.service.game
{
    // The position of the ad on the screen.
    public class GameEvent
    {
        public static readonly string Group_User = "user";
	   public static readonly string onConnectSuccess = "onConnectSuccess";
	public static readonly string onConnectFailed = "onConnectFailed";
	public static readonly string onLoginoutResult = "onLoginoutResult";
	public static readonly string onLoginFail = "onLoginFail";
	public static readonly string onActivityResult = "onActivityResult";
	public static readonly string onConnectionSuspended = "onConnectionSuspended";
	public static readonly string onLoadAchievementsResult = "onLoadAchievementsResult";
	public static readonly string onLoadMoreLeaderboardScores = "onLoadMoreLeaderboardScores";
	public static readonly string onLoadLeaderboardTopScores = "onLoadLeaderboardTopScores";
	public static readonly string onLoadLeaderboardUserCenterScores = "onLoadLeaderboardUserCenterScores";
	public static readonly string onLeaderboardMetadataResult = "onLeaderboardMetadataResult";
	public static readonly string onLoadQuestsResult = "onLoadQuestsResult";
	public static readonly string onLoadEventsResult = "onLoadEventsResult";
	public static readonly string onQuestCompleted = "onQuestCompleted";
	public static readonly string onAcceptQuestResult = "onAcceptQuestResult";
	public static readonly string onClaimQuestResult = "onClaimQuestResult";

	public static readonly string onLoadSnapshotsResult = "onLoadSnapshotsResult";
	public static readonly string onOpenSnapshotResult = "onOpenSnapshotResult";
	public static readonly string onSelectedSnapshot = "onSelectedSnapshot";
	public static readonly string onCanncelSnapshot = "onCanncelSnapshot";
	public static readonly string onWriteSnapshotResult = "onWriteSnapshotResult";
	public static readonly string onDeleteSnapshotResult = "onDeleteSnapshotResult";

	public static readonly string onRoomCreatedCancel = "onRoomCreatedCancel";
	public static readonly string onRoomWaitingChange = "onRoomWaitingChange";
	public static readonly string onRealTimeMessageSent = "onRealTimeMessageSent";

	public static readonly string onRealTimeMessageReceived = "onRealTimeMessageReceived";
	public static readonly string onRoomCreated = "onRoomCreated";
	public static readonly string onJoinedRoom = "onJoinedRoom";
	public static readonly string onRoomConnected = "onRoomConnected";
	public static readonly string onLeftRoom = "onLeftRoom";

	public static readonly string onRoomConnecting = "onRoomConnecting";
	public static readonly string onRoomAutoMatching = "onRoomAutoMatching";
	public static readonly string onPeerInvitedToRoom = "onPeerInvitedToRoom";
	public static readonly string onPeerDeclined = "onPeerDeclined";
	public static readonly string onPeerJoined = "onPeerJoined";
	public static readonly string onPeerLeft = "onPeerLeft";
	public static readonly string onConnectedToRoom = "onConnectedToRoom";
	public static readonly string onDisconnectedFromRoom = "onDisconnectedFromRoom";
	public static readonly string onPeersConnected = "onPeersConnected";
	public static readonly string onPeersDisconnected = "onPeersDisconnected";
	public static readonly string onP2PConnected = "onP2PConnected";
	public static readonly string onP2PDisconnected = "onP2PDisconnected";

	public static readonly string onInvitationReceived = "onInvitationReceived";
	public static readonly string onInvitationRemoved = "onInvitationRemoved";
	public static readonly string onInvitationSelectedResult = "onInvitationSelectedResult";

	public static readonly string onTurnBasedMatchRemoved = "onTurnBasedMatchRemoved";
	public static readonly string onTurnBasedMatchReceived = "onTurnBasedMatchReceived";
	public static readonly string onInitiateMatchResult = "onInitiateMatchResult";
	public static readonly string onInviteTurnbasedPlayerResult = "onInviteTurnbasedPlayerResult";
	public static readonly string onCancelMatchResult = "onCancelMatchResult";
	public static readonly string onUpdateMatchResult = "onUpdateMatchResult";
	public static readonly string onLeaveMatchResult = "onLeaveMatchResult";
	public static readonly string onMatchSelectedResult = "onMatchSelectedResult";
	public static readonly string onLoadMatchResult = "onLoadMatchResult";
	public static readonly string onLoadMatchesResult = "onLoadMatchesResult";
    }
}
