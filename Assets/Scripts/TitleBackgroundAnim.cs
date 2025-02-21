using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackgroundAnim : MonoBehaviour
{

    public float speed = 50f;
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);

    }
}
