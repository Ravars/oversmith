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
        public Animator[] checkListAnimator;
        public Animator openAnimator;
        private static readonly int Check = Animator.StringToHash("Check");
        public GameObject prefabCheckBox;
        public Transform checkboxHolder;
        public ItemStruct ItemStruct;
        private static readonly int Open = Animator.StringToHash("Open");

        public void CheckItem()
        {
            if (amountDone >= amount) return;
            
            checkListAnimator[amountDone].SetTrigger(Check);
            amountDone++;
        }

        public void SetItem(ItemStruct itemStruct, float timeToOpen)
        {
            ItemStruct = itemStruct;
            image.texture = itemStruct.BaseItem.image;
            amount = itemStruct.Amount;
            amountDone = 0;
            amountText.text = amount.ToString();
            checkListAnimator = new Animator[amount];
            
            for (int i = 0; i < amount; i++)
            {
                checkListAnimator[i] = Instantiate(prefabCheckBox, checkboxHolder).GetComponent<Animator>();
            }

            Invoke(nameof(PlayAnimation), 1 * timeToOpen + 0.5f);
        }

        public void PlayAnimation()
        {
            openAnimator.SetTrigger(Open);
        }
    }
}