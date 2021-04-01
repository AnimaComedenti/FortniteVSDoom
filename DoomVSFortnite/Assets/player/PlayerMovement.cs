using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 12f;
    public CharacterController controller;
    public float gravity = -9.81f;
    public float jumpHeight = 6f;

    public Transform groundCollider;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask boosterMask;
    public LayerMask trampolinMask;

    public float multiJumps = 2f;

    public float dashSpeed = 50f;
    public float dashTime = 2f;
    public float dashColdown = 4f;
    public float mutliDashes = 2f;

    public Vector3 velocity;
    bool isGrounded;
    bool onBoost;
    bool onTrampolin;

    bool isDashing;
    float cdTime;
    float dashes;
    float saveDashTime;

    private void Start()
    {
        cdTime = dashColdown;
        dashes = mutliDashes;
        saveDashTime = dashTime;
    }

    // Update is called once per frame
    void Update()
    {

        /*Grounded*/
        isGrounded = checkGroundend(groundMask);
        onBoost = checkGroundend(boosterMask);
        onTrampolin = checkGroundend(trampolinMask);

        if ((isGrounded && velocity.y < 0) || (onBoost && velocity.y < 0))
        {
            velocity.y = -2f;
            multiJumps = 2f;
        }

        if (onTrampolin)
        {
            multiJumps = 2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        /*Jump*/
        if (!onBoost && !onTrampolin)
        {
            if (Input.GetKeyDown(KeyCode.Space) && multiJumps > 0)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                multiJumps--;
            }
        }

        /*Dash*/
        if (mutliDashes <= 1f)
        {
            dashColdown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && mutliDashes > 0)
        {
            mutliDashes--;
            Vector3 dash = move * dashSpeed;
            velocity.x = dash.x;
            velocity.z = dash.z;
            moveCharacter(velocity);
            isDashing = true;
        }

        if (dashColdown <= 0f)
        {
            dashColdown = cdTime;
            mutliDashes = dashes;
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
        }
        else
        {
            dashTime = saveDashTime;
        }

        if (dashTime <= 0)
        {
            velocity.x = 0f;
            velocity.z = 0f;
            dashTime = saveDashTime;
            isDashing = false;
        }

        /*Jump and Move*/
        moveCharacter(move * movementSpeed);
        velocity.y += gravity * Time.deltaTime;
        moveCharacter(velocity);
    }

    public void moveCharacter(Vector3 velocity)
    {
        controller.Move(velocity * Time.deltaTime);
    }

    private bool checkGroundend(LayerMask ground)
    {
        bool onGround = Physics.CheckSphere(groundCollider.position, groundDistance, ground);
        return onGround; 
    }
}
