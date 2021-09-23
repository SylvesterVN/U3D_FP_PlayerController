using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //camera vars
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 0;
    [SerializeField] bool lockCursor = true;
    [SerializeField]float cameraPitch = 0.0f;
    public float cameraLookCapX = -50f;
    public float cameraLookCapY = 50f;
    [SerializeField] [Range(0f, 0.1f)] float mouseSmoothTime = 0.05f;

    //controller vars
    CharacterController controller = null;
    [SerializeField] [Range(0f, 0.1f)] float moveSmoothTime = 0.05f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float gravity = -13f;


    //global vars
    float velocityY = 0f;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDireVelocity = Vector2.zero;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    public void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        controller = GetComponent<CharacterController>();


        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        
    }

    public void Update()
    {
        UpdateMovement();
        UpdateMouseLook();

    }

    public void UpdateMovement()
    {
        //look up get axis raw
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetDir.Normalize();

        //look into this 
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDir, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up *velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocityY = 0f;
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
        }

        
    }

    public void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y *  mouseSensitivity;

        //check what mathF is 
        cameraPitch = Mathf.Clamp(cameraPitch, cameraLookCapX, cameraLookCapY);
        
        //look up euler angles
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
}
