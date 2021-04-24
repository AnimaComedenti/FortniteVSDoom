using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class myNetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public GameObject first,second,third,forth,five,six;
    void Start()
    {
        float randNmr = Random.Range(0,7);
        Vector3 spawnPosition;

        switch (randNmr)
        {
            case 1: spawnPosition = first.transform.position + new Vector3(0, 2, 0);
                break;
            case 2:
                spawnPosition = second.transform.position + new Vector3(0, 2, 0);
                break;
            case 3:
                spawnPosition = third.transform.position + new Vector3(0, 2, 0);
                break;
            case 4:
                spawnPosition = forth.transform.position + new Vector3(0, 2, 0);
                break;
            case 5:
                spawnPosition = five.transform.position + new Vector3(0, 2, 0);
                break;
            case 6:
                spawnPosition = six.transform.position + new Vector3(0, 2, 0);
                break;
            default: spawnPosition = six.transform.position + new Vector3(0, 2, 0);
                break;
        }

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
