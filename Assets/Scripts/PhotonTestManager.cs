using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonTestManager : MonoBehaviourPunCallbacks
{
    public static PhotonTestManager Instance;

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    public string UserID;
    public string nick;
    public string CharacterNumber;
    public UserData userData;

    public ConfigManager configManager;
    public FriendsManager friendsManager;
    public MailBoxManager mailBoxManager;

    public bool RoomFilted;
    public InputField roomFilterName;
    public Dropdown DW;

   // byte maxPlayers = byte.Parse(m_dropdown_RoomMaxPlayers.options[m_dropdown_RoomMaxPlayers.value].text); // ???????????? ?? ????????.
   // byte maxTime = byte.Parse(m_dropdown_MaxTime.options[m_dropdown_MaxTime.value].text);


    [SerializeField] TMP_InputField roomNameInputField;

    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] TMP_Text Countui;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;

    public GameObject LoadingPanel;
    public Button BackBtn, QuickBtn, MakeBtn;
    public TMP_InputField password;

    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;
        //userData = GameObject.Find("UserData").GetComponent<UserData>();
        



    }


    public void CreateRoomPassword(bool Pass)
    {
        password.interactable = Pass;
        /*
        if (Pass)
        {
            password.interactable = false;
        }
        else
        {
            password.interactable = true;
        }*/
    }

    


    void Start()
    {

        UserID = UserData.instance.User_MyID;
        nick = UserData.instance.User_Name;
        CharacterNumber = UserData.instance.Dog1_Character; // or 2 3

                                                            //configManager.myID.text = UserID;
                                                            // configManager.MyName.text = nick;

        // friendsManager.MyDisplayName = userData.User_Name;
        // friendsManager.MyID = userData.User_MyID;

        // mailBoxManager.UpdateInboxListToClient(userData.User_Inbox);

        ConnectPhoton();
        MenuManager.Instance.OpenMenu("Loading");//?????? ????
        StartCoroutine(LoadingTime());
    }
    
    IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(2.0f);
        LoadingPanel.SetActive(false);
    }
   
    public void ConnectPhoton()
    {
        AuthenticationValues authValues = new AuthenticationValues(UserID);
        authValues.UserId = UserID;
        PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.GameVersion = "1.0"; // ????????
        PhotonNetwork.AutomaticallySyncScene = true; // ??????
        PhotonNetwork.NickName = nick; //??????

        
        _myCustomProperties["PlayfabID"] = UserID;
        _myCustomProperties["CharacterNumber"] = CharacterNumber;

        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;

        //PhotonNetwork.LocalPlayer.PlayfabID = UserID;
        PhotonNetwork.ConnectUsingSettings(); //?????? ???? ????
    }


    public override void OnConnectedToMaster() // ?????? ?????? ?????? ????
    {
        base.OnConnectedToMaster();
        Debug.Log($"You have connected to the Photon Master Server");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); //  ???? ???? ????
            //PhotonNetwork.AutomaticallySyncScene = true;  ???????????? ?????? ???? ??????
        }

    }
    public override void OnJoinedLobby() //?????????? ???? ~ ???? ?? ????
    {
        base.OnJoinedLobby();
        Debug.Log($"You have connected to a Photon Lobby");

        //MenuManager.Instance.OpenMenu("TitleMenu");

        BackBtn.interactable = true;
        QuickBtn.interactable = true;
        MakeBtn.interactable = true;
        //??-???????? ?????????? ?????????????? ???????? startGameButton.SetActive(PhotonNetwork.isMasterClient); ?????? ???????? ???? ?????? ???? // ???????? AutomaticallySyncScene = true; ???? ?????? ?????? ?????? ????. 

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//?? ?????? ???????? ?? ??????????
        }

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;


        byte maxplayer = byte.Parse(DW.options[DW.value].text);
        ro.MaxPlayers = maxplayer;

        Debug.Log(maxplayer);
        PhotonNetwork.CreateRoom(roomNameInputField.text, ro); // ???? ???????????????? roomNameInputField.text?? ???????? ???? ??????.
        
        MenuManager.Instance.OpenMenu("Loading");//?????? ????
    }

    public void QuickMatching()
    {
        QuickBtn.interactable = false;
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 8;
        ro.BroadcastPropsChangeToAll = true;
        ro.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "MapNumber", 0 } }; // ???? ???? ????.
        ro.CustomRoomPropertiesForLobby = new string[] { "MapNumber" }; // ?????? ?? ???? ????????, ?????? ?? ???????? ????????.


        PhotonNetwork.JoinRandomOrCreateRoom( expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "MapNumber", 0 } }, expectedMaxPlayers: 8 , matchingType : MatchmakingMode.RandomMatching , typedLobby: null , sqlLobbyFilter : null , roomName : nick + "님의 월드",ro, null);

    }


    public override void OnCreatedRoom() // ?? ?????? ????
    {
        base.OnCreatedRoom();
        Debug.Log($"You have created a Photon Room named {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom() // ?? ?????? ????
    {
        base.OnJoinedRoom();
        Debug.Log($"You have joined the Photon Room {PhotonNetwork.CurrentRoom.Name}");

        Transitioner.Instance.TransitionToScene("MultiPlay");



        //Spawn();
        
        //???? ?????? ????????

        //MenuManager.Instance.OpenMenu("RoomMenu");//?? ???? ????

        //roomNameText.text = PhotonNetwork.CurrentRoom.Name;  ?????? ????

        //Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + 8;  ?????? ????

        //???????? ???? ????**************
        /*
        Player[] players = PhotonNetwork.PlayerList;
        for(int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        */
        //StatingPoint = new Vector3(Random.Range(100, 150), 0, Random.Range(50, 100)); // 
        //PhotonNetwork.Instantiate("BullDogPlayer", StatingPoint, Quaternion.identity, 0);
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate("Prefab_shiva", new Vector3(Random.Range(-6f, 19f), 4f, Random.Range(-6f, 19f)), Quaternion.identity, 0);
        Debug.Log("Spawn");
    }


    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//???? ?????????? JoinRoom???? ?????????? ???? ?????? ????????. 
        MenuManager.Instance.OpenMenu("Loading");//?????? ????
    }
    public void DisConnectPhoton()
    {
        PhotonNetwork.Disconnect();
        MenuManager.Instance.OpenMenu("Loading");//?????? ????
    }

        public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//???????? ???? ???????? ????
        //MenuManager.Instance.OpenMenu("Loading");//?????? ????
    }

 
    public override void OnLeftRoom() // ?? ????????
    {
        base.OnLeftRoom();
        Debug.Log($"You have left a Photon Room");


        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.PlayerList.ID);
        //PhotonNetwork.Disconnect();
        //PhotonNetwork.LoadLevel(2);
    }
    public override void OnJoinRoomFailed(short returnCode, string message) // ?????? ????
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log($"You failed to join a Photon room : {message}");

        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("Error");//???? ???? ????

    }
    public override void OnCreateRoomFailed(short returnCode, string message)//?? ?????? ?????? ????
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("ErrorMenu");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // ?????? ?????? ????
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Another player has joined the room {newPlayer.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + 8;

        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) // ?????????? ????
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player has left the room {otherPlayer.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + 8;
    }
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient) //?????? ?????? ?????? ?????????? 
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log($"new Master Client is {newMasterClient.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + 8;
    }



    
    public override void OnRoomListUpdate(List<RoomInfo> roomList) // ?? ?????????? ????
    {
        if (!RoomFilted)
        {
            GameObject tempRoom = null;
            foreach (var room in roomList)
            {
                //???? ??????????
                if (room.RemovedFromList == true)
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    Destroy(tempRoom);
                    roomDict.Remove(room.Name);
                }
                else // ???????? ????(??????????)
                {
                    //?? ?????? ???? ?????? ????
                    if (roomDict.ContainsKey(room.Name) == false)
                    {
                        GameObject _room = Instantiate(roomListItemPrefab, roomListContent);
                        _room.GetComponent<RoomListItem>().RoomInfo = room;
                        roomDict.Add(room.Name, _room);
                    }
                    else // ?? ?????? ???????? ???? 
                    {
                        roomDict.TryGetValue(room.Name, out tempRoom);
                        tempRoom.GetComponent<RoomListItem>().RoomInfo = room;
                    }
                }
            }
        }
       
        /*
        foreach (Transform trans in roomListContent)//???????? ???? roomListContent
        {
            Destroy(trans.gameObject);//???????? ?????????? ???????? ????????
        }
        for (int i = 0; i < roomList.Count; i++)//?????????? ????
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            //instantiate?? prefab?? roomListContent?????? ?????????? ?? ???????? i???? ?????????? ????. 
        }*/
    }


    public void RoomFilter()
    {
        if (roomFilterName.text == "")
        {
            RoomFilted = false;
        }
        else
        {
            RoomFilted = true;


            List<RoomInfo> FilteredRooms = new List<RoomInfo>() 
            {

            };


            foreach (Transform trans in roomListContent)//???????? ???? roomListContent
            {
                Destroy(trans.gameObject);//???????? ?????????? ???????? ????????
            }
            
            
            for (int i = 0; i < FilteredRooms.Count; i++)//?????????? ????
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(FilteredRooms[i]);
                //instantiate?? prefab?? roomListContent?????? ?????????? ?? ???????? i???? ?????????? ????. 
            }
          
        }
    }

    public void Disconnect()
    {
        BackBtn.interactable = false;
        PhotonNetwork.Disconnect();
        MenuManager.Instance.OpenMenu("Loading");//?????? ????
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        Transitioner.Instance.TransitionToScene("Main");
        //PhotonNetwork.LoadLevel(2); // ???? ?????? ????????
    }
}
