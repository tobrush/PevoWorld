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


    #region 플레이팹


    private void Awake()
    {
        Screen.SetResolution(960, 540, false); // PC기준
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        DontDestroyOnLoad(this.gameObject);
    }
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = Login_EmailInput.text, Password = Login_PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { GetLeaderboard(result.PlayFabId); PhotonNetwork.ConnectUsingSettings(); }, (error) => print("로그인 실패"));
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Email = SignUp_EmailInput.text, Password = SignUp_PasswordInput.text, Username = SignUp_UsernameInput.text, DisplayName = SignUp_UsernameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("회원가입 성공"); SetStat(); SetData("default"); }, (error) => print("회원가입 실패"));
    }

    void SetStat()
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error)=>print("값 저장 실패"));
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
        PlayFabClientAPI.UpdateUserData(request, (result) => { }, (error) => print("데이터 저장 실패"));
    }

    void GetData(string curID)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curID }, (result) =>
        UserHouseDataText.text = curID + "\n" + result.Data["home"].Value,
        (eroor) => print("데이터 불러오기 실패"));
    }

    #endregion

    #region 로비
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    void Update() => LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";

    public override void OnJoinedLobby()
    {
        // 방에서 로비로 올 땐 딜레이없고, 로그인해서 로비로 올 땐 PlayFabUserList가 채워질 시간동안 1초 딜레이
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

    #region 방
    public void JoinOrCreateRoom(string roomName)
    {
        if (roomName == "유저방")
        {
            //PlayFabUserList의 표시이름과 입력받은 닉네임이 같다면 PlayFabID를 커스텀 프로퍼티로 넣고 방을 만든다
            for (int i = 0; i < PlayFabUserList.Count; i++)
            {
                if (PlayFabUserList[i].DisplayName == UserNickNameInput.text)
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 25;
                    roomOptions.CustomRoomProperties = new Hashtable() { { "PlayFabID", PlayFabUserList[i].PlayFabId } };
                    PhotonNetwork.JoinOrCreateRoom(UserNickNameInput.text + "님의 방", roomOptions, null);
                    return;
                }
            }
            print("닉네임이 일치하지 않습니다");
        }
        else PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 25 }, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");



    public override void OnJoinedRoom()
    {
        RoomRenewal();

        string curName = PhotonNetwork.CurrentRoom.Name;
        RoomNameInfoText.text = curName;
        if (curName == "카페1" || curName == "카페2") ShowPanel(CafePanel);
        else if (curName == "상점1" || curName == "상점2") ShowPanel(ShopPanel);
        //유저방이면 데이터 가져오기
        else
        {
            ShowPanel(UserHousePanel);

            string curID = PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString();
            GetData(curID);

            // 현재 방 PlatyFabID 커스텀 프로퍼티가 나의 PlayFabID와 같다면 값을 저장할 수 있음
            if (curID == MyPlayFabInfo.PlayFabId)
            {
                RoomNameInfoText.text += " (나의 방)";

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
        RoomNumInfoText.text = PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
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
        // 자기자신의 방에서만 값 저장이 가능하고, 값 저장 후 1초 뒤에 값 불러오기
        SetData(SetDataInput.text);
        Invoke("SetDataBtnDelay", 1);
    }

    void SetDataBtnDelay() => GetData(PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString());
    #endregion
}
