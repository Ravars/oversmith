using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Developers.Vitor.Multiplayer2.Scripts.UI
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
        public bool Ready;
        public int CharacterID;
        [SerializeField] private Sprite[] charactersImages;
        protected Callback<AvatarImageLoaded_t> ImageLoaded;
        //Manager
        private MadSmithNetworkManager _manager;
        public MadSmithNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }
        private void Start()
        {
            if (Manager.TransportLayer == TransportLayer.Steam)
            {
                ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
            }
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
        public void SetPlayerValues()
        {
            PlayerNameText.text = PlayerName;
            ChangeReadyStatus();
            CharacterImage.sprite = charactersImages[CharacterID];
            if(!AvatarReceived) {GetPlayerIcon();}
        }
        public void ChangeReadyStatus()
        {
            if (Ready)
            {
                PlayerReadyText.text = "Ready";
                PlayerReadyText.color = Color.green;
            }
            else
            {
                PlayerReadyText.text = "Unready";
                PlayerReadyText.color = Color.red;
                
            }
        }
        private void GetPlayerIcon()
        {
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


    }
}