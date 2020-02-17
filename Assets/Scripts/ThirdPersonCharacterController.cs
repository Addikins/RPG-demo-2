using System;
using System.Collections;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour {

    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float rollSpeed = 2f;
    [SerializeField] float speedSmoothTime = 0.1f;
    [SerializeField] float turnSmoothTime = 0.2f;
    [SerializeField] float walkingAnimationSpeed = .5f;
    [SerializeField] float runningAnimationSpeed = 1f;
    [SerializeField] float maxAnimationSpeed = 1.5f;
    [SerializeField] bool hasSpeedBonus = true;

    private State state;
    private enum State {
        Normal,
        Rolling,
    }

    private float currentRunSpeed;
    private float maxSpeed;
    private float defaultRunAnimation;
    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    Animator animator;
    Transform cameraT;

    void Start () {
        state = State.Normal;

        animator = GetComponent<Animator> ();
        cameraT = Camera.main.transform;
        defaultRunAnimation = runningAnimationSpeed;
        maxSpeed = runSpeed * 1.5f;
    }

    void Update () {
        switch (state) {
            case State.Normal:
                Movement ();
                Roll ();
                break;
            case State.Rolling:
                Rolling ();
                break;
        }
    }

    private void Movement () {
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

    private void Roll () {
        if (Input.GetButtonDown ("Jump")) {
            animator.SetTrigger ("roll");
            state = State.Rolling;
        }
    }

    private void Rolling () {
        transform.Translate (transform.forward * rollSpeed * Time.deltaTime, Space.World);

        rollSpeed -= rollSpeed * 5f * Time.deltaTime;

        if (rollSpeed < .01f) {
            state = State.Normal;
            Debug.Log (state.ToString ());
        }
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