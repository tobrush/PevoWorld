using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleScrollSnap;

public class PageSwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public ScrollRect scrollView;
    public SimpleScrollSnap scrollSnap;

    bool forParent;

    private Vector2 StartPos;

    public void Start()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        scrollSnap.OnPointerDown(eventData);
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        scrollSnap.OnPointerUp(eventData);
       
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y); // Mathf.Abs = Àý´ë°ª.
        if (forParent)
        {
            scrollView.OnBeginDrag(eventData);
            scrollSnap.OnBeginDrag(eventData);
        }
        
        //StartPos = eventData.position;


    }

    public void OnDrag(PointerEventData eventData)
    {
        /*
        Vector2 mousePos = eventData.position;
        Vector2 localPos = StartPos - mousePos;
        Vector2 ResultPos;
        Vector2 ChangePos = new Vector2 (0, 0);

        if (localPos.x * localPos.x >= localPos.y * localPos.y)
        {
            //ResultPos = new Vector2(localPos.x, 0);
            ChangePos = new Vector2(eventData.position.x, StartPos.y);
            this.GetComponent<ScrollRect>().vertical = false;
        }
        if (localPos.x * localPos.x < localPos.y * localPos.y)
        {
            //ResultPos = new Vector2(0, localPos.y);
            this.GetComponent<ScrollRect>().vertical = true;
            ChangePos = new Vector2(StartPos.x, eventData.position.y);
            
        }

        eventData.position = ChangePos;

        */
        if (forParent)
        {
            scrollView.OnDrag(eventData);
            scrollSnap.OnDrag(eventData);
            this.GetComponent<ScrollRect>().vertical = false;
        }
        else
        {
            this.GetComponent<ScrollRect>().vertical = true;
        }

          
        //Debug.Log(eventData.position);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            scrollView.OnEndDrag(eventData);
            scrollSnap.OnEndDrag(eventData);
            
        }
        else
        {
            this.GetComponent<ScrollRect>().vertical = true;
        }

    }
}
