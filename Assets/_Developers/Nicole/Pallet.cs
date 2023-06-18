using _Developers.Vitor;
using Oversmith.Scripts.Level;
using System;
using System.Collections.Generic;
using Oversmith.Scripts.UI;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(InteractableHolder))]
public class Pallet : MonoBehaviour
{

    private bool isFull = false;
    public bool isDeliveryPlace = false;
    public Item ItemScript { get; private set; }
    // public BaseItem BaseItem { get; private set; }
    private Transform _itemTransform;

    private InteractableHolder _interactableHolder;
    [SerializeField] private Transform[] pointsToPlaceBox;
    [SerializeField] private List<GameObject> itemsPlaced = new();

    private void Awake()
    {
        _interactableHolder = GetComponent<InteractableHolder>();
    }

    public bool CanSetBox()
    {
        if (isFull)
            return false;
        else
            return true;
    }

    public bool PutOnPallet(Transform itemTransform)
    {
        var deliveryBoxScript = itemTransform.GetComponent<DeliveryBox>();
        
        int emptySpace = -1;
        for (int i = 0; i < pointsToPlaceBox.Length; i++)
        {
            if (pointsToPlaceBox[i].childCount == 0)
            {
                emptySpace = i;
                break;
            }
        }
        if (emptySpace == -1)
            return false;

        if (isDeliveryPlace)
        {
            if (deliveryBoxScript.CheckCompletion())
            {
                deliveryBoxScript.Finish();
                HudController.Instance.RemoveOrder(deliveryBoxScript.wagonName);
            }
            else
            {
                return false;
            }
        }

        //_itemTransform = itemTransform;
        itemsPlaced.Add(itemTransform.gameObject);
        itemTransform.SetParent(pointsToPlaceBox[emptySpace]);
        itemTransform.SetLocalPositionAndRotation(Vector3.zero, pointsToPlaceBox[emptySpace].localRotation);
        deliveryBoxScript.SetTrigger(true);

        return true;
    }

    public void RemoveFromPallet(Transform playerTransform, Transform boxTransform)
    {
        int boxIndex = -1;

        for (int i = 0; i < pointsToPlaceBox.Length; i++)
        {
            if (pointsToPlaceBox[i].childCount > 0 && pointsToPlaceBox[i].GetChild(0) == boxTransform)
            {
                boxIndex = i;
                break;
            }
        }
        if (boxIndex == -1)
            return;

        boxTransform.SetParent(playerTransform);
        return;
    }

    public void DestroyFromPallet(Transform boxTransform)
    {
        int boxIndex = -1;

        for (int i = 0; i < pointsToPlaceBox.Length; i++)
        {
            if (pointsToPlaceBox[i].childCount > 0 && pointsToPlaceBox[i].GetChild(0) == boxTransform)
            {
                boxIndex = i;
                break;
            }
        }
        if (boxIndex == -1)
            return;

        Destroy(boxTransform.gameObject);
    }
}
