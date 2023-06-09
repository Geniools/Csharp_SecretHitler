﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SecretHitlerShared
{
    public partial class Player : ObservableObject
    {
        // Events
        public Action<Player> OnPlayerSelected;

        // Hub properties
        public string LobbyCode { get; set; }
        public string ConnectionId { get; set; }

        // Game properties
        public int RowNr { get; set; }
        public int ColumnNr { get; set; }
        public string Username { get; private set; }

        private SecretRole _role;

        public bool CanBeSelected { get; set; }


        public SecretRole Role { 
            get
            {
                return this._role;
            }
            set
            {
                // Comment the following code in a real game
                if (value is SecretRole.Fascist)
                {
                    this.ImageSource = GameImages.FascistIcon;
                }
                else if (value is SecretRole.Liberal)
                {
                    this.ImageSource = GameImages.PlayerIcon;
                }
                else if (value is SecretRole.Hitler)
                {
                    this.ImageSource = GameImages.HitlerIcon;
                }
                //this.ImageSource = GameImages.PlayerIcon;

                this._role = value;
            }
        }

        [ObservableProperty]
        private string _imageSource;

        private PartyMembership _party;
        public PartyMembership Party {
            get
            {
                return this._party;
            }
            set 
            {
                if(value is PartyMembership.Fascist)
                {
                    this.PartyMembershipImage = GameImages.FascistParty;
                }
                else if (value is PartyMembership.Liberal)
                {
                    this.PartyMembershipImage = GameImages.LiberalParty;
                }

                this._party = value;
            }
        }

        public string PartyMembershipImage { get; set; }

        public Player(
            string username, string lobbyCode = "", string imageSource = GameImages.JoinLobbyImage,
            SecretRole role = SecretRole.Liberal, PartyMembership party = PartyMembership.Liberal
        )
        {
            // Hub properties
            this.LobbyCode = lobbyCode;
            this.ConnectionId = string.Empty;

            // Game properties
            this.Username = username;
            this.ImageSource = imageSource;

            // By default everyone is a liberal
            this.Role = role;
            this.Party = party;
        }

        public bool IsHitler()
        {
            return this.Role is SecretRole.Hitler;
        }

        public bool IsFascist()
        {
            return this.Party is PartyMembership.Fascist;
        }

        public bool IsLiberal()
        {
            return this.Party is PartyMembership.Liberal;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Player player)
            {
                return this.Username.Trim().ToLower().Equals(player.Username.Trim().ToLower());
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [RelayCommand]
        private async Task PlayerSelected()
        {
            this.OnPlayerSelected?.Invoke(this);
        }
    }
}