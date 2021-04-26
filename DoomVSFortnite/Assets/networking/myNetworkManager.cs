using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class myNetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    private GameObject[] spawns;
    void Start()
    {
        spawnplayer();
    }

    Vector3 getPosition(Vector3 pos)
    {
        return new Vector3(pos.x / 2, 2, pos.z / 2);
    }

    public void spawnplayer()
    {
        int randNmr = Random.Range(0, 6);
        Debug.Log(randNmr);
        Vector3 spawnPosition;
        spawns = GameObject.FindGameObjectsWithTag("spawn");
        spawnPosition = spawns[randNmr].transform.position + getPosition(spawns[randNmr].transform.localScale);
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0);
        }
    }
