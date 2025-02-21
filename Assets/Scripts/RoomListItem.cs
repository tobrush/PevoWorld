using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListItem : MonoBehaviour
{

    public TMP_Text RoomInfoText;
    public RoomInfo _roomInfo;

    //public TMP_Text userIdText;

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            //ex : room_03 (1/2)
            RoomInfoText.text = $"{_roomInfo.Name}({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => onEnterRoom(RoomInfo.Name));
        }
    }


    private void Awake()
    {
        //RoomInfoText = GetComponentInChildren<TMP_Text>();
        //userIdText.text = "user88888";//GameObject.Find().GetComponent<Text>(); 

    }

    void onEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 8;

        //PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }

    public void SetUp(RoomInfo _info)
    {
        _roomInfo = _info;
        RoomInfoText.text = $"{_roomInfo.Name}({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
        gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => onEnterRoom(RoomInfo.Name));
    }


    /*
    [SerializeField] TMP_Text text;

    RoomInfo info;

    
    
    public void OnClick()
    {
       PhotonTestManager.Instance.JoinRoom(info);
    }
    */
}
