using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<StatePlayer>().checkPoint == true)
        {
            other.GetComponent<StatePlayer>().Goal = true;
            other.GetComponent<StatePlayer>().runState = DogRunState.GoalIn;
            
        }
        
    }
}
