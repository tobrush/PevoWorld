using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.ComponentModel;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;


[Serializable]
public class GameItem
{
    public string ItemName;
    public string ItemClass;
    public string ItemDesc;
    public string ItemUnit;
    public int ItemPrice;
    public string ItemID;
    public int ItemCount;
    public Sprite ItemSprite;
}


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
   

    public List<Sprite> SpriteList;

    public List<GameItem> ItemList = new List<GameItem>();

    public List<GameObject> InventorySlot;
    public List<GameObject> InventorySlotItem;
    public GameObject SlotItem;
    public Transform InvenList;
    [SerializeField]
    GameObject ball;


    public Camera Cam;

    //public TapManager tapManager;
    //public GameObject invenPanel;
    //public GameObject setbar;

    //public GameObject WorldBtn;
    public Transform SpawnPos;

    public TMP_Text PCtxt0;
    public TMP_Text PCtxt1;

    public TMP_Text PGtxt0;
    public TMP_Text PGtxt1;


    public GameObject ShopItem;
    public Transform ShopItemPos;
    bool CadalogLoad = false;

    public void Awake()
    {
        Instance = this;
    }

    public void SpawnBall()
    {
        Instantiate(ball, SpawnPos.position, Quaternion.identity);

        //tapManager.HidePage();
        Cam.GetComponent<TMPro.Examples.CameraController>().enabled = false;
    }

    public void HideInven()
    {
        //WorldBtn.SetActive(false);
       // invenPanel.SetActive(false);
       // setbar.SetActive(false);

      //  tapManager.HidePage();

        Debug.Log("hide Inven");
    }


    public void GetInventory() 
    {
        if (InventorySlotItem.Count > 0)
        {
            for (int i = 0; i < InventorySlotItem.Count; i++)
            {
                Destroy(InventorySlotItem[i]);
            }
        }
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
        {
            
            print("현재금액 : " + result.VirtualCurrency["PC"] + " / " + result.VirtualCurrency["PG"]);
            DisplayPC(result.VirtualCurrency["PC"].ToString());
            DisplayPG(result.VirtualCurrency["PG"].ToString());

            ItemList.Clear();

            for (int i = 0; i < result.Inventory.Count; i++)
            {
                var inven = result.Inventory[i];
                GameItem gameitem = new GameItem();
                gameitem.ItemName = inven.DisplayName;  //이름
                gameitem.ItemClass = inven.ItemClass;   // 클래스
                gameitem.ItemDesc = "";  // inven.Annotation;   // 설명  // 주석이였음.
                gameitem.ItemUnit = inven.UnitCurrency;   // 화폐단위
                gameitem.ItemPrice = int.Parse(inven.UnitPrice.ToString()); //가격
                gameitem.ItemID = inven.ItemInstanceId;  // 아이템 고유코드
                gameitem.ItemCount = inven.RemainingUses.Value; //  갯수
                
                if (inven.ItemId == "DogCookie")
                {
                    gameitem.ItemSprite = SpriteList[0];
                }
                if (inven.ItemId == "GoodFood")
                {
                    gameitem.ItemSprite = SpriteList[2];
                }
                if (inven.ItemId == "GreatFood")
                {
                    gameitem.ItemSprite = SpriteList[3];
                }
                if (inven.ItemId == "NormalFood")
                {
                    gameitem.ItemSprite = SpriteList[1];
                }
                if (inven.ItemId == "Water")
                {
                    gameitem.ItemSprite = SpriteList[4];
                }
                if (inven.ItemId == "DogShampoo")
                {
                    gameitem.ItemSprite = SpriteList[5];
                }
                if (inven.ItemId == "WoodStick")
                {
                    gameitem.ItemSprite = SpriteList[6];
                }
                if (inven.ItemId == "WoodStick_AR")
                {
                    gameitem.ItemSprite = SpriteList[7];
                }
                if (inven.ItemId == "TestItemBox")
                {
                    gameitem.ItemSprite = SpriteList[8];
                }

                ItemList.Add(gameitem);
                //print(inven.DisplayName + " / " + inven.UnitCurrency + " / " + inven.UnitPrice + " / " + inven.ItemInstanceId + " / " + inven.RemainingUses);

                var InvenItem = Instantiate(SlotItem, InvenList);
                InvenItem.transform.GetChild(0).GetComponent<TMP_Text>().text = inven.DisplayName;
                InvenItem.transform.GetChild(1).GetComponent<TMP_Text>().text = "";  //inven.Annotation;
                InvenItem.transform.GetChild(2).GetComponent<Image>().sprite = gameitem.ItemSprite;
                InvenItem.transform.GetChild(3).GetComponent<TMP_Text>().text = "X "+ gameitem.ItemCount.ToString();

                InvenItem.GetComponent<SlotItem>().itemID = inven.ItemId;
                InvenItem.GetComponent<SlotItem>().itemSecretCode = gameitem.ItemID;
                InvenItem.GetComponent<SlotItem>().itemClass = gameitem.ItemClass;
                InvenItem.GetComponent<SlotItem>().itemCount = gameitem.ItemCount;
                //InvenItem.GetComponent<ObjectSettings>().Id = inven.ItemId;
                //DDM.AllObjects = new ObjectSettings[DDM.AllObjects.Length + 1];
                //DDM.AllObjects[i] = InvenItem.GetComponent<ObjectSettings>();

                InventorySlotItem.Add(InvenItem);
                //InvenItem.GetComponent<Image>().sprite = gameitem.ItemSprite;
                //InvenItem.transform.GetChild(0).GetComponent<Text>().text = gameitem.ItemCount.ToString();
            }



        }, (error) => print("인벤토리 불러오기 실패"));
    }


    public void GetCatalogItem() //상품가져오기
    {
        if(!CadalogLoad)
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = "Shop" }, (result) =>
            {
                for (int i = 0; i < result.Catalog.Count; i++)
                {
                    var Catalog = result.Catalog[i];
                    var ItemObj = Instantiate(ShopItem, ShopItemPos);

                    if (Catalog.ItemId == "DogCookie")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[0];
                    }
                    if (Catalog.ItemId == "GoodFood")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[2];
                    }
                    if (Catalog.ItemId == "GreatFood")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[3];
                    }
                    if (Catalog.ItemId == "NormalFood")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[1];
                    }
                    if (Catalog.ItemId == "DogShampoo")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[5];
                    }
                    if (Catalog.ItemId == "Water")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[4];
                    }
                    if (Catalog.ItemId == "TestItemBox")
                    {
                        ItemObj.transform.GetChild(1).GetComponent<Image>().sprite = SpriteList[8];
                    }

                    ItemObj.GetComponent<ShopItem>().catalogVersion = Catalog.CatalogVersion;
                    ItemObj.GetComponent<ShopItem>().itemId = Catalog.ItemId;
                    ItemObj.GetComponent<ShopItem>().virtualCurrency = "PC";
                    var buyPrice = Catalog.VirtualCurrencyPrices["PC"];
                    ItemObj.GetComponent<ShopItem>().price = int.Parse(buyPrice.ToString());



                    ItemObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = Catalog.DisplayName;
                    ItemObj.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>().text = Catalog.VirtualCurrencyPrices["PC"].ToString();
                    ItemObj.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = Catalog.Description;
                    //print(Catalog.ItemId + " / " + Catalog.DisplayName + " / " + Catalog.Description + " / " + Catalog.VirtualCurrencyPrices["PC"] + " / " + Catalog.Consumable.UsageCount);

                    CadalogLoad = true;
                }


                ShopItemPos.GetComponent<RectTransform>().sizeDelta = new Vector2(ShopItemPos.GetComponent<RectTransform>().sizeDelta.x, result.Catalog.Count * 900);
            },
                   (error) => print("상점 불러오기 실패"));
        }
       
    }


    public void PurchaseItem() // 아이템구매
    {
        var request = new PurchaseItemRequest() { 
            CatalogVersion = "Shop", 
            ItemId = "DogCookie", 
            VirtualCurrency = "PC", 
            Price = 5
        };
        PlayFabClientAPI.PurchaseItem(request, (result) => { print("아이템 구입 성공"); GetInventory(); }, (error) => print("아이템 구입 실패"));;
    }


    public void ConsumeItem() // 아이템 사용
    {
        var request = new ConsumeItemRequest { ConsumeCount = 1, ItemInstanceId = ItemList[0].ItemID };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("아이템 사용 성공"); GetInventory(); }, (error) => print("아이템 사용 실패"));
        // 아이템 효과

        if (ItemList[0].ItemCount > 1)
        {
            ItemList[0].ItemCount = ItemList[0].ItemCount - 1;
        }
        else
        {
            ItemList.Remove(ItemList[0]);
        }
    }


    public void AddMoney() // 돈추가
    {
        //UserData.instance.Happy = UserData.instance.Happy + 20;
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "PC", Amount = 100 };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("돈 얻기 성공! 현재 돈 : " + result.Balance); DisplayPC(result.Balance.ToString()); }, (error) => print("돈 얻기 실패"));
    }

    public void SubtractMoney() // 돈삭제
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "PC", Amount = 50 };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) => { print("돈 빼기 성공! 현재 돈 : " + result.Balance); DisplayPC(result.Balance.ToString()); }, (error) => print("돈 빼기 실패"));;
    }

    public void DisplayPC(string PC)
    {
        PCtxt0.text = PC;
        PCtxt1.text = PC;

    }

    public void DisplayPG(string PG)
    {
        PGtxt0.text = PG;
        PGtxt1.text = PG;
    }
}



