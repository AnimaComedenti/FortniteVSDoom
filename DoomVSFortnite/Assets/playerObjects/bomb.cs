using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bomb : MonoBehaviourPun
{
    public float delay;
    public float explodRadius;
    public LayerMask interactionLayer;
    public GameObject particlePrefab;
    private GameObject particleInstance;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    // Update is called once per frame
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodRadius, interactionLayer);
        foreach(Collider c in colliders)
        {
            c.gameObject.GetComponent<PlayerMovement>().hit(false);
        }
        particleInstance = PhotonNetwork.Instantiate(particlePrefab.name, transform.position, Quaternion.identity,0);
        GetComponent<AudioSource>().Play();
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        photonView.RPC("killBomb", RpcTarget.All);
    }

    [PunRPC]
    void killBomb()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(particleInstance);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
