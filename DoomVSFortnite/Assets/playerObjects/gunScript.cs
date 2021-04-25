using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gunScript : MonoBehaviourPun
{
    Vector3 objectPosition, hookVelocity;
    public float maxHookDistance;
    public float dragSpeeed;
    bool isCurrentlyHooking = false;
    public LineRenderer lineRenderer;
    public Camera cam;
    public Transform gunPosition;
    public PlayerMovement controller;

    public LayerMask player;
    public float shootForce;
    public GameObject bulletPrefab;
    public float shootdelay = 2f;
    float saveDelay;
    bool isshooting;
    public AudioClip shoot;

    private void LateUpdate()
    {
        drawRope();
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
           saveDelay = shootdelay;
        }
        else
        {
            GetComponent<gunScript>().enabled = false;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            graplingHook();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!isshooting)
            {
                shootBullet();
                isshooting = true;
            }
        }

        if (isshooting)
        {
            shootdelay -= Time.deltaTime;
        }
        if (shootdelay <= 0)
        {
            shootdelay = saveDelay;
            isshooting = false;
        }

        if (isCurrentlyHooking)
        {
            Vector3 hookDirection = objectPosition - gunPosition.position;
            hookVelocity = hookDirection * (dragSpeeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                isCurrentlyHooking = false;
                hookVelocity = Vector3.zero;
                lineRenderer.useWorldSpace = false;
            }
        }
        controller.moveCharacter(hookVelocity);
    }

    void drawRope()
    {
        if (!isCurrentlyHooking)
        {
            return;
        }
        lineRenderer.SetPosition(0, gunPosition.position);
        lineRenderer.SetPosition(1, objectPosition);
    }

    private void graplingHook()
    {
        if (!isCurrentlyHooking)
        {
            Ray graplingHookRay = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(graplingHookRay, out hit, maxHookDistance,~player))
            {
                isCurrentlyHooking = true;
                objectPosition = hit.point;
                lineRenderer.useWorldSpace = true;
            }
        }

    }

    [PunRPC]
    void playBulletSound()
    {
        AudioSource audioRPC = gameObject.GetComponent<AudioSource>();
        audioRPC.clip = shoot;
        audioRPC.spatialBlend = 1;
        audioRPC.minDistance = 25;
        audioRPC.maxDistance = 100;
        audioRPC.Play();
    }

    private void shootBullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, cam.transform.position, Quaternion.identity,0);
        photonView.RPC("playBulletSound", RpcTarget.All);
        bullet.GetComponent<Rigidbody>().AddForce(cam.transform.forward * shootForce, ForceMode.Impulse);
    }

}
