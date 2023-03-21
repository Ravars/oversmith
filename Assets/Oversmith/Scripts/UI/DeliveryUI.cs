using _Developers.Vitor;
using TMPro;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public class DeliveryUI : MonoBehaviour
    {
        public TextMeshProUGUI[] texts;

        public void SetItems(BaseItem[] itemsToShow)
        {
            Debug.Log(itemsToShow.Length);
            
            
            foreach (var text in texts)
            {
                text.text = string.Empty;
            }
            for (int i = 0; i < itemsToShow.Length; i++)
            {
                Debug.Log(itemsToShow[i].itemName);
                texts[i].text = itemsToShow[i].itemName;
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

    }
}