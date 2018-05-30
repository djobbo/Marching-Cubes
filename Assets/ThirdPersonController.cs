using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour {

    public Rigidbody rb;
    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    public bool grounded = false;
    public float jumpForce = 10f;
    public KeyCode forwardKey, leftKey, backwardKey, rightKey, jumpKey;
    public float speed = 2f;
    public float swimmingSpeed = 1f;

    private void Update()
    {
        var move = Vector3.zero;
        if (Input.GetKey(forwardKey)) move.z += 1;
        if (Input.GetKey(leftKey)) move.x -= 1;
        if (Input.GetKey(rightKey)) move.x += 1;
        if (Input.GetKey(backwardKey)) move.z -= 1;

        grounded = Physics.Raycast(groundCheck.position, -transform.up, groundCheckDistance);

        transform.position += (transform.right * move.x + transform.forward * move.z) * speed * Time.deltaTime;

        if (grounded && Input.GetKeyDown(jumpKey)) Jump();

    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce);
    }
}
