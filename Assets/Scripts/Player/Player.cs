using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float movementSpeed = 12.5f,
          normalMoveSpeed = 12.5f,
          rotationSpeedMouse = 240,
          focusRotationSpeedMouse = 100,
          maxRotationSpeedController = 350,
          currentRotationSpeedController = 50,
          focusRotationSpeedController = 30,
          focusZoom = 30,
          focusMovementSpeed = 4.5f,
          shakeTimer = 0,
          shakeSpeed = 0.01f,
          shakePosition = 0;
    bool isFocussing = false,
         isRunning = false,
         isMoving = false,
         shakeDown = true,
         rotateLeft = true,
         lastRotLeft = true;
    [SerializeField] Camera cam;
    
    Rigidbody rigB;

    private void Start()
    {
        rigB = GetComponent<Rigidbody>();
        AttachCamera();
    }

    void Update()
    {
        KeyAndMouseInput();
        ControllerInput();
        RunMovement();

        CheckCollisionFinish();
        CameraFollow();

        if (isMoving)
        {
            ShakeCamera();
        }

    }

    private void RunMovement()
    {
        if (Input.GetAxis("Controller Button L Press") != 0 && !isFocussing)
        {
            movementSpeed = 20;
            isRunning = true;
            shakeSpeed = 0.01f;
        }
        else
        {
            isRunning = false;
            shakeSpeed = 0.005f;
        }
    }

    private void ControllerInput()
    {
        float mx = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime,
              my = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime,
              lx = Input.GetAxis("Controller X"),
              ly = Input.GetAxis("Controller Y");

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (lx > 0 || lx < 0)
        {
            if (lx > 0) { rotateLeft = false; }
            if (lx < 0) { rotateLeft = true; }

            if (lastRotLeft != rotateLeft) { currentRotationSpeedController = 50; }
            if (lx < 0.1f && lx > 0) { currentRotationSpeedController = 50; }
            if (lx > -0.1f && lx < 0) { currentRotationSpeedController = 50; }

            if (currentRotationSpeedController < maxRotationSpeedController) { currentRotationSpeedController += 2f; }

            lastRotLeft = rotateLeft;
        }


        transform.Translate(mx, 0, my);
        if (Input.GetAxis("L2") == 0)
        {
            cam.transform.Rotate(new Vector3(ly, lx, 0) * currentRotationSpeedController * Time.deltaTime);
            cam.fieldOfView = 60;
            movementSpeed = normalMoveSpeed;
            isFocussing = false;

        }
        else
        {
            cam.transform.Rotate(new Vector3(ly, lx, 0) * focusRotationSpeedController * Time.deltaTime);
            cam.fieldOfView = focusZoom;
            movementSpeed = focusMovementSpeed;
            isFocussing = true;
        }

        if (Input.GetAxis("Controller Button X") != 0 && Physics.Raycast(this.transform.position, Vector3.down * 2f, out RaycastHit hitInfo, 1.1f)) { rigB.AddForce(new Vector3(0, 550, 0)); }

        this.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);

    }

    private void KeyAndMouseInput()
    {
        //Basic movement with WASD
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0, 0, movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0, 0, -movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space) && Physics.Raycast(this.transform.position, Vector3.down * 1.5f, out RaycastHit hitInfo, 1.1f))
        {
            rigB.AddForce(new Vector3(0, 550, 0));
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        MouseRotation();
    }

    private void ShakeCamera()
    {
        if (!isFocussing)
        {
            if (shakePosition > 0.15f)
            {
                shakeDown = true;
            }
            else if (shakePosition < -0.15f)
            {
                shakeDown = false;
            }

            if (shakeDown)
            {
                shakePosition -= shakeSpeed;
                cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1 + shakePosition, this.transform.position.z);
            }
            else
            {
                shakePosition += shakeSpeed;
                cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1 + shakePosition, this.transform.position.z);
            }
        }
    }

    private void MouseRotation()
    {
        cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeedMouse * Time.deltaTime);
        this.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);
    }

    private void CameraFollow()
    {
        if (!isMoving)
        {
            cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
        if (isFocussing)
        {
            cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
    }

    private void AttachCamera()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        cam.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    //Casts a sphere raycast that only checks for layer 10 (Finish).
    private void CheckCollisionFinish()
    {
        if (Physics.Raycast(this.transform.position, Vector3.forward, out RaycastHit hit1, 1, 1 << 10) ||
            Physics.Raycast(this.transform.position, Vector3.left, out RaycastHit hit2, 1, 1 << 10) ||
            Physics.Raycast(this.transform.position, Vector3.right, out RaycastHit hit3, 1, 1 << 10) ||
            Physics.Raycast(this.transform.position, Vector3.back, out RaycastHit hit4, 1, 1 << 10))
        {
            //GameObject.Find("SetupManager").GetComponent<Setup>().ResetGame();
        }
    }
}
