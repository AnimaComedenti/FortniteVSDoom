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
        spawnplayer();
    }

    Vector3 getPosition(Vector3 pos)
    {
        return new Vector3(pos.x / 2, 2, pos.z / 2);
    }

    public void spawnplayer()
    {
            float randNmr = Random.Range(0, 6);
            Vector3 spawnPosition;

            switch (randNmr)
            {
                case 1:
                    spawnPosition = first.transform.position + getPosition(first.transform.localScale);
                    break;
                case 2:
                    spawnPosition = second.transform.position + getPosition(second.transform.localScale);
                    break;
                case 3:
                    spawnPosition = third.transform.position + getPosition(third.transform.localScale);
                    break;
                case 4:
                    spawnPosition = forth.transform.position + getPosition(forth.transform.localScale);
                    break;
                case 5:
                    spawnPosition = five.transform.position + getPosition(five.transform.localScale);
                    break;
                case 6:
                    spawnPosition = six.transform.position + getPosition(six.transform.localScale);
                    break;
                default:
                    spawnPosition = six.transform.position + getPosition(six.transform.localScale);
                    break;
            }
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0);
        }
    }
