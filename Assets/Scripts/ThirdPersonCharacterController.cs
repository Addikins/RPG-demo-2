using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    

    private void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(horizontalInput, 0f, verticalInput).normalized * speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }
}
