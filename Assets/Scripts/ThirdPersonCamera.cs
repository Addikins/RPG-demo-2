using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] Transform Target, Player;
    [SerializeField] float distanceFromTarget = 2f;

    float mouseX, mouseY;

    void Start()
    {
    //     Cursor.visible = false;
    //     Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    void CameraMovement()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        Vector3 targetRotation = new Vector3(mouseY, mouseX);
        transform.eulerAngles = targetRotation;


        // transform.LookAt(Target);

        // Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        // Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }


}
