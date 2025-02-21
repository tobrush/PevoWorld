using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotationgLeft = false;
    private bool isRotationgRight = false;
    private bool isWalking = false;


    private void FixedUpdate()
    {
        if(isWandering == false)
        {
            StartCoroutine(Wander());
        }

        if (isRotationgRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotationgRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 4);
        int walkTime = Random.Range(1, 5);

        isWandering = true;
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1) // R
        {
            isRotationgRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotationgRight = false;
        }
        else if(rotateLorR == 2) // L
        {
            isRotationgLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotationgLeft = false;
        }
        isWandering = false;
    }
}
