using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class playerListing : MonoBehaviourPunCallbacks
{
    public OtherUserManager OUM;
    [SerializeField]
    private TMP_Text _text;
    public Player _player { get; private set; }

    public string _dogName;
    public string _dogStyleNumber;
    public string _playfabID;

    public Image ChaIcon;

    public PhotonView pv;

   
    public void SetPlayerInfo(Player player)
    {
        pv = GetComponent<PhotonView>();
        OUM = GameObject.Find("OtherUserManager").GetComponent<OtherUserManager>();

        _player = player;
        _text.text = player.NickName;
        this.gameObject.name = player.NickName;

        _playfabID = (string)player.CustomProperties["PlayfabID"];
        _dogStyleNumber = (string)player.CustomProperties["CharacterNumber"];

        if(_playfabID == UserData.instance.User_MyID)
        {
            _text.text = player.NickName + "<color=grey>" + " <나>" + "</color>";
        }
        

        switch (_dogStyleNumber)
        {
            case "2": ChaIcon.sprite = OUM.chaimages[0]; break;
            case "3": ChaIcon.sprite = OUM.chaimages[1]; break;
            case "4": ChaIcon.sprite = OUM.chaimages[2]; break;
            case "5": ChaIcon.sprite = OUM.chaimages[3]; break;
            case "6": ChaIcon.sprite = OUM.chaimages[4]; break;
            case "7": ChaIcon.sprite = OUM.chaimages[5]; break;
            case "8": ChaIcon.sprite = OUM.chaimages[6]; break;
            case "9": ChaIcon.sprite = OUM.chaimages[7]; break;
            case "10": ChaIcon.sprite = OUM.chaimages[8]; break;
            case "11": ChaIcon.sprite = OUM.chaimages[9]; break;
            case "12": ChaIcon.sprite = OUM.chaimages[10]; break;
            case "13": ChaIcon.sprite = OUM.chaimages[11]; break;
            case "14": ChaIcon.sprite = OUM.chaimages[12]; break;
            default: ChaIcon.sprite = OUM.chaimages[0]; break;

        }


    }

    public void GetPlayerInfo()
    {
        OUM.UserInfomationOpen(_playfabID);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if(targetPlayer !=null && targetPlayer == _player)
        {
            if (changedProps.ContainsKey("RandomNumber"))
            {
                SetPlayerText(targetPlayer);
            }

        }
    }

    private void SetPlayerText(Player player)
    {
        int result = -1;
        if(player.CustomProperties.ContainsKey("RandomNumber"))
        {
            result = (int)player.CustomProperties["RandomNumber"];

        }
       // _dogStyleNumber = result;
    }


    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
              stream.SendNext(_dogName);
              stream.SendNext(_dogStyleNumber);
             stream.SendNext(_playfabID);
        }
        else
        {

            _dogName = (string)stream.ReceiveNext();
            _dogStyleNumber = (int)stream.ReceiveNext();
            _playfabID = (string)stream.ReceiveNext();

            /*
            switch (_dogStyleNumber)
            {
                case 2: ChaIcon.sprite = OUM.chaimages[0]; break;
                case 3: ChaIcon.sprite = OUM.chaimages[1]; break;
                case 4: ChaIcon.sprite = OUM.chaimages[2]; break;
                case 5: ChaIcon.sprite = OUM.chaimages[3]; break;
                case 6: ChaIcon.sprite = OUM.chaimages[4]; break;
                case 7: ChaIcon.sprite = OUM.chaimages[5]; break;
                case 8: ChaIcon.sprite = OUM.chaimages[6]; break;
                case 9: ChaIcon.sprite = OUM.chaimages[7]; break;
                case 10: ChaIcon.sprite = OUM.chaimages[8]; break;
                case 11: ChaIcon.sprite = OUM.chaimages[9]; break;
                case 12: ChaIcon.sprite = OUM.chaimages[10]; break;
                case 13: ChaIcon.sprite = OUM.chaimages[11]; break;
                case 14: ChaIcon.sprite = OUM.chaimages[12]; break;
                default: ChaIcon.sprite = OUM.chaimages[0]; break;

            }
            
        }
    */
}



