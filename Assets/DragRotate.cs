using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    protected Vector2 FirstEvent;
    public GameObject RotateTrigger;
    public float LastEvent;
    private Vector2 delta;

    void Start()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        FirstEvent = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        delta = eventData.position - FirstEvent;
        RotateTrigger.transform.localRotation = Quaternion.Euler(0, delta.x * 0.1f + LastEvent, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        LastEvent = delta.x * 0.1f + LastEvent;
    }

}
