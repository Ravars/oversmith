using System;
using MadSmith.Scripts.Input;
using UnityEngine;

namespace MadSmith.Scripts._Dev
{
    public class StartPlayer : MonoBehaviour
    {
        public InputReader inputReader;
        private void Awake()
        {
            inputReader.EnableGameplayInput();            
        }
    }
}