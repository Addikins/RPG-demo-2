using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {
    CharacterController controller;
    // Start is called before the first frame update
    [SerializeField] float speed = 6.0F;
    [SerializeField] float jumpSpeed = 8.0F;
    [SerializeField] float gravity = 20.0F;
    [SerializeField] float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    private Vector3 moveDirection = Vector3.zero;
    private float targetSpeed;
    private bool running;

    Transform cameraT;

    void Start () {
        controller = GetComponent<CharacterController> ();
        cameraT = Camera.main.transform;
    }

    void Update () {
        Movement ();
    }

    private void Movement () {
        if (controller.isGrounded) {
            running = Input.GetKey (KeyCode.LeftShift);
            if (running) { targetSpeed = speed * 1.5f; } else { targetSpeed = speed; }

            moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
            moveDirection = transform.TransformDirection (moveDirection);
            moveDirection *= targetSpeed;

            // if (moveDirection != Vector3.zero) {
            //     float targetRotation = Mathf.Atan2 (moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            //     transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            // }

            if (Input.GetButton ("Jump")) {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move (moveDirection * Time.deltaTime);
    }
}