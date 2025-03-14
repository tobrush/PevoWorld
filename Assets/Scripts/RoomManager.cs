using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{

    public UserData userData;
    public ConfigManager configManager;
    public FriendsManager friendsManager;
    public MailBoxManager mailBoxManager;

    public GameObject CC;
    public GameObject PC;

    public PlayerManager MyPM;
    public TMP_InputField ChatInput;

    private Censor censor;



    public Text ListText;
    public TMP_Text RoomInfoText;
    public TMP_Text RoomInfoText2;
    public TMP_Text RoomInfoText3;
    public string RoomName;
    public Text[] ChatText;

    public bool EmojiWindowSwitch;
    public GameObject EmojiWindow;

    public PhotonView pv;


    public void LeaveRoom()
    {
        CC = GameObject.Find("MoveLookArea");
        PC = GameObject.Find("JoystickArea");

        PC.GetComponent<PlayerController>().enabled = false;
        CC.GetComponent<CameraController>().enabled = false;
        
        PhotonNetwork.LeaveRoom();//???????? ???? ???????? ????
        //MenuManager.Instance.OpenMenu("Loading");//?????? ????
    }

    public void ChatFilter() // string ?????????? ???????? ???????? ????????????
    {
        ChatInput.text = censor.CensorText(ChatInput.text);

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 입장하셨습니다.</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 나가셨습니다.</color>");
    }

    void RoomRenewal()
    {
        /*
         for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : "\n");
        }
           */
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name;
        RoomInfoText2.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        RoomInfoText3.text = "참여중 " + "<color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>" + "명";
    }

    [PunRPC] // RPC?? ?????????? ???????? ?? ???? ???????? ????????
    void ChatRPC(string msg)
    {
        //bool isInput = false;
        //for (int i = 0; i < ChatText.Length; i++)
        //    if (ChatText[i].text == "")
        //    {
        //        isInput = true;
        //        ChatText[i].text = msg;
        //        break;
        //    }
        //if (!isInput) // ?????? ?????? ???? ????
        //{
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        //}
    }

    public void OnEmoji()
    {
        if(!EmojiWindowSwitch)
        {
            EmojiWindow.SetActive(true);
            EmojiWindowSwitch = true;
        }
        else
        {
            EmojiWindow.SetActive(false);
            EmojiWindowSwitch = false;
        }
    }


    public override void OnLeftRoom() // ?? ????????
    {
        base.OnLeftRoom();
        Debug.Log($"You have left a Photon Room");
        PhotonNetwork.Disconnect();
        //PhotonNetwork.LoadLevel(3);
        Transitioner.Instance.TransitionToScene("WorldLobby");

        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.PlayerList.ID);
        //PhotonNetwork.Disconnect();

    }

    
    public void Start()
    {

        userData = GameObject.Find("UserData").GetComponent<UserData>();
        //configManager.myID.text = userData.User_MyID;
        //configManager.MyName.text = userData.User_Name;

        //friendsManager.MyDisplayName = userData.User_Name;
        //friendsManager.MyID = userData.User_MyID;

        //mailBoxManager.UpdateInboxListToClient(userData.User_Inbox);

        censor = GetComponent<Censor>();
        RoomName = PhotonNetwork.CurrentRoom.Name;
       
        switch (UserData.instance.Dog1_Character)
        {
            case "2":
                PhotonNetwork.Instantiate("02_siba_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;

            case "3":
                PhotonNetwork.Instantiate("03_Dachs_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "4":
                PhotonNetwork.Instantiate("04_Pome_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "5":
                PhotonNetwork.Instantiate("05_Bulldog_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "6":
                PhotonNetwork.Instantiate("06_Bedlington_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "7":
                PhotonNetwork.Instantiate("07_Welsh_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "8":
                PhotonNetwork.Instantiate("08_Chihuahua_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "9":
                PhotonNetwork.Instantiate("09_Bichon_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "10":
                PhotonNetwork.Instantiate("10_Beagle_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "11":
                PhotonNetwork.Instantiate("11_Golden_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "12":
                PhotonNetwork.Instantiate("12_Shih_Tzu_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "13":
                PhotonNetwork.Instantiate("13_French_Bulldog_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            case "14":
                PhotonNetwork.Instantiate("14_Jindo_idle01_Multi", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
                break;
            default:
                Debug.Log("");
                break;
        }    
        
        
       // PhotonNetwork.Instantiate("Prefab_shiva", new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f)), Quaternion.identity, 0);
        Debug.Log("Spawn");

        RoomRenewal();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";


    }
    public void InputMessage()
    {
        if(MyPM != null && ChatInput.text != "")
        {
            pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
            MyPM.sendMessage(ChatInput.text);
            ChatInput.text = "";
            
        }
    }

    public void ClickText()
    {

    }



    public void SendEmoji(int i)
    {
        EmojiWindowSwitch = false;

        if (MyPM != null)
        {
            MyPM.sendEmoji(i);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
