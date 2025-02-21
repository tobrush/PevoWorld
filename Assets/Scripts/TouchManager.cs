using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Touch CurrentTouch;
    public Camera MainCam;

    public Transform Target;
    public GameObject Hand;
    public bool handOn = false;
    bool bIsPress = false;
    public float faccumTime = 0f;

    public Vector3 ChangeHit;
    public Vector3 HitPoint;

    public bool isBtnDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        isBtnDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isBtnDown = false;
    }


    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
             
                if(hit.collider.tag == "Dogs")
                {
                    //Debug.Log(hit);

                    Vector3 handRoteate = hit.normal;
                    if (!handOn)
                    {
                        Instantiate(Hand, hit.point, Quaternion.LookRotation(handRoteate), transform);
                        handOn = true;
                        bIsPress = true;
                    }
                    GameObject.Find("CartoonHand(Clone)").transform.position = hit.point;
                    GameObject.Find("CartoonHand(Clone)").transform.rotation = Quaternion.LookRotation(handRoteate);
  
                    Debug.DrawLine(Target.position, hit.point, Color.red);
                    Debug.DrawRay(hit.point, hit.normal, Color.green);

                    HitPoint = hit.point;
                    StartCoroutine("SameHit");

                }
            }

          


            if (bIsPress == true) // ´­·µÀ»¶§
            {
                if (HitPoint != ChangeHit)
                {
                    faccumTime += Time.deltaTime;
                }

                if (faccumTime >= 0.5f)
                {
                    Debug.Log("Have Love");

                    UserData.instance.Dog1_Happy = UserData.instance.Dog1_Happy + 5;                // dog add
                    //Target.GetComponent<Animator>().SetTrigger("Feel");
                    faccumTime = 0f;
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            
            bIsPress = false;
            Destroy(GameObject.Find("CartoonHand(Clone)"));
            handOn = false;
            StopCoroutine("SameHit");
            faccumTime = 0f;
        }


        if (Input.touchCount > 0)
        {

            CurrentTouch = Input.GetTouch(0);

            RaycastHit hit;
            Ray ray = MainCam.ScreenPointToRay(CurrentTouch.position);

            if (Physics.Raycast(ray, out hit))
            {
             
                if(hit.collider.tag == "Dogs")
                {
                    Debug.Log(hit);

                    Vector3 handRoteate = hit.normal;
                    if (!handOn)
                    {
                        Instantiate(Hand, hit.point, Quaternion.LookRotation(handRoteate), transform);
                        handOn = true;
                        bIsPress = true;
                    }
                    GameObject.Find("CartoonHand(Clone)").transform.position = hit.point;
                    GameObject.Find("CartoonHand(Clone)").transform.rotation = Quaternion.LookRotation(handRoteate);
  
                    Debug.DrawLine(Target.position, hit.point, Color.red);
                    Debug.DrawRay(hit.point, hit.normal, Color.green);

                    HitPoint = hit.point;
                    StartCoroutine("SameHit");

                }
            }

          


            if (bIsPress == true) // ´­·µÀ»¶§
            {
                if (HitPoint != ChangeHit)
                {
                    faccumTime += Time.deltaTime;
                }

                if (faccumTime >= 0.5f)
                {
                    Debug.Log("Have Love");

                    // UserData.instance.Happy = UserData.instance.Happy + 5;                        // dog add
                    //Target.GetComponent<Animator>().SetTrigger("Feel");
                    faccumTime = 0f;
                }
            }
        }
        if(CurrentTouch.phase == TouchPhase.Ended)
        {
            bIsPress = false;
            Destroy(GameObject.Find("CartoonHand(Clone)"));
            handOn = false;
            StopCoroutine("SameHit");
            faccumTime = 0f;
        }
    }

    IEnumerator SameHit()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeHit = HitPoint;
    }




}