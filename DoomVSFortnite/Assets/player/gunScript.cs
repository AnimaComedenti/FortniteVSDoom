using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour
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



    private void LateUpdate()
    {
        drawRope();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            graplingHook();
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
}
