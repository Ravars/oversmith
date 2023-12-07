using TMPro;
using UnityEngine;

namespace MadSmith.Scripts._Dev
{
    public class FpsCounter : MonoBehaviour
    {
        private int lastFrameIndex;
        private float[] frameDeltaTimeArray;
        [SerializeField] private TextMeshProUGUI uiText;

        private void Awake()
        {
            frameDeltaTimeArray = new float[50];
            Debug.Log("FpsCounter awake");
        }

        private void Update()
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
            uiText.text = Mathf.RoundToInt(CalculateFPS()).ToString();
        }

        private float CalculateFPS()
        {
            float total = 0f;
            foreach (var deltaTime in frameDeltaTimeArray)
            {
                total += deltaTime;
            }
            return frameDeltaTimeArray.Length / total;
        }
    }
}
