using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampolinHandler : MonoBehaviour
{
    public PlayerMovement player;
    public float bounciness = 0.75f;

    public float gravity = -9.81f;
    public float jumpHeight = 15f;

    private bool isPlayerOnit;
    Vector3 velocity;

    void Update()
    {
        if (isPlayerOnit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                player.moveCharacter(velocity);
                player.velocity = velocity;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        velocity = player.velocity;
        isPlayerOnit = true;

        velocity.y = (velocity.y * -1) * bounciness;
        player.velocity = velocity;
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerOnit = false;
    }


}
