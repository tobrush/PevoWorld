using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System;
using TMPro;

[Serializable]
public class MailData
{
    public int Sort;
    public string To;
    public string ToID;
    public int Index;
    public string MailName;
    public string MailDesc;
    public string ItemID;
    public string ItemAmount;
    public string GetItem;
    public string ExpireTime;

}

public class MailBoxManager : MonoBehaviour
{

    public Image MailInItemImage;
    public Sprite[] itemSprite;

    public List<MailData> mailDatas = new List<MailData>();

    public Button AllGetButton;

    public int ItemAll;
    public int ItemReady;

    public GameObject OpenBox;
    public Transform MailList;
    public GameObject MailItem;

    public Button RemoveButton;
    public Button GetButton;
    public GameObject Block;

    public int index;

    public GameObject emptyItem;

    public List<string> itemIdss;

    public void Start()
    {
        StartCoroutine("ResetList");
    }

    IEnumerator ResetList()
    {
        yield return new WaitForSeconds(30f);

        //Debug.Log("ResetList");
        UpdateInboxListToClient(UserData.instance.User_Inbox);
        StartCoroutine("ResetList");

    }


    public void AlreadyGet()
    {
        RemoveButton.gameObject.SetActive(true);
        GetButton.interactable = false;
        Block.SetActive(true);

    }

    public void NewGet()
    {
        RemoveButton.gameObject.SetActive(false);
        GetButton.interactable = true;
        Block.SetActive(false);
    }

    void GetItem(int index)
    {
        Debug.Log("item Get ya!!!");
        int i = index;
        if (mailDatas[i].Sort == 0)
        {
            if (mailDatas[i].GetItem == "0")
            {
                string GetiD = mailDatas[i].ItemID;
                int amount = int.Parse(mailDatas[i].ItemAmount);

                ClaimItem(GetiD, amount);
            }
        }
    }

    void GetAllItem()
    {
        Debug.Log("All item Get ya!!!");

        for (int i = 0; i < mailDatas.Count; i++)
        {
            if (mailDatas[i].Sort == 0)
            {
                if (mailDatas[i].GetItem == "0")
                {
                    string GetiD = mailDatas[i].ItemID;
                    int amount = int.Parse(mailDatas[i].ItemAmount);


                    ClaimItem(GetiD, amount);
                }
            }
        }
        
    }



