using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FLook;
using DG.Tweening;

public class ItemThrow : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    private float throwSpeed;
    private float speed;
    private float lastMouseX, lastMouseY;

    [SerializeField]
    private bool thrown, holding, curve;

    private Rigidbody rigidbody;
    private Vector3 newPosition;

    [SerializeField]
    private float curveAmount = 0f, curveSpeed = 2f, minCurveAmountToCurveBall = 1f, maxCurveAmount = 2.5f;

    private Rect circlingBox;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); // 내 리지드바디야~

        rigidbody.maxAngularVelocity = curveAmount * 8f;
        circlingBox = new Rect(Screen.width / 2, Screen.height / 2, 0f, 0f);

        Reset();
        
    }

    private void Update()
    {
        if(holding)
        {
            OnTouch();
        }

        curve = (Mathf.Abs(curveAmount) > minCurveAmountToCurveBall);

        if(curve && thrown)
        {
            Vector3 direction = Vector3.right;
            direction = Camera.main.transform.InverseTransformDirection(direction);

            rigidbody.AddForce(direction * curveAmount * Time.deltaTime, ForceMode.Impulse);
            
        }
        rigidbody.maxAngularVelocity = curveAmount * 20f;
        rigidbody.angularVelocity = transform.forward * curveAmount * 8f + rigidbody.angularVelocity;

        if (thrown)
        {
            return;
        }

        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit,100f))
            {
                if(hit.transform == transform) // 레이쏴서맞은게 나면?
                {
                    holding = true;
                    transform.SetParent(null);
                }
            }
        }
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Debug.Log(lastMouseY + " / " + Input.GetTouch(0).position.y);

            if (lastMouseY <= Input.GetTouch(0).position.y)
            {
                //ThrowBall(Input.GetTouch(0).position); // auto로 바꿔도됨. Toy의 경우
                transform.DOLocalMove(target.transform.position, 1.5f);
            }
            else
            {
                Debug.Log("CancleItem");

            }
        }

        if(Input.touchCount == 1)
        {
            lastMouseX = Input.GetTouch(0).position.x;
            lastMouseY = Input.GetTouch(0).position.y;

            if(lastMouseX < circlingBox.x)
            {
                circlingBox.x = lastMouseX;
            }
            if (lastMouseX > circlingBox.xMax)
            {
                circlingBox.xMax = lastMouseX;
            }
            if (lastMouseY < circlingBox.y)
            {
                circlingBox.y = lastMouseY;
            }
            if (lastMouseY > circlingBox.yMax)
            {
                circlingBox.yMax = lastMouseY;
            }
        }
    }

    private void Reset()
    {
        transform.GetChild(0).transform.localPosition = new Vector3(0f, 0f, -5f);
        transform.GetChild(0).DOLocalMoveZ(0, 1.5f);

        this.GetComponent<TrailRenderer>().enabled = false;
        curveAmount = 0f;
        CancelInvoke();

        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, Camera.main.nearClipPlane * 30f));
        newPosition = transform.position;
        thrown = holding = false;

        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.Sleep();

        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.SetParent(Camera.main.transform);
    }
    private void OnTouch()
    {
        CalcCurveAmount();

        Vector3 mousePos = Input.GetTouch(0).position;
        mousePos.z = Camera.main.nearClipPlane * 30f;

        newPosition = Camera.main.ScreenToWorldPoint(mousePos);

        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, 50f * Time.deltaTime);

    }

    void CalcCurveAmount()
    {
        Vector2 b = new Vector2(lastMouseX, lastMouseY);
        Vector2 c = Input.GetTouch(0).position;
        Vector2 a = circlingBox.center;

        if (b == c)
        {
            return;
        }


        bool isLeft = ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0; // a = mid, b = last, c = now

        if (isLeft)
        {
            curveAmount -= Time.deltaTime * curveSpeed;
        }
        else
        {
            curveAmount += Time.deltaTime * curveSpeed;
        }

        curveAmount = Mathf.Clamp(curveAmount, -maxCurveAmount, maxCurveAmount);
    }

    private void ThrowBall(Vector2 mousePos)
    {
        this.GetComponent<TrailRenderer>().enabled = true;
        rigidbody.useGravity = true;
        float differenceY = (mousePos.y - lastMouseY) / Screen.height * 10f;
        speed = throwSpeed * differenceY;


        float x = (mousePos.x - lastMouseX) / Screen.width;
        /*
        float x = (mousePos.x / Screen.width) - (lastMouseX / Screen.width);
        x = Mathf.Abs(Input.GetTouch(0).position.x - lastMouseX) / Screen.width * 100 * x;
        Vector3 direction = new Vector3(x, 0f, 1f);
        */

        Vector3 direction = Quaternion.AngleAxis(x * 180f, Vector3.up) * new Vector3(0f, 10f, 10f);
        direction = Camera.main.transform.TransformDirection(direction);

        rigidbody.AddForce((direction * speed / 2) + (Vector3.up * speed));

        holding = false;
        thrown = true;

        Invoke("Reset", 4.0f);

    }
}

