﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_WPIDEMO : MonoBehaviour
{
    //This script is attached to all four of the spawnable game objects
    //It detects if the player has pressed "r" to reset
    //If so, this game object destroys itself


    void OnEnable()
    {
        event_manager_WPIDEMO.onStartLevel += ClearGameObjects;
    }

    void OnDisable()
    {
        event_manager_WPIDEMO.onStartLevel -= ClearGameObjects;
    }



    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                CapsuleCollider bc = hit.collider as CapsuleCollider;
                if(bc!=null)
                {
                    Destroy(bc.gameObject);
                }
                
            }
        }

        if (Input.GetKeyDown("r"))
        {
            scene_manager_WPIDEMO.spawnCount = 0;
            Destroy(gameObject);
        }
    }

    void ClearGameObjects()
    {
        scene_manager_WPIDEMO.spawnCount = 0;
        if (scene_manager_WPIDEMO.curLevel != 3)
        {
            Destroy(gameObject);
        }
    }
}
