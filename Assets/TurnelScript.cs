using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnelScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
       
            other.GetComponent<StatePlayer>().TurnelAni = true;
        

    }
}
