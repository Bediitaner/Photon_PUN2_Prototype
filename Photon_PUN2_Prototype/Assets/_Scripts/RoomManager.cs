using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spawnPosition;
    
    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        Debug.Log("Connected to Server.");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        PhotonNetwork.JoinOrCreateRoom("test", null, null);
        
        Debug.Log("Joined Lobby.");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        Debug.Log("Joined Room.");
        
        GameObject player = PhotonNetwork.Instantiate(_player.name, _spawnPosition.position, Quaternion.identity);
    }
}
