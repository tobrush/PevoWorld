using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public void ReTurnToHome()
    {
        // update & save UserData
        Transitioner.Instance.TransitionToScene("Main");
    }
}
