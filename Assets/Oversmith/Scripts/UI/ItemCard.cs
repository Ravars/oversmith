using Oversmith.Scripts.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Oversmith.Scripts.UI
{
    public class ItemCard : MonoBehaviour
    {
        public RawImage image;
        public TextMeshProUGUI amountText;
        public int amount;
        public int amountDone;
        public Animator[] animator;
        private static readonly int Check = Animator.StringToHash("Check");
        public GameObject prefabCheckBox;
        public Transform checkboxHolder;
        public ItemStruct ItemStruct;
        public void CheckItem()
        {
            if (amountDone >= amount) return;
            
            animator[amountDone].SetTrigger(Check);
            amountDone++;
        }

        public void SetItem(ItemStruct itemStruct)
        {
            ItemStruct = itemStruct;
            image.texture = itemStruct.BaseItem.image;
            amount = itemStruct.Amount;
            amountDone = 0;
            amountText.text = amount.ToString();
            animator = new Animator[amount];
            for (int i = 0; i < amount; i++)
            {
                animator[i] = Instantiate(prefabCheckBox, checkboxHolder).GetComponent<Animator>();
            }
        }
    }
}