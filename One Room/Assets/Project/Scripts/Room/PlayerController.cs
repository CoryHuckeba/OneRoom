using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    public Rigidbody2D rb;

    public bool canMove = true;
    public float moveSpeed = 300f;

    private float horizontalSpeed = 0f;
    private float verticalSpeed = 0f;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            verticalSpeed = 0f;
            horizontalSpeed = 0f;
            // Move Up
            if (Input.GetKey(KeyCode.W))
            {
                verticalSpeed += moveSpeed;
            }
            // Move Down
            if (Input.GetKey(KeyCode.S))
            {
                verticalSpeed -= moveSpeed;
            }
            // Move Left
            if (Input.GetKey(KeyCode.A))
            {
                horizontalSpeed -= moveSpeed;
            }
            // Move right
            if (Input.GetKey(KeyCode.D))
            {
                horizontalSpeed += moveSpeed;
            }

            rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }
    }
}
