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
    public AudioClip explosion;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    // Update is called once per frame
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodRadius, interactionLayer);
       
            foreach (Collider c in colliders)
            {
                if (c != null)
                 {
                     c.gameObject.GetComponent<PlayerMovement>().hit(1);
                 }
            }
        photonView.RPC("exposionAudio", RpcTarget.All);
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

    void killing()
    {
        photonView.RPC("killBomb", RpcTarget.All);
    }

    [PunRPC]
    public void exposionAudio()
    {
        AudioSource audioRPC = gameObject.AddComponent<AudioSource>();
        audioRPC.clip = explosion;
        audioRPC.spatialBlend = 1;
        audioRPC.minDistance = 25;
        audioRPC.maxDistance = 100;
        audioRPC.Play();

        if (photonView.IsMine)
        {
            particleInstance = PhotonNetwork.Instantiate(particlePrefab.name, transform.position, Quaternion.identity);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(killing), 2f);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explodRadius);
    }
}
