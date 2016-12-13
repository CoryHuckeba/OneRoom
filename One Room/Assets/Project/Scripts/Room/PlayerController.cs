using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController> {

    public Animator anim;
    public Rigidbody2D rb;
    public Text pauseInstruction;

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
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                verticalSpeed += moveSpeed;
            }
            // Move Down
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                verticalSpeed -= moveSpeed;
            }
            // Move Left
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalSpeed -= moveSpeed;
            }
            // Move right
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontalSpeed += moveSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu.Instance.Open();
            }

            // Set walking animation
            if (Mathf.Abs(horizontalSpeed) > 0)
            {
                anim.SetBool("walking_right", horizontalSpeed > 0);
                anim.SetBool("walking_left", horizontalSpeed < 0);
            }
            else if (Mathf.Abs(verticalSpeed) > 0)
            {
                anim.SetBool("walking_right", verticalSpeed > 0);
                anim.SetBool("walking_left", verticalSpeed < 0);
            }

            rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }
    }

    public void AllowMove(bool can)
    {
        pauseInstruction.gameObject.SetActive(can);
        this.canMove = can;
        if (!can)
            rb.velocity = Vector2.zero;
    }
}
