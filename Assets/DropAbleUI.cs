using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropAbleUI : MonoBehaviour, IDropHandler
{
    private Image image;
    private RectTransform rect;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }
   
    public void OnDrop(PointerEventData eventData)
    {
        //pointerDrag = 드래그하고있는 대상
        if (eventData.pointerDrag != null)
        {

            if (this.gameObject.transform.childCount > 0)
            {
                Debug.Log("Change");
                this.gameObject.transform.GetChild(0).position = eventData.pointerDrag.GetComponent<SlotItem>().previousParent.position;
                this.gameObject.transform.GetChild(0).SetParent(eventData.pointerDrag.GetComponent<SlotItem>().previousParent);
              
                

                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
               
            }
            else
            {
                if(eventData.pointerDrag.gameObject.name == "SlotItem(Clone)")
                {
                    Debug.Log("OnDrop");
                    //드래그 하고있는 대상의 부모를 현재오브젝트로 설정, 위치를 현재오브젝트와 동일하게 설정
                    eventData.pointerDrag.transform.SetParent(transform);
                    eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
                }
               
            }
        }
    }
}
