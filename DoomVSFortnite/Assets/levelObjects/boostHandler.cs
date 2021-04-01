using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostHandler : MonoBehaviour
{
    public PlayerMovement player;
    public LayerMask playerMask;
    public Transform playerFinder;

    public float playerDistance = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 10f;


    bool hasPlayerOnIt;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        hasPlayerOnIt = Physics.CheckSphere(playerFinder.position, playerDistance, playerMask);
        if(hasPlayerOnIt)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = player.controller.velocity;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                player.moveCharacter(velocity);
                player.velocity = velocity;            
            }
        }
    }

}
