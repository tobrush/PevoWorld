using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FIMSpace.FLook;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Throw : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
   // public Text DebugText;

    public FLookAnimator fLookAnimator;
    public bool canThrow;
    public bool TouchMyToy;

    public Animator anim;
    public GameObject item;

    public GameObject itemsBtn;
    public GameObject stick;
    public GameObject frisbee;

    public Transform returnPoint;
    public Transform DogreturnPoint;

    Rigidbody itemRB;
    Collider itemCol;


    public NavMeshSurface navMeshPlne;

    public NavMeshAgent navMeshAgent;

    public Vector3 holdingPositionOffset;
    Vector3 holdingPos;
    public float holdingItemTargetVelocity;
    public float holdingItemMaxVelocityChange;

    public float throwSpeed;
    private Vector3 throwDir;

    Vector3 startingPos;
    Vector3 currentPos;

    public Vector3 StickPos = new Vector3(0.5f, 0.5f, 0.0f);

    public bool currentlyTouching;
    Touch currentTouch;
    public bool usingTouchInput;
    public bool usingMouseInput;

    public Camera cam;

    Vector3 TartgetDistance;
    Vector3 ReturnDistance;

    void Awake()
    {
        cam = GameObject.Find("AR Camera").GetComponent<Camera>();
        anim = navMeshAgent.transform.GetChild(int.Parse(UserData.instance.Dog1_Character) +1 ).gameObject.GetComponent<Animator>();

    }
    void Start()
    {
        itemsBtn.SetActive(true);
        itemRB = item.GetComponent<Rigidbody>();
        itemCol = item.GetComponent<Collider>();
        //navMeshPlne.GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(MakeNavMesh());

    }
    IEnumerator MakeNavMesh()
    {
       // navMeshPlne.BuildNavMesh();
       // navMeshPlne.GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(2.0f);

        fLookAnimator.ObjectToFollow = cam.transform;
    }

    void StartSwipe()
    {
        startingPos = cam.ScreenToViewportPoint(Input.GetTouch(0).position) - StickPos;
        usingTouchInput = true;
        currentlyTouching = true;
        currentTouch = Input.GetTouch(0);
  
        fLookAnimator.ObjectToFollow = item.transform;
    }

    void StartMouseDrag()
    {
        startingPos = cam.ScreenToViewportPoint(Input.mousePosition) - StickPos;
        usingMouseInput = true;
        currentlyTouching = true;

        fLookAnimator.ObjectToFollow = item.transform;

    }

    void ThrowItem()
    {
        TouchMyToy = false;
        canThrow = false;
        itemsBtn.SetActive(false);
        itemRB.velocity *= .5f;
        throwDir = (currentPos - startingPos);
        var dir = cam.transform.TransformDirection(throwDir) + cam.transform.forward;
        dir.y = 0;
        itemRB.AddForce(dir * throwSpeed, ForceMode.VelocityChange);
        StartCoroutine(DelayedThrow());
    }

    IEnumerator DelayedThrow()
    {
        yield return new WaitForSeconds(2.0f);
        
        StartCoroutine(GoGetItemGame());
        navMeshAgent.SetDestination(item.transform.position);
        anim.SetBool("Run", true);
    }

    public IEnumerator GoGetItemGame()
    {
        if (Application.isEditor)
        {
            //print("STARTING GoGetItemGame()"); 
        }
        yield return null; // remove

        //dogAgent.target = item; 
        //dogAgent.runningToItem = true; 
        navMeshAgent.SetDestination(item.transform.position);
        anim.SetBool("Run", true);
        Debug.Log("아이템 찾아라!");
        while (TartgetDistance.sqrMagnitude > 0.1f) //wait until we are close
        {
            yield return null;
        }

        PickUpItemGame();
        Debug.Log("Pick");

        while (ReturnDistance.sqrMagnitude > 0.1f) //wait until we are close
        {
            yield return null;
        }

        DropItemGame();
        anim.SetBool("Run", false);
        yield return new WaitForSeconds(1.0f);
        fLookAnimator.ObjectToFollow = cam.transform;


        if (Application.isEditor)
        {
            //print("ENDING GoGetItemGame()"); //debug
        }
    }

    public void SelectStick()
    {
        stick.SetActive(true);
        frisbee.SetActive(false);
        item = stick;
        itemRB = item.GetComponent<Rigidbody>();
        itemCol = item.GetComponent<Collider>();

    }
    public void SelectFrisbee()
    {
        stick.SetActive(false);
        frisbee.SetActive(true);
        item = frisbee;
        itemRB = item.GetComponent<Rigidbody>();
        itemCol = item.GetComponent<Collider>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    void FixedUpdate()
    {
        /*
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.tag == "DogToy")
                {
                    TouchMyToy = true;
                }
            }
        }
        */
        TartgetDistance = navMeshAgent.transform.position - item.transform.position;
        ReturnDistance = navMeshAgent.transform.position - returnPoint.transform.position;

        if (currentlyTouching)
        {
            
            if (usingTouchInput)
            {
                currentTouch = Input.GetTouch(0);

                currentPos = cam.ScreenToViewportPoint(currentTouch.position) - new Vector3(0.5f, 0.5f, 0.0f);
            }
            if (usingMouseInput)
            {
                currentPos = cam.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.0f);
            }
            holdingPos = cam.transform.TransformPoint(holdingPositionOffset + (currentPos * 2));
            Vector3 moveToPos = holdingPos - itemRB.position;  //cube needs to go to the standard Pos
            Vector3 velocityTarget = moveToPos * holdingItemTargetVelocity * Time.deltaTime; //not sure of the logic here, but it modifies velTarget
            itemRB.velocity = Vector3.MoveTowards(itemRB.velocity, velocityTarget, holdingItemMaxVelocityChange);
        }
    }

    void Update()
    {
        if (canThrow && TouchMyToy)
        {
            itemsBtn.SetActive(false);
            if (Input.touchCount > 0 && !currentlyTouching)
            {
                currentTouch = Input.GetTouch(0);
                if (currentTouch.phase == TouchPhase.Began)
                {
                    StartSwipe();
                    //DebugText.text = "Touch_StartSwipe";
                }
            }

            if (usingTouchInput && currentlyTouching)
            {
                currentTouch = Input.GetTouch(0);
                if (currentTouch.phase == TouchPhase.Ended)
                {
                    currentlyTouching = false;
                    ThrowItem();
                    // DebugText.text = "Touch_ThrowItem";
                }
            }

            if (Input.GetMouseButtonDown(0) && !currentlyTouching)
            {
                StartMouseDrag();
               // DebugText.text = "Touch_StartMouseDrag";
            }

            if (usingMouseInput && currentlyTouching)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    currentlyTouching = false;
                    ThrowItem();
                   // DebugText.text = "Touch_ThrowItem";
                }
            }
        }
    }

    public void PickUpItemGame()
    {
        itemCol.enabled = false; 
        itemRB.isKinematic = true;

        int ChaNum = int.Parse(UserData.instance.Dog1_Character);

        item.transform.position = navMeshAgent.transform.GetChild(ChaNum+1).GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).position; 
        //item.transform.rotation = navMeshAgent.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(7).rotation;
        item.transform.SetParent(navMeshAgent.transform.GetChild(ChaNum+1).GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0));
        //dogAgent.runningToItem = false; 
        // 1 - 1 - 0 -2 - 0 - 0 0 
        //dogAgent.target = returnPoint;
        //dogAgent.UpdateDirToTarget();
        //dogAgent.returningItem = true; 

        navMeshAgent.SetDestination(DogreturnPoint.transform.position);
        anim.SetBool("Run", true);
        Debug.Log("아이템 찾았으니 갖고와라");
    }

  
    public void DropItemGame()
    {
        itemRB.isKinematic = false;
        item.transform.parent = this.gameObject.transform.parent; 
        itemCol.enabled = true; 
        // dogAgent.returningItem = false; 
        canThrow = true;
        itemsBtn.SetActive(true);
    }

}

