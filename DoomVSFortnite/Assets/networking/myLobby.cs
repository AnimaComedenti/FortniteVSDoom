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
            //Verbindung zum Server
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //Callback nach connections zum Server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //Fals kein room existiert
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        PhotonNetwork.LoadLevel("Level1");
    }
}
