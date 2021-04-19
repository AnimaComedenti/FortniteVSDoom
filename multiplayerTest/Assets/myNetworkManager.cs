using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class myNetworkManager : MonoBehaviour
{
    public GameObject plyerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(plyerPrefab.name, new Vector3(0,5,0),Quaternion.identity,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
