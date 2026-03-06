using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerID
    {
        Player1,
        Player2
    }

    public PlayerID player;

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        bool jumpPressed = false;

        if (player == PlayerID.Player1)
        {
            if (Input.GetKey(KeyCode.A)) horizontal = -1;
            if (Input.GetKey(KeyCode.D)) horizontal = 1;
            if (Input.GetKey(KeyCode.W)) vertical = 1;
            if (Input.GetKey(KeyCode.S)) vertical = -1;

            jumpPressed = Input.GetKeyDown(KeyCode.LeftShift);
        }
        else if (player == PlayerID.Player2)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1;
            if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1;
            if (Input.GetKey(KeyCode.UpArrow)) vertical = 1;
            if (Input.GetKey(KeyCode.DownArrow)) vertical = -1;

            jumpPressed = Input.GetKeyDown(KeyCode.RightShift);
        }

        Vector3 move = new Vector3(horizontal, 0, vertical) * moveSpeed;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x;
        velocity.z = move.z;
        rb.linearVelocity = velocity;

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
