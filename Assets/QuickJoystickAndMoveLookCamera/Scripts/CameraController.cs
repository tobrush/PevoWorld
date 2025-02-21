using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Xamin;

public class CameraController : MonoBehaviourPunCallbacks , IDragHandler,IPointerDownHandler,IPointerUpHandler, IPointerClickHandler
{

    public Transform player;
    public Texture pointerTex;

    public float piovtY = 0;
    public float distance = 5.0f; // distance from target (used with zoom)
    public float minDistance = 2f;
    public float maxDistance = 7f;
    public float zoomSpeed = 0.2f;
    public float xSpeed = 0.3f;
    public float ySpeed = 0.3f;
    public float yMinLimit = -15f;
    public float yMaxLimit = 90f;

    private Vector3 pivotOffset; // offset from target's pivot
    private float x = 0;
    private float y = 0;
    private float targetX = 0;
    private float targetY = 0;
    private float targetDistance = 0;
    private float xVelocity = 1f;
    private float yVelocity = 1f;
    private float zoomVelocity = 1f;
    private CharacterController cameraCC;
    private Transform cameraTransform;

    public int firstPointerId = -1;
    public int secondPointerId = -1;
    private Vector2 firstDragPos;
    private Vector2 secondDragPos;
    private bool isFirstZoom = true;
    private float firstDragOffset;
    //private RectTransform pointer;
    private GameObject firstPointerObj;
    private GameObject secondPointerObj;

    public NavMeshAgent playerAgent;

    public bool isMove;
    public Vector3 TargetPos; 

    public GameObject MoveTargetEffect;
    public List<GameObject> MoveTargetEffects;
    private bool MoveTargeted;

    //TouchTest
    public Text TestText;
    public bool OnDraged = false;


    public void ResetTarget()
    {
        isMove = false;
        for (int i = 0; i < MoveTargetEffects.Count; i++)
        {
            Destroy(MoveTargetEffects[i].gameObject);
        }
        MoveTargetEffects.Clear();
        MoveTargeted = false;

    }
    public void Update()
    {
            if (Vector3.Distance(player.position, TargetPos) <= 0.1f)
            {
                ResetTarget();
            }

        if (!OnDraged)
        {
            if (isMove)
            {
                if (Vector3.Distance(player.position, TargetPos) <= 3f)
                {
                    player.GetComponent<Animator>().SetBool("Walk", true);
                    player.GetComponent<Animator>().SetBool("Run", false);
                    playerAgent.speed = 1.2f;
                }
                else
                {
                    player.GetComponent<Animator>().SetBool("Run", true);
                    playerAgent.speed = 3.5f;
                }

            }
            else
            {
                player.GetComponent<Animator>().SetBool("Walk", false);
            }

        }
        
    }
    
    private void Start()
    {
            playerAgent = player.GetComponent<NavMeshAgent>();
            TargetPos = player.position;

            //Charactercontroller parameters
            cameraTransform = Camera.main.transform;
            if (!cameraTransform.GetComponent<CharacterController>())
            {
                cameraCC = cameraTransform.gameObject.AddComponent<CharacterController>();
            }
            else
            {
                cameraCC = cameraTransform.GetComponent<CharacterController>();
            }
            cameraCC.radius = 0.5f;
            cameraCC.height = 0.5f;

            //major parameter
            pivotOffset = new Vector3(0, piovtY, 0);
            Vector3 angles = cameraTransform.eulerAngles;
            targetX = x = angles.x;
            targetY = y = ClampAngle(angles.y, yMinLimit, yMaxLimit);
            targetDistance = distance;

            Canvas canvas = (Canvas)FindObjectOfType(typeof(Canvas));

            firstPointerObj = new GameObject();
            firstPointerObj.transform.parent = canvas.transform;
            RectTransform firstPointer = firstPointerObj.AddComponent<RectTransform>();
            firstPointer.gameObject.AddComponent<RawImage>().texture = pointerTex;
            firstPointer.rect.Set(0, 0, 100, 100);
            firstPointer.pivot.Set(0.5f, 0.5f);
            firstPointerObj.SetActive(false);
            firstPointer.sizeDelta = new Vector2(Screen.height, Screen.height) * 0.25f * 0.4f;

            secondPointerObj = new GameObject();
            secondPointerObj.transform.parent = canvas.transform;
            RectTransform secondPointer = secondPointerObj.AddComponent<RectTransform>();
            secondPointer.gameObject.AddComponent<RawImage>().texture = pointerTex;
            secondPointer.rect.Set(0, 0, 100, 100);
            secondPointer.pivot.Set(0.5f, 0.5f);
            secondPointerObj.SetActive(false);
            secondPointer.sizeDelta = new Vector2(Screen.height, Screen.height) * 0.25f * 0.4f;


    }

