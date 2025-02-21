using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class chatID : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MasterID);
        }
        else
        {
            MasterID = (string)stream.ReceiveNext();
        }
    }

    public string MasterID;

    public PhotonView PV;

    private void Awake()
    {
        PV = this.GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            MasterID = UserData.instance.User_MyID;
        }
        else
        {

        }
    }


    public void UserInforCheck()
    {
        print(MasterID);
    }

}
