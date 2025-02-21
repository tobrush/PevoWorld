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

    public Transform canvas; // ui가 소속되어있는 최상단 Canvas Transform
    public Transform previousParent; // 해당 오브젝트가 직전에 소속되어있던 부모 Transform
    public RectTransform rect;  // ui 위치제어를 위한 Recttransform
    public CanvasGroup canvasGroup; // ui 알파값과 상호작용 제어를 위한 CanvasGroup


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform; 
        rect = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /*

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent; // 드래그직전 부모 Transfrom 저장
        transform.SetParent(canvas); // 부모 오브젝트 canvas 변경
        transform.SetAsLastSibling(); // 가장앞에 보이도록 마지막 자식으로 설정

        canvasGroup.alpha = 0.6f; // 알파 0.6
        canvasGroup.blocksRaycasts = false; // 광선충돌처리 x
    }
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; //포지션 트래킹
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas) // 엉뚱한곳에 끌었다면 돌아가
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        canvasGroup.alpha = 1.0f; // 알파 0.1
        canvasGroup.blocksRaycasts = true; // 광선충돌처리 x

      
        if(transform.parent == previousParent)

        {

            Debug.Log("Use Item");
        }
   
        
        /*
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            // 레이쏴서 플레이어면 사용하고 제거
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
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User 데이터 저장 성공"); }, (error) => { });
    }



    public void ConsumeItem() // 아이템 사용
    {
        var request = new ConsumeItemRequest { ConsumeCount = 1, ItemInstanceId = itemSecretCode };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("아이템 사용 성공"); InventoryManager.Instance.GetInventory(); UserItem(); }, (error) => print("아이템 사용 실패"));

        if (itemID == "DogCookie") // 쿠키
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 1;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 5;
        }
        if (itemID == "NormalFood") // 일반사료
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 5;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 10;
        }
        if (itemID == "GoodFood") // 고급사료
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 10;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 20;
        }
        if (itemID == "GreatFood") // 최고급사료
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 20;
            UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry + 30;
        }

        if (itemID == "Water") //물
        { 
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 5;
            UserData.instance.Dog1_Tirsty = UserData.instance.Dog1_Tirsty + 40;
        }

        if (itemID == "DogShampoo") // 샴푸
        {
            UserData.instance.Dog1_Health = UserData.instance.Dog1_Health + 20;
            UserData.instance.Dog1_Clean = UserData.instance.Dog1_Clean + 50;
        }

        if (itemID == "WoodStick") // 샴푸
        {
            //UserData.instance.Clean = UserData.instance.Clean + 50;
            Debug.Log("WoodStick Used");
        }
        if (itemID == "WoodStick_AR") // 샴푸
        {
            //UserData.instance.Clean = UserData.instance.Clean + 50;
            Debug.Log("WoodStick_AR Used");
        }
    }

    public void UserItem() // 아이템 사용
    {
        Destroy(this);
    }
}
