using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    public TouchManager TM;

    public Transform Target;
    public GameObject Character01;
    public GameObject Character01Addon;
    public GameObject Character02;
    public GameObject Character02Addon;
    public GameObject Character03;
    public GameObject Character03Addon;
    public GameObject Character04;
    public GameObject Character04Addon;
    public GameObject Character05;
    public GameObject Character05Addon;
    public GameObject Character06;
    public GameObject Character06Addon;


    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Target = Character01.transform;
        SetTarget01Addon();
    }


    void Update()
    {
        Vector3 targetPosition = Target.TransformPoint(new Vector3(0, 0, 0));

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SetTarget01()
    {
        Target = Character01.transform;
        TM.Target = Character01.transform;
    }
      public void SetTarget01Addon()
    {
        Target = Character01Addon.transform;
        TM.Target = Character01.transform;
    }


    public void SetTarget02()
    {
        Target = Character02.transform;
        TM.Target = Character02.transform;
    }

    public void SetTarget02Addon()
    {
        Target = Character02Addon.transform;
        TM.Target = Character02.transform;
    }

    public void SetTarget03()
    {
        Target = Character03.transform;
        TM.Target = Character03.transform;
    }

    public void SetTarget03Addon()
    {
        Target = Character03Addon.transform;
        TM.Target = Character03.transform;
    }

    public void SetTarget04()
    {
        Target = Character04.transform;
        TM.Target = Character04.transform;
    }
    public void SetTarget04Addon()
    {
        Target = Character04Addon.transform;
        TM.Target = Character04.transform;
    }


    public void SetTarget05()
    {
        Target = Character05.transform;
        TM.Target = Character05.transform;
    }

    public void SetTarget05Addon()
    {
        Target = Character05Addon.transform;
        TM.Target = Character05.transform;
    }

    public void SetTarget06()
    {
        Target = Character06.transform;
        TM.Target = Character06.transform;
    }

    public void SetTarget06Addon()
    {
        Target = Character06Addon.transform;
        TM.Target = Character06.transform;
    }
}
