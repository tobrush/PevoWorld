using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetTest : MonoBehaviour
{

    public Rigidbody rb;
    public Transform target;
    public float speed = 0.1f;
    public float t = 0.1f;

    public float force;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //SetPosition : transform.position = target.position;

        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.Lerp(a, b, speed);


        //�ι�°��� : MoveTowards�� ���� ��밡�� / �����Ӹ��� ������ �ӵ��� �ִ�. // ���������Ͱ��� ������
        //transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);

        /*
        Vector3 f = target.position - transform.position;
        f = f.normalized;
        f = f * force;
        rb.AddForce(f);
        */
    }
}
