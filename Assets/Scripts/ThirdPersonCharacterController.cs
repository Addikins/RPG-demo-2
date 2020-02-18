using System;
using System.Collections;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour {

    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float rollSpeed = 2f;
    [SerializeField] float rollTime = 2.4f;
    [SerializeField] float rollDelay = .5f;
    [SerializeField] float maxRollTime = 1.5f;
    [SerializeField] float speedSmoothTime = 0.1f;
    [SerializeField] float turnSmoothTime = 0.2f;
    [SerializeField] float walkingAnimationSpeed = .5f;
    [SerializeField] float runningAnimationSpeed = 1f;
    [SerializeField] float rollingAnimationSpeed = 1f;
    [SerializeField] float maxAnimationSpeed = 1.5f;
    [SerializeField] float bonusSpeedMultiplier = 1.5f;
    [SerializeField] bool hasSpeedBonus = true;
    private State state;
    private float timeSpentRolling;

    private enum State {
        Normal,
        Rolling,
    }

    private float currentRunSpeed;
    private float currentRollSpeed;
    private float maxSpeed;
    private float defaultRunAnimation;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    Transform cameraT;
    Animator animator;

    void Start () {
        state = State.Normal;
        animator = GetComponent<Animator> ();

        cameraT = Camera.main.transform;
        defaultRunAnimation = runningAnimationSpeed;
        maxSpeed = runSpeed * bonusSpeedMultiplier;
        animator.SetFloat ("rollSpeed", rollingAnimationSpeed);
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
        bool running = Input.GetKey (KeyCode.LeftShift);
        Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
        float targetSpeed = ((running) ? currentRunSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);

        float animationSpeed = ((running) ? runningAnimationSpeed : walkingAnimationSpeed) * inputDir.magnitude;
        animator.SetFloat ("forwardSpeed", animationSpeed, speedSmoothTime, Time.deltaTime);
    }

    private void Roll () {
        if (Input.GetButtonDown ("Jump")) {
            currentRollSpeed = rollSpeed;
            animator.SetTrigger ("roll");
            state = State.Rolling;
            timeSpentRolling = 0f;

            // animator.SetFloat ("rollSpeed", rollSpeed);
        }
    }

    private void Rolling () {
        float delay;
        if (animator.GetFloat ("forwardSpeed") > 1) { delay = 0; } else { delay = rollDelay; }

        timeSpentRolling += Time.deltaTime;
        if (timeSpentRolling > delay && timeSpentRolling < maxRollTime) {
            transform.Translate ((transform.forward * Time.deltaTime * currentRunSpeed), Space.World);
        }

        if (timeSpentRolling >= maxRollTime) {
            animator.SetFloat ("forwardSpeed", .5f);
        }

        if (timeSpentRolling >= rollTime) {
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