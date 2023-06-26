using System;
using System.Linq;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{
    public class Pallet : MonoBehaviour
    {
        [SerializeField] private DeliveryBox blueBoxesPrefabs,browBoxesPrefabs, orangeBoxesPrefabs, pinkBoxesPrefabs;
        [SerializeField] private DeliveryBox[] deliveryBoxesSpawned;
        [SerializeField] private Transform[] pointsToSpawnBoxes;
        public ClientsManager clientsManager;

        private void Awake()
        {
            deliveryBoxesSpawned = new DeliveryBox[pointsToSpawnBoxes.Length];
        }

        public void SpawnBox(ItemDeliveryList deliveryList, BoxColor boxColor, int npcId)
        {
            if (!HasEmptyBox()) return;
            int indexToSpawn = -1;
            for (int i = 0; i < deliveryBoxesSpawned.Length; i++)
            {
                if (ReferenceEquals(deliveryBoxesSpawned[i], null))
                {
                    indexToSpawn = i;
                    break;
                }
            }

            DeliveryBox boxColorToUse;
            switch (boxColor)
            {
                case BoxColor.Pink:
                    boxColorToUse = pinkBoxesPrefabs;
                    break;
                case BoxColor.Orange:
                    boxColorToUse = orangeBoxesPrefabs;
                    break;
                case BoxColor.Brown:
                    boxColorToUse = browBoxesPrefabs;
                    break;
                case BoxColor.Blue:
                    boxColorToUse = blueBoxesPrefabs;
                    break;
                default:
                    boxColorToUse = blueBoxesPrefabs;
                    break;
            }


            var deliveryBox = Instantiate(boxColorToUse, pointsToSpawnBoxes[indexToSpawn].position, Quaternion.identity, transform);
            deliveryBox.Init(deliveryList,boxColor,npcId, clientsManager);
            deliveryBoxesSpawned[indexToSpawn] = deliveryBox;

        }

        public void DeSpawnBox(int npcId)
        {
            for (int i = 0; i < deliveryBoxesSpawned.Length; i++)
            {
                if (deliveryBoxesSpawned[i].npcId != npcId) continue;
                //TODO: make box animation
                Destroy(deliveryBoxesSpawned[i].gameObject);
                deliveryBoxesSpawned[i] = null;
                break;
            }
        }

        public bool HasEmptyBox()
        {
            return deliveryBoxesSpawned.Any(spawnBox => ReferenceEquals(spawnBox, null));
        }

        public BoxColor? GetUnusedBoxColor()
        {
            bool colorBlue = false, colorBrown = false, colorOrange = false, colorPink = false;
            foreach (var deliveryBox in deliveryBoxesSpawned)
            {
                if (deliveryBox != null)
                {
                    switch (deliveryBox.boxColor)
                    {
                        case BoxColor.Pink:
                            colorPink = true;
                            break;
                        case BoxColor.Orange:
                            colorOrange = true;
                            break;
                        case BoxColor.Brown:
                            colorBrown = true;
                            break;
                        case BoxColor.Blue:
                            colorBlue = true;
                            break;
                    }
                }
            }

            if (!colorBlue)
            {
                return BoxColor.Blue;
            }
            if (!colorBrown)
            {
                return BoxColor.Brown;
            }
            if (!colorOrange)
            {
                return BoxColor.Orange;
            }
            if (!colorPink)
            {
                return BoxColor.Pink;
            }

            return null;
        }
        
        

    }
}