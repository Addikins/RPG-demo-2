using System;
using System.Collections;
using UnityEngine;

namespace RPG.Control {

    public class ThirdPersonCharacterController : MonoBehaviour {

        [SerializeField] float gravity = -10f;
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float runSpeed = 6f;
        [SerializeField] float rollSpeed = 5f;
        [SerializeField] float maxRollTime = 1f;
        [SerializeField] float rollDelay = .25f;
        [SerializeField] float fallDelay = .3f;
        [SerializeField] float speedSmoothTime = 0.15f;
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
            Falling,
        }

        private float currentRunSpeed;
        private float maxSpeed;
        private float defaultRunAnimation;
        private float distanceToGround;
        private Vector3 velocity;
        private float velocityY;
        private float timeOffGround;

        float turnSmoothVelocity;
        float speedSmoothVelocity;
        float currentSpeed;
        Transform cameraT;
        Animator animator;
        CharacterController controller;

        void Start () {
            state = State.Normal;
            animator = GetComponent<Animator> ();

            cameraT = Camera.main.transform;
            defaultRunAnimation = runningAnimationSpeed;
            currentSpeed = walkSpeed;
            animator.SetFloat ("rollSpeed", rollingAnimationSpeed);
            maxSpeed = runSpeed * 1.5f;

            // distanceToGround = GetComponent<SphereCollider> ().bounds.extents.y;
            controller = GetComponent<CharacterController> ();
        }

        void Update () {
            switch (state) {
                case State.Normal:
                    Movement ();
                    Roll ();
                    CheckGround ();
                    break;
                case State.Rolling:
                    Rolling ();
                    break;
                case State.Falling:
                    Falling ();
                    CheckGround ();
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

            HandleMovement ();

            if (controller.isGrounded) {
                velocityY = 0;
            }

            float animationSpeed = ((running) ? runningAnimationSpeed : walkingAnimationSpeed) * inputDir.magnitude;
            animator.SetFloat ("forwardSpeed", animationSpeed, speedSmoothTime, Time.deltaTime);
        }

        private void HandleMovement () {
            velocityY += gravity * Time.deltaTime;
            velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            controller.Move (velocity * Time.deltaTime);
        }

        private void Roll () {
            if (Input.GetButtonDown ("Jump")) {
                currentSpeed = Mathf.Max (rollSpeed, currentSpeed);
                animator.SetTrigger ("roll");
                state = State.Rolling;
                timeSpentRolling = 0f;
            }
        }

        private void Rolling () {
            float delay;
            //Differenciates between run-to-roll vs stand-to-roll
            if (animator.GetFloat ("forwardSpeed") > 1) { delay = 0; } else { delay = rollDelay; }

            timeSpentRolling += Time.deltaTime;
            if (timeSpentRolling > delay && timeSpentRolling < maxRollTime) {
                HandleMovement ();
            }

            if (timeSpentRolling >= maxRollTime) {
                state = State.Normal;
            }
        }

        private void Falling () {
            HandleMovement ();
            // controller.Move (velocity * Time.deltaTime);
            // transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }

        private void CheckGround () {
            // isGrounded = Physics.Raycast (transform.position + (Vector3.up * .2f) + (Vector3.forward * .1f), Vector3.down, distanceToGround + 0.5f) ||
            //     Physics.Raycast (transform.position + (Vector3.up * .2f) + (Vector3.forward * -.1f), Vector3.down, distanceToGround + 0.5f) ||
            //     Physics.Raycast (transform.position + (Vector3.up * .2f), Vector3.down, distanceToGround + 0.5f);
            if (controller.isGrounded) {
                animator.SetBool ("isGrounded", controller.isGrounded);
                state = State.Normal;
                timeOffGround = 0;
                return;
            }
            timeOffGround += Time.deltaTime;
            
            if (timeOffGround > fallDelay) {
                animator.SetBool ("isGrounded", controller.isGrounded);
                state = State.Falling;
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
}