using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] Transform target;
    [SerializeField] float distanceFromTarget = 2f;
    [SerializeField] Vector2 verticalMinMax = new Vector2(-35, 85);
    [SerializeField] float rotationSmoothTime = .12f;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float mouseX, mouseY;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    void CameraMovement()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, verticalMinMax.x, verticalMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX), ref rotationSmoothVelocity, rotationSmoothTime);


        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
