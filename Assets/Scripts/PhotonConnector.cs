using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 포톤기능 사용
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonConnector : MonoBehaviourPunCallbacks // 다른 포톤 반응 받아들이기
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
        PhotonNetwork.ConnectUsingSettings(); // 설정한 포톤서버에 따라 마스터 서버에 연결
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
    public override void OnConnectedToMaster() // 마스터 서버에 연결시 호출
    {
        base.OnConnectedToMaster(); 
        Debug.Log($"You have connected to the Photon Master Server");
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); //  바로 로비 참가
        }

    }
    public override void OnJoinedLobby() //로비참가시 호출 ~ 자동 방 생성
    {
        base.OnJoinedLobby();
        Debug.Log($"You have connected to a Photon Lobby");
        CreatePhotonRoom("TestRoom");  // TODO : 버튼으로 만드는게낫지 이름넣고
    }
    public override void OnCreatedRoom() // 방 생성시 호출
    {
        base.OnCreatedRoom();
        Debug.Log($"You have created a Photon Room named {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinedRoom() // 방 참가시 호출
    {
        base.OnJoinedRoom();
        Debug.Log($"You have joined the Photon Room {PhotonNetwork.CurrentRoom.Name}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
        
        StatingPoint = new Vector3(Random.Range(100, 150), 0, Random.Range(50, 100)); // 
        PhotonNetwork.Instantiate("BullDogPlayer", StatingPoint, Quaternion.identity, 0);
    }
    /*
    public override void OnLeftRoom() // 방 나감
    {
        base.OnLeftRoom();
        Debug.Log($"You have left a Photon Room");
        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.PlayerList.ID);
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(2);
    }
    */
    public override void OnJoinRoomFailed(short returnCode, string message) // 방참가 실패
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log($"You failed to join a Photon room : {message}");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // 새로운 유저의 등장
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Another player has joined the room {newPlayer.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
    }
    /*
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) // 다른유저의 이탈
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player has left the room {otherPlayer.UserId}");
        Countui.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / 10";
    }*/
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient) //방장이 나가서 방장이 바뀌었을때 
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


