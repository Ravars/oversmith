using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.Multiplayer.Old.Managers;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using TransportLayer = MadSmith.Scripts.Multiplayer.Managers.TransportLayer;
namespace MadSmith.Scripts.Multiplayer.UI
{
    public class PlayerListItem : MonoBehaviour
    {
        public string PlayerName;
        public int ConnectionID;
        public ulong PlayerSteamID;
        private bool AvatarReceived;
        
        public TextMeshProUGUI PlayerNameText;
        public RawImage PlayerIcon;
        public Image CharacterImage;
        public TextMeshProUGUI PlayerReadyText;
        public LocalizeStringEvent readyTextLocalize;
        public TextMeshProUGUI CharacterIdText;
        public bool Ready;
        public int CharacterID;
        [SerializeField] private Sprite[] charactersImages;
        protected Callback ImageLoaded;
        
        // [Header("Listening to")]
        // [SerializeField] private VoidEventChannelSO onUpdatePlayerList = default;
        //Manager
        private MadSmithNetworkRoomManager _manager;

        private MadSmithNetworkRoomManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkRoomManager;
            }
        }
        private void Start()
        {
            // Debug.Log("Player list item start");
            if (Manager.TransportLayer == TransportLayer.Steam)
            {
                ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
            }

            // onUpdatePlayerList.OnEventRaised += SetPlayerValues;
            // SetPlayerValues();
        }

        private void OnDisable()
        {
            // onUpdatePlayerList.OnEventRaised -= SetPlayerValues;
        }

        public void SetPlayerValues()
        {
            PlayerNameText.text = PlayerName;
            ChangeReadyStatus();
            // CharacterIdText.text = CharacterID.ToString();
            CharacterImage.sprite = charactersImages[CharacterID];
            if(!AvatarReceived) {GetPlayerIcon();}
        }

        private void ChangeReadyStatus()
        {
            if (Ready)
            {
                readyTextLocalize.StringReference.TableEntryReference = "Ready";
                PlayerReadyText.color = Color.green;
            }
            else
            {
                readyTextLocalize.StringReference.TableEntryReference = "Unready";
                PlayerReadyText.color = Color.red;
                
            }
        }

        #region Steam
        private void GetPlayerIcon()
        {
            if (!SteamLobby.InstanceExists) return;
            int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
            if (ImageID == -1)
            {
                return;
            }

            PlayerIcon.texture = GetSteamImageAsTexture(ImageID);
        }
        private Texture2D GetSteamImageAsTexture(int iImage)
        {
            Texture2D texture = null;

            bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
            if (isValid)
            {
                byte[] image = new byte[width * height * 4];

                isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

                if (isValid)
                {
                    texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                    texture.LoadRawTextureData(image);
                    texture.Apply();
                }
            }
            AvatarReceived = true;
            return texture;
        }
        private void OnImageLoaded(AvatarImageLoaded_t callback)
        {
            if (callback.m_steamID.m_SteamID == PlayerSteamID)
            {
                PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}