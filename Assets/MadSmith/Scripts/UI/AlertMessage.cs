using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class AlertMessage : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public Image image;
        // public 
        public float time = 4;
        private void FixedUpdate()
        {
            time -= Time.fixedDeltaTime;
            if (time <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}