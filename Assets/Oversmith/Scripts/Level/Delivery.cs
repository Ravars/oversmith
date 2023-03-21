using System.Threading;
using _Developers.Vitor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oversmith.Scripts.Level
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Delivery : MonoBehaviour
    {
        public BaseItem[] availableItems; // Maybe remove from this script
        public BaseItem[] requiredItems; // private
        public BaseItem[] currentItems;
        public int itemsDelivered; 
        [Range(2,10)] 
        public int maxItems;
        public GameObject[] pointsToSpawn;
        public float totalTime = 120;
        public float currentTime;
        public bool isRunning;

        public GameObject visual;
        public GameObject visualItems;
        public GameObject wagonMan;
        
        private void Start()
        {
            int amount = Random.Range(1, maxItems + 1);
            requiredItems = new BaseItem[amount];
            currentItems = new BaseItem[amount];
            for (int i = 0; i < amount; i++)
            {
                int itemIndex = Random.Range(0, availableItems.Length);
                requiredItems[i] = availableItems[itemIndex];
            }

            SetVisual(false);
            Invoke(nameof(StartTimer),5);
        }

        public bool CanSetItem()
        {
            Debug.Log(itemsDelivered + " " + requiredItems.Length);
            return itemsDelivered < requiredItems.Length; 
        }
        
        public void SetItem(BaseItem newItem)
        {
            if (CanSetItem())
            {
                currentItems[itemsDelivered] = newItem;
                SpawnItem(newItem, itemsDelivered);
                itemsDelivered++;
                if (itemsDelivered >= requiredItems.Length)
                {
                    Finish();
                }
            }
        }
        private void SpawnItem(BaseItem item, int index)
        {
            Instantiate(item.prefab, pointsToSpawn[index].transform.position, pointsToSpawn[index].transform.rotation, pointsToSpawn[index].transform.parent);
        }

        private void Finish()
        { 
            isRunning = false; 
            Invoke(nameof(FinishTimer),1);
        }

        private void FinishTimer()
        {
            SetVisual(false);
            
            //  Avisar pro game manager que foi finalizado
            
            // Enviar para ele a lista currentItem e a Required items e deixa ele se virar
        }

        public void StartTimer()
        {
            SetVisual(true);
            isRunning = true;
        }

        private void SetVisual(bool state) // This is just to simulate the Wagon delivery to the Store
        {
            visual.SetActive(state);            
            visualItems.SetActive(state);            
            wagonMan.SetActive(state);
        }

        private void FixedUpdate()
        {
            if (!isRunning) return;
            currentTime += Time.fixedDeltaTime;
            if (currentTime >= totalTime)
            {
                Finish();
            }
        }
    }
}