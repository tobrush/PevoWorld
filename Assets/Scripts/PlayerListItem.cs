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

    public override void OnPlayerLeftRoom(Player otherPlayer) //플레이어가 방떠났을때 호출
    {
        if (player == otherPlayer)//나간 플레이어가 나면?
        {
            Debug.Log("다른이나감");
            Destroy(gameObject);//다른이 이름표 삭제
        }
    }

    public override void OnLeftRoom()//방 나가면 호출
    {
        Debug.Log("나 나감");
        Destroy(gameObject);//내 이름표 삭제
    }
}