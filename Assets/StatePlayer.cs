using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DogRunState { Ready, Starting, Maximum, Exhaust, LastSpurt, GoalIn }


public class StatePlayer : MonoBehaviour
{
    public GameObject DogPipe;
    public Transform DogRePos;
    public float Turnelfloat = 0;
    public bool TurnelAni;
    public bool TurnelPass;

    public Animator anim;

    public bool checkPoint;

    bool Boost = false;
    public bool Goal = false;

    public DogRunState runState;

    public Rigidbody rb;

    public float speed = 8;
    public float acceleration = 30;

    public float currentSpeed;
    public float targetSpeed;

    public float StaminaTime;

    private Vector3 dir = Vector3.right;

    public bool Restart;
    private void Start()
    {
       // rb = GetComponent<Rigidbody>();

        if (runState == DogRunState.Ready)
        {
            StartCoroutine("ReadyRun");
        }
    }

    IEnumerator ReadyRun()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Start!");
        runState = DogRunState.Starting;
    }

    private void FixedUpdate()
    {

        if (runState != DogRunState.Ready)
        {
            if (TurnelAni)
            {
                Turnelfloat = Turnelfloat + 2f;
                DogPipe.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Turnelfloat);

                if (Turnelfloat >= 80)
                {
                    if(!TurnelPass)
                    {
                        this.transform.position = DogRePos.position + new Vector3( 0,0.1f,0);
                        anim.gameObject.transform.localRotation = Quaternion.Euler(0, -90f, 0);
                        Restart = true;
                        TurnelPass = true;
                    }
                }

                if (Turnelfloat >= 100)
                {
                    Turnelfloat = 0;
                    TurnelAni = false;
                }
            }


            if (!checkPoint)
            {
                anim.SetBool("Run", true);
                this.gameObject.GetComponent<Rigidbody>().velocity = (dir * currentSpeed * 0.1f); // 달려라!
            }
            else // 체크포인트 도달후
            {

                if (Restart)
                {

                    if (!Goal)
                    {

                        this.gameObject.GetComponent<Rigidbody>().velocity = (-dir * currentSpeed * 0.1f);
                    }
                    else
                    {
                        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //세레모니
                    }
                }
                else // 체크포인트에 도달하면 리스타트가 될때까지 일단멈춰 //  리스타트가 되면 다시달려! // 그리고 골에 도달하면 멈춰!
                {
                    this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
            
   

        if(runState == DogRunState.Starting)
        {
            targetSpeed = speed;
            currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);
            
            if(targetSpeed == currentSpeed)
            {
                runState = DogRunState.Maximum;
            }
        }


        if (runState == DogRunState.Maximum)
        {
            if(!Boost)
            {
                StartCoroutine(holdMax());
                Boost = true;
            }
        }

        IEnumerator holdMax()
        {
            yield return new WaitForSeconds(StaminaTime);
            runState = DogRunState.Exhaust;
        }

        if (runState == DogRunState.Exhaust)
        {
            float dd = 6.4f;
           // currentSpeed = Mathf.Lerp(currentSpeed, dd, float);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        }
        if (runState == DogRunState.LastSpurt)
        {

        }
        if (runState == DogRunState.GoalIn)
        {
            anim.SetBool("Run", false);
        }
    }

    public float IncrementTowards(float n, float target, float a)
    {
        if (n == target) 
        {
            return n; 
        }
        else
        {        //방향 Sign -> 음수 면 - 1 양수거나 0이면 1 반환       
            float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target        
            n += a * Time.deltaTime * dir;      
            return (dir == Mathf.Sign(target - n)) ? n : target; // if n has now passed target then return target, otherwise return n 
        }

    }

}
