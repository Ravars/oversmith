using MadSmith.Scripts.Animations;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class BridgeInteractableFix : Interactable
    {
        [SerializeField] private BridgeAnimation bridgeAnimation;
        
        public override void Interact(PlayerInteractions obj)
        {
            bridgeAnimation.ResetBridge();
        }
    }
}