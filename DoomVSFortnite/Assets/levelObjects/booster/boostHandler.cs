using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostHandler : MonoBehaviour
{
    private PlayerMovement player;
    public float gravity = -9.81f;
    public float jumpHeight = 5000f;
    bool isOnBooster;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        if(isOnBooster)
        {
            if (player.photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity = player.controller.velocity;
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    player.moveCharacter(velocity);
                    player.velocity.y = velocity.y;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("yeah");
        if (other.tag == "playerfeet")
        {
            isOnBooster = true;
            player = other.GetComponentInParent<PlayerMovement>(); 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("no");
        isOnBooster = false;
    }

}
