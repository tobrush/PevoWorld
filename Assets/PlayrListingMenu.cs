using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayrListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private playerListing _playerListing;
    [SerializeField]
    private List<playerListing> _listings = new List<playerListing>();

 


    private void Awake()
    {
        GetCurrentRoomPlayers();
    }
    public void GetCurrentRoomPlayers()
    {
        foreach(KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player Player)
    {

        var listing = Instantiate(_playerListing, _content );
        listing.transform.SetParent(_content);
        if(listing != null)
        {
            listing.GetComponent<playerListing>().SetPlayerInfo(Player);
            _listings.Add(listing.GetComponent<playerListing>());
        }


    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);
        AddPlayerListing(newPlayer);


    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x._player  == otherPlayer);
        if(index != -1)
        {
            Destroy(_listings[index].gameObject);
        }
        _listings.RemoveAt(index);
    }

}
