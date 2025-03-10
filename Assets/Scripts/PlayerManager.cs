using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;//path????????
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public Text UserNameText;
    public Text DogNameText;

    public string PlayfabID;
    public string dogName;
    public int CharacterNumber;

    public Transform tr;
    public PhotonView PV;

    public GameObject CC;
    public GameObject PC;

    public Canvas canvas;

    public GameObject ChatBox;
    public Text ChatText;
    public Image ShowEmoji;

    public RoomManager RM;
    public EmojiManager EM;

    public void Awake()
        
    {

        PV = this.GetComponent<PhotonView>();
        tr = this.GetComponent<Transform>();

        CC = GameObject.Find("MoveLookArea");
        PC = GameObject.Find("JoystickArea");
        
        
        if (PV.IsMine)
        {
            UserNameText.text = PhotonNetwork.NickName + "의";
            DogNameText.color = Color.green;

            dogName = UserData.instance.Dog1_Name;
            PlayfabID = UserData.instance.User_MyID;
            DogNameText.text = dogName;
            CharacterNumber = int.Parse(UserData.instance.Dog1_Character);
        }
        else
        {
            UserNameText.text = PV.Owner.NickName + "의";
            DogNameText.color = Color.white;
        }
    }

    public void Start()
    {
       EM= GameObject.Find("EmojiManager").GetComponent<EmojiManager>();
      
        if (PV.IsMine)
        {
            //this.transform.GetChild(int.Parse(UserData.instance.Dog1_Character) + 1).gameObject.SetActive(true);
            RM = GameObject.Find("RoomManager").GetComponent<RoomManager>();
            RM.MyPM = this;
            CreateController();//???????? ???????? ????????. 
        }
        else
        {
            DogNameText.text = dogName;
            
            //this.transform.GetChild(CharacterNumber + 1).gameObject.SetActive(true);
        }
        

        void CreateController()//???????? ???????? ??????
        {

            CC.GetComponent<CameraController>().player = tr;
            //CC.GetComponent<CameraController>().playerAgent = tr.gameObject.GetComponent<Navmesh>();
            PC.GetComponent<PlayerController>().player = tr;
   

            CC.GetComponent<CameraController>().enabled = true;
            PC.GetComponent<PlayerController>().enabled = true;


            //???? ???? ????
            Debug.Log("Instantiated Player Controller");
        }

        // PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
        // ???? ???????? ???? ???????? ?????????? ?? ?????? ?? ?????? ??????????
    }

    public void Update()
    {
        canvas.transform.LookAt(Camera.main.gameObject.transform);

        
       
    }

    public void sendMessage(string message)
    {
        PV.RPC("ChatRPC", RpcTarget.All, message);
    }



    public void sendEmoji(int i)
    {
        int a = i;
        PV.RPC("Emoji", RpcTarget.All, a);
    }

    [PunRPC]
    void Emoji(int i)
    {
        StopCoroutine("DelayHide");
        ShowEmoji.GetComponent<Image>().sprite = EM.emojiList[i];
        ShowEmoji.gameObject.SetActive(true);
        ChatText.gameObject.SetActive(false);
        ChatBox.SetActive(true);
        StartCoroutine("DelayHide");
    }


    [PunRPC]
    void ChatRPC(string message)
    {
        StopCoroutine("DelayHide");
        ChatText.text = message;

        ShowEmoji.gameObject.SetActive(false);
        ChatText.gameObject.SetActive(true);
        ChatBox.SetActive(true);
        StartCoroutine("DelayHide");
    }

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(3.0f);
        ChatBox.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(dogName);
            stream.SendNext(CharacterNumber);
            stream.SendNext(PlayfabID);
        }
        else
        {
            // Network player, receive data
            dogName = (string)stream.ReceiveNext();
            CharacterNumber = (int)stream.ReceiveNext();
            PlayfabID = (string)stream.ReceiveNext();
        }
    }

    public void OnSelect(string command)
    {
        if(command == "mail")
        {
            Debug.Log("mail");
        }
        if (command == "info")
        {
            Debug.Log("info");
        }
        if (command == "friend")
        {
            Debug.Log("friend");
        }
        if (command == "block")
        {
            Debug.Log("block");
        }
        if (command == "skin")
        {
            Debug.Log("skin");
        }
    }
}
