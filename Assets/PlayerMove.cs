using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState { Back, Idle, Walk, Run, SuperRun, Attack, Damaged, Dead };
    public PlayerState playerState = PlayerState.Idle;

    Rigidbody rb;
    Animator anim;

    public bool superRun;

    public float moveSpeed = 1;
    public float rotateSpeed = 1;
 
    float commanded = 0;
    float commandTime = 0;
    public float commandDelay = 0.5f;

    public bool DoubleCommand()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            commanded++;
            if (commanded == 1) commandTime = Time.time;
        }
        if (commanded > 1 && Time.time - commandTime < commandDelay)
        {
            commanded = 0;
            commandTime = 0;
            return true;
        }
        else if (commanded > 2 || Time.time - commandTime > 1) commanded = 0;
        return false;
    }

    public void RandomChance()
    {
        int randomInt = Random.Range(0, 99);
        if (randomInt >= 50) // 아이템+능력치로 추가보정
        {
            superRun = true;
            Debug.Log(randomInt + " : On");
        }
        else
        {
            superRun = false;
            Debug.Log(randomInt + " : Off");
        }
    }

        void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical") * moveSpeed;
        float h = Input.GetAxis("Horizontal") * rotateSpeed;

        Vector3 move = transform.forward * v;
        Vector3 rotate = new Vector3(0, h, 0);

        rb.rotation *= Quaternion.Euler(rotate);
        rb.velocity = move;

        if (v != 0) // 연속성 호출
        {
           
        }
        else
        {
            playerState = PlayerState.Idle;
            moveSpeed = 0.5f;
            rotateSpeed = 3;
            superRun = false;
            //anim.SetBool("walk", false);
        }

        if (Input.GetKeyDown(KeyCode.W)) // 1회성 호출
        {
            if (DoubleCommand())
            {
                //anim.SetBool("run", true);
                RandomChance(); //확률성 발동조건
                if(superRun)
                {
                    playerState = PlayerState.SuperRun;
                    moveSpeed = 1.5f; //추가보정 필요
                    rotateSpeed = 0.5f; // 하락치 필요
                    //add Booster Effect
                    
                }
                else
                {
                    playerState = PlayerState.Run;
                    moveSpeed = 1f;
                    rotateSpeed = 1;
                }
            }
            else
            {
                playerState = PlayerState.Walk;
                moveSpeed = 0.5f;
                rotateSpeed = 2;
                //anim.SetBool("walk", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerState = PlayerState.Back;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //jump or 구르기
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            //attack1 short
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            //attack2 long
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //attack3 heavy
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //attack4 special
        }
    }
}
