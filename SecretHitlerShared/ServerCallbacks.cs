using System;
using System.Collections.Generic;
using System.Text;

namespace SecretHitlerShared
{
    public static class ServerCallbacks
    {
        // Names of server callbacks
        public const string PlayerConnectedName = "PlayerConnected";
        public const string ConnectPlayerName = "ConnectPlayer";
        public const string SessionStartedName = "SessionStarted";
        public const string StartGameName = "StartGame";
        public const string EndGameName = "EndGame";
        public const string ChatMessageName = "ChatMessage";
        public const string ElectionVoteName = "ElectionVote";
    }
}
