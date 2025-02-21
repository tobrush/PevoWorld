using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayTouchManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Touch CurrentTouch;

    public Throw throwManager;
    public Camera ARCam;

    public Transform Target;
    public GameObject Hand;
    public bool handOn = false;
    bool bIsPress = false;
    public float faccumTirme = 0f;

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

    void Awake()
    {
        Target.GetChild(int.Parse(UserData.instance.Dog1_Character) + 1).gameObject.SetActive(true);

        ARCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }


    void Update()
    {
        if (throwManager.canThrow && !throwManager.TouchMyToy)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = ARCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.tag == "Dogs")
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
                    if (hit.collider.tag == "DogToy")
                    {
                        throwManager.TouchMyToy = true;
                    }
                }




                if (bIsPress == true) // ´­·µÀ»¶§
                {
                    if (HitPoint != ChangeHit)
                    {
                        faccumTirme += Time.deltaTime;
                    }

                    if (faccumTirme >= 0.5f)
                    {
                        Debug.Log("Have Love");
                        //Target.GetChild(int.Parse(UserData.instance.Dog1_Character) + 1).GetComponent<Animator>().SetTrigger("Feel");
                        faccumTirme = 0f;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                bIsPress = false;
                Destroy(GameObject.Find("CartoonHand(Clone)"));
                handOn = false;
                StopCoroutine("SameHit");
                faccumTirme = 0f;
            }

            
            if (Input.touchCount > 0)
            {

                CurrentTouch = Input.GetTouch(0);

                RaycastHit hit;
                Ray ray = ARCam.ScreenPointToRay(CurrentTouch.position) ;

                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.tag == "Dogs")
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
                    if (hit.collider.tag == "DogToy")
                    {
                        throwManager.TouchMyToy = true;
                    }
                }




                if (bIsPress == true) // ´­·µÀ»¶§
                {
                    if (HitPoint != ChangeHit)
                    {
                        faccumTirme += Time.deltaTime;
                    }

                    if (faccumTirme >= 0.5f)
                    {
                        Debug.Log("Have Love");
                        //Target.GetComponent<Animator>().SetTrigger("Feel");
                        faccumTirme = 0f;
                    }
                }
            }
            if (CurrentTouch.phase == TouchPhase.Ended)
            {
                bIsPress = false;
                Destroy(GameObject.Find("CartoonHand(Clone)"));
                handOn = false;
                StopCoroutine("SameHit");
                faccumTirme = 0f;
            }
        }
    }
    IEnumerator SameHit()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeHit = HitPoint;
    }




}