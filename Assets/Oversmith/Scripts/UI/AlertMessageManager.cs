using System;
using Oversmith.Scripts.Utils;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public class AlertMessageManager : Singleton<AlertMessageManager>
    {
        [SerializeField] private AlertMessage _alertMessagePrefab;
        [SerializeField] private GameObject _alertMessageHolder;
        public void SpawnAlertMessage(string message, MessageType type)
        {
            AlertMessage alertMessage = Instantiate(_alertMessagePrefab, _alertMessageHolder.transform).GetComponent<AlertMessage>();
            alertMessage.text.text = message;
            Color color = Color.white;

            switch (type)
            {
                case MessageType.Normal:
                    color = Color.white;
                    break;
                case MessageType.Success:
                    color = Color.green;
                    break;
                case MessageType.Error:
                    color = Color.red;
                    break;
                case MessageType.Alert:
                    color = Color.yellow;
                    break;

            }
            color.a = 0.4f;
            alertMessage.image.color = color;
            
        }
    }

    public enum MessageType
    {
        Normal,
        Success,
        Error,
        Alert
    }
    
    
}