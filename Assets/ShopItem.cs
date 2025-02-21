using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.ComponentModel;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro.Examples;
using Newtonsoft.Json;

public class ShopItem : MonoBehaviour
{
    public string catalogVersion = "Shop";
    public string itemId;
    public string virtualCurrency = "PC";
    public int price;

    public void PurchaseItem() // �����۱���
    {
        var request = new PurchaseItemRequest()
        {
            CatalogVersion = "Shop",
            ItemId = itemId,
            VirtualCurrency = virtualCurrency,
            Price = price
        };
        PlayFabClientAPI.PurchaseItem(request, (result) => { print("������ ���� ����"); InventoryManager.Instance.GetInventory(); }, (error) => print("������ ���� ����")); ;
    }

    
}
