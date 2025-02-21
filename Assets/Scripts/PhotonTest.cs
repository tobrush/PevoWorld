using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    public string nickName;

    void Start()
    {
        PhotonNetwork.AuthValues = new AuthenticationValues(nickName); //인증값
        PhotonNetwork.GameVersion = "1.0"; // 게임버전
        PhotonNetwork.AutomaticallySyncScene = true; // 동기화
        PhotonNetwork.NickName = nickName; //닉네임

        PhotonNetwork.ConnectUsingSettings(); //마스터 서버 접속
    }

    public override void OnConnectedToMaster() // 마스터 서버에 연결시 호출
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();   //  바로 만들거나 로비 참가
            //loading
        }
    }
    public override void OnJoinedLobby() //로비참가시 호출 ~ 자동 방 생성
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
        //loading
    }
    public override void OnJoinedRoom() // 방 참가시 호출
    {
        Player[] players = PhotonNetwork.PlayerList;
        //플레이어 생성
        Debug.Log("OnRoom");
    }
}
