using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 velocity;
    public Vector3 hookVelocity;
    public Transform playerBody;
    public Camera cam;

    /*Physics*/
    public float gravity = -9.81f;

    /*Jumping*/
    public float jumpHeight = 6f;
    public float multiJumps = 2f;

    /*Movement*/
    public float movementSpeed = 12f;

    /*Grounded*/
    bool isGrounded;
    bool onBoost;
    bool onTrampolin;

    public Transform groundCollider;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask boosterMask;
    public LayerMask trampolinMask;

    /*Dashing*/
    bool isDashing;
    float saveCdTime;
    float saveMaxDashes;
    float saveDashTime;
    float saveGravity;

    public float dashSpeed = 50f;
    public float dashTime = 2f;
    public float dashColdown = 4f;
    public float mutliDashes = 2f;

    /*Climbing*/
    public Transform headCollider;
    public LayerMask wallLayer;
    public float climbDetection = 1f;

    /*Hook*/
    Vector3 objectPosition;
    public float maxHookDistance;
    public float dragSpeeed;
    bool isCurrentlyHooking = false;


    private void Start()
    {
        saveCdTime = dashColdown;
        saveMaxDashes = mutliDashes;
        saveDashTime = dashTime;
        saveGravity = gravity;
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
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move;

        /*Climbing*/
        bool isClimbing = detectClimbing();

        /*Jump*/
        if (!onBoost && !onTrampolin)
        {
            if (Input.GetKeyDown(KeyCode.Space) && multiJumps > 0)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                multiJumps--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            graplingHook();
        }

        if (isCurrentlyHooking)
        {
            Vector3 hookDirection = objectPosition - transform.position;
            hookVelocity = hookDirection * (dragSpeeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Q) || getBetrag(hookVelocity.x,hookVelocity.y,hookVelocity.z) <= 8 )
            {
                isCurrentlyHooking = false;
                hookVelocity = Vector3.zero;
                gravity = saveGravity;
            }
        }

        if (isClimbing)
        {
            move = transform.up * z + transform.right * x;
            velocity.y = velocity.y < 0 ? 0 : velocity.y;
        }
        else
        {
            move = transform.right * x + transform.forward * z;
            velocity.y += gravity * Time.deltaTime;
        }


        /*Dashing*/
        dash(move);

        /*Move*/
        moveCharacter(hookVelocity);
        moveCharacter(velocity);
        moveCharacter(move * movementSpeed);
    }

    private float getBetrag(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
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

    private bool detectClimbing()
    {
        bool onTop = Physics.CheckSphere(headCollider.position, climbDetection, wallLayer);
        bool onBottom = Physics.CheckSphere(groundCollider.position, climbDetection, wallLayer);

        if (onBottom)
        {
            velocity.y = -2f;
            multiJumps = 1;
        }

        if (onTop || onBottom)
        {
            return true;
        }

        return false;
    }

    private void dash(Vector3 move)
    {
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
            dashColdown = saveCdTime;
            mutliDashes = saveMaxDashes;
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
    }

    private void graplingHook()
    {
        if (!isCurrentlyHooking)
        {
            Ray graplingHookRay = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(graplingHookRay, out hit, maxHookDistance))
            {
                isCurrentlyHooking = true;
                objectPosition = hit.point;
            }
        }
            
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(headCollider.position, climbDetection);
        Gizmos.DrawSphere(groundCollider.position, climbDetection);
    }
}
