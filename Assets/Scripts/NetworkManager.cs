using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject CafePanel, ShopPanel, UserHousePanel, LobbyPanel, DisconnectPanel;

    [Header("DisConnect")]
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();
    public InputField Login_EmailInput, Login_PasswordInput, SignUp_EmailInput, SignUp_PasswordInput, SignUp_UsernameInput, SignUp_PevoIDInput;

    public Toggle NABox;

    [Header("Lobby")]
    public InputField UserNickNameInput;
    public Text LobbyInfoText, UserNickNameText;

    [Header("Room")]
    public InputField SetDataInput;
    public GameObject SetDataBtnObj;
    public Text UserHouseDataText, RoomNameInfoText, RoomNumInfoText;

    bool isLoaded;


    #region �÷�����


    private void Awake()
    {
        Screen.SetResolution(960, 540, false); // PC����
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        DontDestroyOnLoad(this.gameObject);
    }
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = Login_EmailInput.text, Password = Login_PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { GetLeaderboard(result.PlayFabId); PhotonNetwork.ConnectUsingSettings(); }, (error) => print("�α��� ����"));
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Email = SignUp_EmailInput.text, Password = SignUp_PasswordInput.text, Username = SignUp_UsernameInput.text, DisplayName = SignUp_UsernameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("ȸ������ ����"); SetStat(); SetData("default"); }, (error) => print("ȸ������ ����"));
    }

    void SetStat()
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error)=>print("�� ���� ����"));
    }

    void GetLeaderboard(string myID)
    {
        PlayFabUserList.Clear();

        for(int i = 0; i < 10; i++)
        {
            var request = new GetLeaderboardRequest
            {
                StartPosition = i * 100,
                StatisticName = "IDInfo",
                MaxResultsCount = 100,
                ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true }
            };
            PlayFabClientAPI.GetLeaderboard(request, (result) =>
            {
                if (result.Leaderboard.Count == 0) return;
                for (int j = 0; j < result.Leaderboard.Count; j++)
                {
                    PlayFabUserList.Add(result.Leaderboard[j]);
                    if (result.Leaderboard[j].PlayFabId == myID) MyPlayFabInfo = result.Leaderboard[j];
                }
            },
            (error) => { });
        }
    }

    void SetData(string curData)
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "Home", curData} },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => { }, (error) => print("������ ���� ����"));
    }

    void GetData(string curID)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curID }, (result) =>
        UserHouseDataText.text = curID + "\n" + result.Data["home"].Value,
        (eroor) => print("������ �ҷ����� ����"));
    }

    #endregion

    #region �κ�
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    void Update() => LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";

    public override void OnJoinedLobby()
    {
        // �濡�� �κ�� �� �� �����̾���, �α����ؼ� �κ�� �� �� PlayFabUserList�� ä���� �ð����� 1�� ������
        if (isLoaded)
        {
            ShowPanel(LobbyPanel);
            ShowUserNickName();
        }
        else Invoke("OnJoinedLobbyDelay", 1);
    }

    void OnJoinedLobbyDelay()
    {
        isLoaded = true;
        PhotonNetwork.LocalPlayer.NickName = MyPlayFabInfo.DisplayName;

        ShowPanel(LobbyPanel);
        ShowUserNickName();
    }

    void ShowPanel(GameObject CurPanel)
    {
        LobbyPanel.SetActive(false);
        CafePanel.SetActive(false);
        ShopPanel.SetActive(false);
        UserHousePanel.SetActive(false);
        DisconnectPanel.SetActive(false);

        CurPanel.SetActive(true);
    }

    void ShowUserNickName()
    {
        UserNickNameText.text = "";
        for (int i = 0; i < PlayFabUserList.Count; i++) UserNickNameText.text += PlayFabUserList[i].DisplayName + "\n";
    }

    public void XBtn()
    {
        if (PhotonNetwork.InLobby) PhotonNetwork.Disconnect();
        else if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isLoaded = false;
        ShowPanel(DisconnectPanel);
    }
    #endregion

    #region ��
    public void JoinOrCreateRoom(string roomName)
    {
        if (roomName == "������")
        {
            //PlayFabUserList�� ǥ���̸��� �Է¹��� �г����� ���ٸ� PlayFabID�� Ŀ���� ������Ƽ�� �ְ� ���� �����
            for (int i = 0; i < PlayFabUserList.Count; i++)
            {
                if (PlayFabUserList[i].DisplayName == UserNickNameInput.text)
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 25;
                    roomOptions.CustomRoomProperties = new Hashtable() { { "PlayFabID", PlayFabUserList[i].PlayFabId } };
                    PhotonNetwork.JoinOrCreateRoom(UserNickNameInput.text + "���� ��", roomOptions, null);
                    return;
                }
            }
            print("�г����� ��ġ���� �ʽ��ϴ�");
        }
        else PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 25 }, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");



    public override void OnJoinedRoom()
    {
        RoomRenewal();

        string curName = PhotonNetwork.CurrentRoom.Name;
        RoomNameInfoText.text = curName;
        if (curName == "ī��1" || curName == "ī��2") ShowPanel(CafePanel);
        else if (curName == "����1" || curName == "����2") ShowPanel(ShopPanel);
        //�������̸� ������ ��������
        else
        {
            ShowPanel(UserHousePanel);

            string curID = PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString();
            GetData(curID);

            // ���� �� PlatyFabID Ŀ���� ������Ƽ�� ���� PlayFabID�� ���ٸ� ���� ������ �� ����
            if (curID == MyPlayFabInfo.PlayFabId)
            {
                RoomNameInfoText.text += " (���� ��)";

                SetDataInput.gameObject.SetActive(true);
                SetDataBtnObj.SetActive(true);
            }
        }
    }


   
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) => RoomRenewal();

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) => RoomRenewal();

   

    void RoomRenewal()
    {
        UserNickNameText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            UserNickNameText.text += PhotonNetwork.PlayerList[i].NickName + "\n";
        RoomNumInfoText.text = PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";
    }

    public override void OnLeftRoom()
    {
        SetDataInput.gameObject.SetActive(false);
        SetDataBtnObj.SetActive(false);

        SetDataInput.text = "";
        UserNickNameInput.text = "";
        UserHouseDataText.text = "";
    }

    public void SetDataBtn()
    {
        // �ڱ��ڽ��� �濡���� �� ������ �����ϰ�, �� ���� �� 1�� �ڿ� �� �ҷ�����
        SetData(SetDataInput.text);
        Invoke("SetDataBtnDelay", 1);
    }

    void SetDataBtnDelay() => GetData(PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString());
    #endregion
}
