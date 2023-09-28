using UnityEngine;

namespace MadSmith.Scripts.Animations
{
    public class WoodFallingWaterAnimationHandler : MonoBehaviour
    {
        [SerializeField] private BridgeAnimation _bridgeAnimation;
        public void HandleAnimation()
        {
            _bridgeAnimation.BridgeFall();
        }
    }
}