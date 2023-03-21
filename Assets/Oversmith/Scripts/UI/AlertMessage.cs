using System;
using TMPro;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public class AlertMessage : MonoBehaviour
    {
        public TextMeshProUGUI text;
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