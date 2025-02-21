using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]

public class HeadIK : MonoBehaviour
{
    protected Animator animator;
    public bool ikActive = false;
    public Transform lookObj = null;
    public float lookWeight = 2f;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(lookWeight);
                    animator.SetLookAtPosition(lookObj.position);
                }

            }
            else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}