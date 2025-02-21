using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutDoorConnector : MonoBehaviour
{
    // Start is called before the first frame update
    public void OutDoor()
    {
        Transitioner.Instance.TransitionToScene("WorldLobby");
    }

    public void ReturnHome()
    {
        Transitioner.Instance.TransitionToScene("Main");
    }

    public void PlayAR()
    {
        Transitioner.Instance.TransitionToScene("AR");
    }
}
