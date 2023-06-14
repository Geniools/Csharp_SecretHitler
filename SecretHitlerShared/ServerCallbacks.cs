
namespace SecretHitlerShared
{
    public static class ServerCallbacks
    {
        // Names of server callbacks
        public const string PlayerConnectedName = "PlayerConnected";
        public const string PlayerDisconnectedName = "PlayerDisconnected";
        public const string ConnectPlayerName = "ConnectPlayer";
        public const string DisconnectPlayerName = "DisconnectPlayer";
        
        public const string StartGameName = "StartGame";
        public const string ClearAllPlayersName = "ClearAllPlayers";
        
        public const string SessionStartedName = "SessionStarted";
        public const string EndGameName = "EndGame";

        public const string ChatMessageName = "ChatMessage";
        public const string ElectionVoteName = "ElectionVote";

        public const string GetConnectionIdName = "GetConnectionId";
    }
}
