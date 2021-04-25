using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletScript : MonoBehaviourPun
{
    public float travelTime = 5f;
    private void Update()
    {
        travelTime -= Time.deltaTime;
        if (travelTime <= 0)
        {
            photonView.RPC("killBullet", RpcTarget.All);
        }
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            return;
        }
        if (collision.transform.CompareTag("playerfeet"))
        {
            Debug.Log("Hit");
            collision.gameObject.GetComponent<PlayerMovement>().hit(0);
            photonView.RPC("killBullet", RpcTarget.All);
        }
       
    }

    [PunRPC]
    void killBullet()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}


