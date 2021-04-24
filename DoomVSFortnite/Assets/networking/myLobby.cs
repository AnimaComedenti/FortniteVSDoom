using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class myLobby : MonoBehaviourPunCallbacks
{
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Lol-Room",new Photon.Realtime.RoomOptions { MaxPlayers = 2});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        PhotonNetwork.LoadLevel("Level1");
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
