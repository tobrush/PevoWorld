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
using UnityEngine.EventSystems;

//IBeginDragHandler, IDragHandler, IEndDragHandler,
public class SlotItem : MonoBehaviour,  IPointerClickHandler
{

    public string itemSecretCode;
    public string itemID;
    public string itemClass;
    public int itemCount;

    public Transform canvas; // ui�� �ҼӵǾ��ִ� �ֻ�� Canvas Transform
    public Transform previousParent; // �ش� ������Ʈ�� ������ �ҼӵǾ��ִ� �θ� Transform
    public RectTransform rect;  // ui ��ġ��� ���� Recttransform
    public CanvasGroup canvasGroup; // ui ���İ��� ��ȣ�ۿ� ��� ���� CanvasGroup


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform; 
        rect = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /*

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent; // �巡������ �θ� Transfrom ����
        transform.SetParent(canvas); // �θ� ������Ʈ canvas ����
        transform.SetAsLastSibling(); // ����տ� ���̵��� ������ �ڽ����� ����

        canvasGroup.alpha = 0.6f; // ���� 0.6
        canvasGroup.blocksRaycasts = false; // �����浹ó�� x
    }
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; //������ Ʈ��ŷ
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas) // �����Ѱ��� �����ٸ� ���ư�
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        canvasGroup.alpha = 1.0f; // ���� 0.1
        canvasGroup.blocksRaycasts = true; // �����浹ó�� x

      
        if(transform.parent == previousParent)

        {

            Debug.Log("Use Item");
        }
   
        
        /*
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            // ���̽��� �÷��̾�� ����ϰ� ����
            if(hit.transform.tag == "Dogs")
            {
                if(itemClass !="DogToy")
                {
                    Debug.Log("UseItem");
                    ConsumeItem();
                    Destroy(this.gameObject);
                    
                }
                else
                {
                    if(itemID == "WoodStick")
                    {
                        SaveDogData();
                        //inventorymanager - add money
                        Transitioner.Instance.TransitionToScene("TestThrow");

                    }
                    if (itemID == "WoodStick_AR")
                    {
                        SaveDogData();
                        //inventorymanager - add money
                        Transitioner.Instance.TransitionToScene("AR");
                        
                       
                    }
                }

               
            }

        }
        
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Use Item");
        if (itemClass != "DogToy")
        {
            Debug.Log("UseItem");
            ConsumeItem();
            Destroy(this.gameObject);

        }
        else
        {
            if (itemID == "WoodStick")
            {
                SaveDogData();
                //inventorymanager - add money
                Transitioner.Instance.TransitionToScene("TestThrow");

            }
            if (itemID == "WoodStick_AR")
            {
                SaveDogData();
                //inventorymanager - add money
                Transitioner.Instance.TransitionToScene("AR");


            }
        }
    }

    public void SaveDogData()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Dog1_Health", UserData.instance.Dog1_Health.ToString() }, {"Dog1_Hungry", UserData.instance.Dog1_Hungry.ToString()}, {"Dog1_Tirsty", UserData.instance.Dog1_Tirsty.ToString()}, {"Dog1_Clean",  UserData.instance.Dog1_Clean.ToString()}, {"Dog1_Happy",  UserData.instance.Dog1_Happy.ToString()},
                {"Dog2_Health", UserData.instance.Dog2_Health.ToString() }, {"Dog2_Hungry", UserData.instance.Dog2_Hungry.ToString()}, {"Dog2_Tirsty", UserData.instance.Dog2_Tirsty.ToString()}, {"Dog2_Clean",  UserData.instance.Dog2_Clean.ToString()}, {"Dog2_Happy",  UserData.instance.Dog2_Happy.ToString()},
                {"Dog3_Health", UserData.instance.Dog3_Health.ToString() }, {"Dog3_Hungry", UserData.instance.Dog3_Hungry.ToString()}, {"Dog3_Tirsty", UserData.instance.Dog3_Tirsty.ToString()}, {"Dog3_Clean",  UserData.instance.Dog3_Clean.ToString()}, {"Dog3_Happy",  UserData.instance.Dog3_Happy.ToString()}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User ������ ���� ����"); }, (error) => { });
    }



    public void ConsumeItem() // ������ ���
    {
        var request = new ConsumeItemRequest { ConsumeCount = 1, ItemInstanceId = itemSecretCode };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("������ ��� ����"); InventoryManager.Instance.GetInventory(); UserItem(); }, (error) => print("������ ��� ����"));

        if (itemID == "DogCookie") // ��Ű
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 1;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 5;
        }
        if (itemID == "NormalFood") // �Ϲݻ��
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 5;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 10;
        }
        if (itemID == "GoodFood") // ��޻��
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 10;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 20;
        }
        if (itemID == "GreatFood") // �ְ�޻��
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 20;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 30;
        }

        if (itemID == "Water") //��
        { 
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 5;
            UserData.instance.Dog1_Tirsty = UserData.instance.Dog1_Tirsty + 40;
        }

        if (itemID == "DogShampoo") // ��Ǫ
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 20;
            UserData.instance.Dog1_Clean = UserData.instance.Dog1_Clean + 50;
        }

        if (itemID == "WoodStick") // ��Ǫ
        {
            //UserData.instance.Clean = UserData.instance.Clean + 50;
            Debug.Log("WoodStick Used");
        }
        if (itemID == "WoodStick_AR") // ��Ǫ
        {
            //UserData.instance.Clean = UserData.instance.Clean + 50;
            Debug.Log("WoodStick_AR Used");
        }
    }

    public void UserItem() // ������ ���
    {
        Destroy(this);
    }
}
