using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWindow : MonoBehaviour
{
    public GameObject ChatWindows;
    public bool ChatBtn;
    public void ChatWindowOnOff()
    {
        if(!ChatBtn)
        {
            ChatWindows.SetActive(true);
            ChatBtn = true;
        }
        else
        {
            ChatWindows.SetActive(false);
            ChatBtn = false;
        }
    }
    
}
