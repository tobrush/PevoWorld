using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Photon.Realtime.Player player;       // as photon realtime

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;


    }

    public override void OnPlayerLeftRoom(Player otherPlayer) //�÷��̾ �涰������ ȣ��
    {
        if (player == otherPlayer)//���� �÷��̾ ����?
        {
            Debug.Log("�ٸ��̳���");
            Destroy(gameObject);//�ٸ��� �̸�ǥ ����
        }
    }

    public override void OnLeftRoom()//�� ������ ȣ��
    {
        Debug.Log("�� ����");
        Destroy(gameObject);//�� �̸�ǥ ����
    }
}