using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 velocity;
    public Transform playerBody;
    public Camera cam;
    public Transform hand;
    public GameObject bombprefab;
    public float throwforce;
    public float bombTimer = 10f;
    float saveBomb;

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

    public float dashSpeed = 50f;
    public float dashTime = 2f;
    public float dashColdown = 4f;
    public float mutliDashes = 2f;

    /*Climbing*/
    public Transform headCollider;
    public LayerMask wallLayer;
    public float climbDetection = 1f;

    /*
    public Text dashCD;
    public Text dashCNT;
    public Text jumpCNT;
    public Text bombCD;
    */
    bool throwedBomb;


    private void Start()
    {
        saveCdTime = dashColdown;
        saveMaxDashes = mutliDashes;
        saveDashTime = dashTime;
        saveBomb = bombTimer;
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

        if (!throwedBomb)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                throwBomb();
                throwedBomb = true;
            }
        }

        if (bombTimer <= 0f)
        {
            bombTimer = saveBomb;
            throwedBomb = false;
        }

        if (throwedBomb)
        {
            bombTimer -= Time.deltaTime;
        }

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
        moveCharacter(velocity);
        moveCharacter(move * movementSpeed);
        //jumpCNT.text = "" + multiJumps;
        //bombCD.text = "" + bombTimer;
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

        //dashCNT.text = "" + mutliDashes;
        //dashCD.text = "" + dashColdown;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(headCollider.position, climbDetection);
        Gizmos.DrawSphere(groundCollider.position, climbDetection);
    }

    void throwBomb()
    {
        GameObject bomb = Instantiate(bombprefab, hand.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwforce, ForceMode.Impulse);
    }
}
