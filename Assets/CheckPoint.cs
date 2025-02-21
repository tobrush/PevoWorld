using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject Ratio;
    public GameObject Solo;
    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<StatePlayer>().checkPoint = true;
        Ratio.SetActive(true);
        Solo.SetActive(true);
    }
}
