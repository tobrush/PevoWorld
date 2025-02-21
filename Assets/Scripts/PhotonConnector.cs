using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // ������ ���
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonConnector : MonoBehaviourPunCallbacks // �ٸ� ���� ���� �޾Ƶ��̱�
{

    public GameObject BullDogPlayer;
    
    public Vector3 StatingPoint;

    public Text Countui;
    public UserData userData;

    #region Unity Methods
    private void Start()
    {
        // string randomeName = $"Tester{Guid.NewGuid().ToString()}";
        // ConnectToPhoton(randomeName);

        userData = GameObject.Find("UserData").GetComponent<UserData>();
        string nickname = userData.User_Name;
        ConnectToPhoton(nickname);
    }
    #endregion

    #region Private Methods
    private void ConnectToPhoton(string nickName)
    {
        Debug.Log($"Connect to Photon as {nickName}");
        PhotonNetwork.AuthValues = new AuthenticationValues(nickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.ConnectUsingSettings(); // ������ ���漭���� ���� ������ ������ ����
    }
    private void CreatePhotonRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
    #endregion

    #region Public Methods

    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster() // ������ ������ ����� ȣ��
    {
        base.OnConnectedToMaster(); 
        Debug.Log($"You have connected to the Photon Master Server");
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); //  �ٷ� �κ� ����
        }

    }
    public override void OnJoinedLobby() //�κ������� ȣ�� ~ �ڵ� �� ����
    {
        base.OnJoinedLobby();
        Debug.Log($"You have connected to a Photon Lobby");
        CreatePhotonRoom("TestRoom");  // TODO : ��ư���� ����°Գ��� �̸��ְ�
    }
    public override void OnCreatedRoom() // �� ������ ȣ��
    {
        base.OnCreatedRoom();
        Debug.Log($"You have created a Photon Room named {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinedRoom() // �� ������ ȣ��
    {
        base.OnJoinedRoom();
        Debug.Log($"You have joined the Photon Room {PhotonNetwork.CurrentRoom.Name}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
        
        StatingPoint = new Vector3(Random.Range(100, 150), 0, Random.Range(50, 100)); // 
        PhotonNetwork.Instantiate("BullDogPlayer", StatingPoint, Quaternion.identity, 0);
    }
    /*
    public override void OnLeftRoom() // �� ����
    {
        base.OnLeftRoom();
        Debug.Log($"You have left a Photon Room");
        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.PlayerList.ID);
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(2);
    }
    */
    public override void OnJoinRoomFailed(short returnCode, string message) // ������ ����
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log($"You failed to join a Photon room : {message}");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // ���ο� ������ ����
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Another player has joined the room {newPlayer.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
    }
    /*
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) // �ٸ������� ��Ż
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player has left the room {otherPlayer.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
    }*/
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient) //������ ������ ������ �ٲ������ 
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log($"new Master Client is {newMasterClient.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
    }

    #endregion

    public void BackToMain()
    {
        //PhotonNetwork.Destroy(BullDogPlayer);
        PhotonNetwork.LeaveRoom();
        
    }
}


