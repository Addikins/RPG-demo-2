using System;
using System.Collections;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour {

    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 6;
    [SerializeField] float speedSmoothTime = 0.1f;
    [SerializeField] float turnSmoothTime = 0.2f;
    [SerializeField] float walkingAnimationSpeed = .5f;
    [SerializeField] float runningAnimationSpeed = 1f;
    [SerializeField] float maxAnimationSpeed = 1.5f;
    [SerializeField] bool hasSpeedBonus;

    private float currentRunSpeed;
    private float maxSpeed;
    private float defaultRunAnimation;
    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    Animator animator;
    Transform cameraT;

    void Start () {
        animator = GetComponent<Animator> ();
        cameraT = Camera.main.transform;
        defaultRunAnimation = runningAnimationSpeed;
        maxSpeed = runSpeed * 1.5f;
    }

    void Update () {
        if (Input.GetButtonDown ("Jump")) { hasSpeedBonus = !hasSpeedBonus; }

        CheckBonusSpeed ();
        Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = Input.GetKey (KeyCode.LeftShift);
        float targetSpeed = ((running) ? currentRunSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);

        float animationSpeed = ((running) ? runningAnimationSpeed : walkingAnimationSpeed) * inputDir.magnitude;
        animator.SetFloat ("forwardSpeed", animationSpeed, speedSmoothTime, Time.deltaTime);

    }

    private void CheckBonusSpeed () {
        if (hasSpeedBonus) {
            currentRunSpeed = maxSpeed;
            runningAnimationSpeed = maxAnimationSpeed;
            return;
        }
        currentRunSpeed = runSpeed;
        runningAnimationSpeed = defaultRunAnimation;
    }
}