    public void RequestGetItemInbox()  // ???????????? ???? ?????? ???? ?????? ???? (???? ?????? ????)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetInbox",
            FunctionParameter = new { Index = index }, // FJsonKeeper(JsonObject)
            GeneratePlayStreamEvent = true,
        }, (result) => { GetItem(index); OnInboxRefreshSuccess(result); }, (error) => OnFailed(error));

       


    }

    public void RequestGetAllItemInbox()  // ???????????? ???? ?????? ???? ?????? ???? (???? ?????? ????)
    {

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetAllInbox",
            GeneratePlayStreamEvent = true,
        }, (result) => { GetAllItem(); OnInboxRefreshSuccess(result); }, (error) => OnFailed(error));

        
    }



    public void RequestClaimInbox()  // ???????????? ???? ?????? ???? ?????? ???? (????????)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "ClaimInbox",
            FunctionParameter = new { Index = index }, // FJsonKeeper(JsonObject)
            GeneratePlayStreamEvent = true,  
        }, OnInboxRefreshSuccess, (error) => print("????"));
    }

    public void RequestClaimAllInbox()  //???? ?????? ???? null (???? ????)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "ClaimAllInbox",
            GeneratePlayStreamEvent = true, 
        }, OnInboxRefreshSuccess, (error) => print("????"));
    }

    public void RequestInboxList() // ? 1???? ????? ?? (????????)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "RefreshInbox",
            GeneratePlayStreamEvent = true,
        }, OnInboxRefreshSuccess, (error) => OnFailed(error)) ;
    }

    public void OnFailed(PlayFabError error)
    {
        Debug.Log(error);
    }


    public void OnInboxRefreshSuccess(ExecuteCloudScriptResult result)
    {
        

        if (result.FunctionResult != null)
        {
            string resultData = result.FunctionResult.ToString();

            UserData.instance.User_Inbox = resultData;

            UpdateInboxListToClient(resultData);
        }
    }


    public void UpdateInboxListToClient(string result)
    {

        // ?????????? ????
        for (int i = 0; i < MailList.childCount; i++)
        {
            Destroy(MailList.GetChild(i).gameObject);
        }

        // ???? ????
        if (result != "" && result != null)
        {
            mailDatas = JsonConvert.DeserializeObject<List<MailData>>(result);
            
            //JsonData MailData = JsonMapper.ToObject(result);
            ItemAll = mailDatas.Count;
            ItemReady = 0;

            for (int i = 0; i < mailDatas.Count; i++)
            {
                mailDatas[i].Index = i;

                GameObject item = Instantiate(MailItem, MailList);
                item.GetComponent<Mail_Item>().Index = i;
                item.GetComponent<Mail_Item>().Sort = mailDatas[i].Sort;
                item.GetComponent<Mail_Item>().To = mailDatas[i].To.ToString();
                item.GetComponent<Mail_Item>().ToID = mailDatas[i].ToID.ToString();
                item.GetComponent<Mail_Item>().MailName = mailDatas[i].MailName.ToString();
                item.GetComponent<Mail_Item>().MailDesc = mailDatas[i].MailDesc.ToString();
                item.GetComponent<Mail_Item>().ItemID = mailDatas[i].ItemID.ToString();
                item.GetComponent<Mail_Item>().ItemAmount = mailDatas[i].ItemAmount.ToString();
                item.GetComponent<Mail_Item>().GetItem = mailDatas[i].GetItem.ToString();
                item.GetComponent<Mail_Item>().ExpireTime = mailDatas[i].ExpireTime.ToString();


                item.transform.GetChild(0).GetComponent<TMP_Text>().text = mailDatas[i].MailName.ToString();
                item.transform.GetChild(1).GetComponent<TMP_Text>().text = mailDatas[i].ExpireTime.ToString();
                item.transform.GetChild(2).GetComponent<TMP_Text>().text = mailDatas[i].To.ToString();
                
                

                if(mailDatas[i].GetItem == "1")
                {
                    ItemReady += 1;
                }
            }
        }
        else
        {
            GameObject EmptyItem = Instantiate(emptyItem, MailList); // ???????? ?????? ????
        }

        if (ItemAll == ItemReady)
        {
            AllGetButton.interactable = false;
        }
        else
        {
            AllGetButton.interactable = true;
        }
    }



    void ClaimItem(string itemID, int amount) 
    {
        if (itemID == "PC")
        {

            var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "PC", Amount = amount };
            PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("?? ???? ????! ???? ?? : " + result.Balance); InventoryManager.Instance.DisplayPC(result.Balance.ToString()); }, (error) => print("?? ???? ????"));

            //AddItem(amount);
            Debug.Log(itemID + " : " + amount);
        }
        else if (itemID == "PG")
        {

            var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "PG", Amount = amount };
            PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("?? ???? ????! ???? ?? : " + result.Balance); InventoryManager.Instance.DisplayPG(result.Balance.ToString()); }, (error) => print("?? ???? ????"));

            //AddItem(amount);
            Debug.Log(itemID + " : " + amount);
        }
        else if (itemID == "TestItemBox")
        {
            itemIdss.Add("TestItemBox");
            GiveItemToUser("Shop", itemIdss);
        }
        else if (itemID == "WT")
        {
            //AddItem(amount);
        }
        else if (itemID == "PT")
        {
            //AddItem(amount);
        }
        else if (itemID == "SS")
        {
            //AddItem(amount);
        }
    }
    
    public void GiveItemToUser(string catalogversion, List<string> itemIds)
    {
        PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest
        {
            CatalogVersion = catalogversion,
            PlayFabId = UserData.instance.User_MyID,
            ItemIds = itemIds
        };
        PlayFabServerAPI.GrantItemsToUser(request, result => { InventoryManager.Instance.GetInventory(); }, (error) => print("????"));
    }
    
}
