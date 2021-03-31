using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostHandler : MonoBehaviour
{
    public CharacterController controller;
    public LayerMask playerMask;
    public Transform playerFinder;
    public float playerDistance = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 10f;

    bool hasPlayerOnIt;

    // Update is called once per frame
    void Update()
    {
        hasPlayerOnIt = Physics.CheckSphere(playerFinder.position, playerDistance, playerMask);
        if (hasPlayerOnIt)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 velocity = controller.velocity;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }

}
