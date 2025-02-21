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
        PhotonNetwork.AuthValues = new AuthenticationValues(nickName); //������
        PhotonNetwork.GameVersion = "1.0"; // ���ӹ���
        PhotonNetwork.AutomaticallySyncScene = true; // ����ȭ
        PhotonNetwork.NickName = nickName; //�г���

        PhotonNetwork.ConnectUsingSettings(); //������ ���� ����
    }

    public override void OnConnectedToMaster() // ������ ������ ����� ȣ��
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();   //  �ٷ� ����ų� �κ� ����
            //loading
        }
    }
    public override void OnJoinedLobby() //�κ������� ȣ�� ~ �ڵ� �� ����
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
        //loading
    }
    public override void OnJoinedRoom() // �� ������ ȣ��
    {
        Player[] players = PhotonNetwork.PlayerList;
        //�÷��̾� ����
        Debug.Log("OnRoom");
    }
}
