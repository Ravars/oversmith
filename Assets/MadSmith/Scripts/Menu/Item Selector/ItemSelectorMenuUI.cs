using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Menu.Item_Selector
{
    public class ItemSelectorMenuUI : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI textMesh;
        public Image image;

        public string text = "none";
        public Sprite icon;

        public ItemSelectorMenuUI(string itemName, Sprite itemIcon)
        {
            text = itemName;
            icon = itemIcon;
        }

        // Start is called before the first frame update
        void Start()
        {
            textMesh.text = text;
            image.sprite = icon;
        }
    }
}