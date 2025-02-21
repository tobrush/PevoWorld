using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayFabAuthenticator : MonoBehaviour
{

    private string _playFabPlayerIdCache;

    public void Awake()
    {
        AuthenticateWithPlayFab();
    }


    // 1. ���� Playfab ����� ���� / �ܼ�ȭ�� ���� LoginWithCustomID API ȣ�� / �̴ܰ迡�� ���ϴ� �ٸ� �α��ι�� ��밡�� / PlayFabSettings.DeviceUniqueIdentifier�� ����� ���� ID�� ��� / �����ܰ谡 �� �ݹ����� RequestPhotonToken ���� / ��������
    private void AuthenticateWithPlayFab()
    {
        LogMessage("PlayFab authenticating using Custom ID...");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = PlayFabSettings.DeviceUniqueIdentifier
        }, RequestPhotonToken, OnPlayFabError);
    }


    // 2. PlayFab���� Photon ���� ��ū�� ��û / Photon�� �ٸ� ���� ��ū�� ����ϱ� ������ �̰��� �߿� / PlayFab������ PlayFab SessionTicket�� ���� ���Ұ� / ����� ��ū ��û�ؾ��� / �� APIȣ���� ������ �����ؾ��� / Photon �� ID ���� / PhotonNetworkŬ������ ���� �����ʵ� ����Ͽ� ������ / �ݹ����� AuthenticateWithPhoton�� ���� / ��ū ������ ȹ��  
    private void RequestPhotonToken(LoginResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = "024b5b66-2950-4c8c-9090-b2d9d4bd6cd2"
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    // 3. ���ο� AuthenticationValue �ν��Ͻ��� ���� / �� Ŭ������ Photon ȯ�� ������ �÷��̾ �����ϴ� ����� ����
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        // AuthType�� ����� �������� �����մϴ�. ��, ��ü PlayFab ���� ������ �����ɴϴ�.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        // "username" �Ű������� �߰��մϴ�. ȥ������ ���ʽÿ�. PlayFab�� �� �Ű� ������ ����� �̸��� �ƴ� �÷��̾� PlayFab ID(!)�� ���Ե� ������ �����մϴ�.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);

        // "��ū" �Ű������� �߰��մϴ�. PlayFab�� ���� �ܰ迡�� Photon ���� ��ū ������ ������ ������ �����մϴ�.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        // ���������� ��ü ���ø����̼ǿ��� �� ���� �Ű������� ����ϵ��� Photon�� �����մϴ�.
        PhotonNetwork.AuthValues = customAuth;
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        LogMessage(obj.GenerateErrorReport());
    }

    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);
    }
}