    public void OnDrag(PointerEventData eventData)
    {
        
        OnDraged = true;
        TestText.text = "OnDrag";
        //print("Ondrag");
        if (firstPointerId != -1 && secondPointerId != -1)
        {
            if (eventData.pointerId == firstPointerId)
            {
                firstDragPos = eventData.position;
                firstPointerObj.transform.position = eventData.position;
            }

            if (eventData.pointerId == secondPointerId)
            {
                secondDragPos = eventData.position;
                secondPointerObj.transform.position = eventData.position;
            }

            float tempDragOffset = Mathf.Abs(firstDragPos.x - secondDragPos.x);
            if (isFirstZoom)
            {
                firstDragOffset = tempDragOffset;
                isFirstZoom = false;
            }

            if (tempDragOffset > firstDragOffset)
            {
                targetDistance -= zoomSpeed;
                firstDragOffset = tempDragOffset;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }
            else if (tempDragOffset < firstDragOffset)
            {
                firstDragOffset = tempDragOffset;
                targetDistance += zoomSpeed;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }
        }
        else if (firstPointerId != -1 || secondPointerId != -1)
        {
            if(firstPointerId!=-1){

                firstPointerObj.transform.position = eventData.position;
            }

            if(secondPointerId!=-1){

                secondPointerObj.transform.position = eventData.position;
            }

            targetX += eventData.delta.x * xSpeed;
            targetY -= eventData.delta.y * ySpeed;

            targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
        }


    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle= angle < -360 ? angle + 360 : angle;
        angle = angle > 360 ? angle - 360 : angle;
        return Mathf.Clamp(angle, min, max);
    }

    public void CameraSet()
    {
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f);
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.3f);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + player.position + pivotOffset;
        cameraTransform.rotation = rotation;

        if (position != cameraTransform.position)
        {
            cameraCC.Move(position - cameraTransform.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (firstPointerId == -1)
        {
            firstPointerId = eventData.pointerId;
            firstPointerObj.SetActive(true);
            firstPointerObj.transform.position = eventData.position;
            //print("bd=" + eventData.position);
        }
        else if (secondPointerId == -1)
        {
            secondPointerId = eventData.pointerId;
            secondPointerObj.SetActive(true);
            secondPointerObj.transform.position = eventData.position;
        }
        //print("pointerDrown=" + eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == firstPointerId)
        {
            firstPointerId = -1;
            firstPointerObj.SetActive(false);
        }

        if (eventData.pointerId == secondPointerId)
        {
            secondPointerId = -1;
            secondPointerObj.SetActive(false);
        }

        StartCoroutine(Dealay());
      
        //print("pointerUp=" + eventData.position);
    }


    IEnumerator Dealay()
    {
        yield return new WaitForSeconds(0.1f);
        OnDraged = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!OnDraged)
        {

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);

            RaycastHit hit;

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인

            {
                if (hit.collider.tag == "Player")
                {
                    print(hit.collider.name);
                    TestText.text = "<color=Red>Touch Player</color>";
                }

                if (hit.collider.tag == "Ground")
                {
                    if (player.position != hit.point)
                    {
                        print(hit.collider.name);
                        playerAgent.SetDestination(hit.point);
                        isMove = true;
                        TargetPos = hit.point;
                        TestText.text = "<color=Yellow>Touch Ground</color>";

                        if (!MoveTargeted)
                        {
                            MoveTargetEffects.Add((GameObject)Instantiate(MoveTargetEffect, new Vector3(hit.point.x, 0.1f, hit.point.z), Quaternion.identity));
                            MoveTargeted = true;
                        }
                        else
                        {
                            MoveTargetEffects[0].transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
                        }
                    }
                }
            }
        }
    }
}
