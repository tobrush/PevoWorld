using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PevoCustomProperty : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField]
    private string PlayfabID;
    [SerializeField]
    private string CharacterNumber;

    private void SetCoustomUserInfo()
    {
        CharacterNumber = UserData.instance.Dog1_Character; // or 2 3
        PlayfabID = UserData.instance.User_MyID;
        _myCustomProperties["PlayfabID"] = PlayfabID;
        _myCustomProperties["CharacterNumber"] = CharacterNumber;

        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }

    public void OnClick_Btn()
    {
        SetCoustomUserInfo();
    }
}
