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
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (player == PlayerID.Player1)
        {
            if (Input.GetKey(KeyCode.A)) horizontal = -1;
            if (Input.GetKey(KeyCode.D)) horizontal = 1;
            if (Input.GetKey(KeyCode.W)) vertical = 1;
            if (Input.GetKey(KeyCode.S)) vertical = -1;
        }
        else if (player == PlayerID.Player2)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1;
            if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1;
            if (Input.GetKey(KeyCode.UpArrow)) vertical = 1;
            if (Input.GetKey(KeyCode.DownArrow)) vertical = -1;
        }

        Vector3 move = new Vector3(horizontal, 0, vertical);

        // Movement
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x * moveSpeed;
        velocity.z = move.z * moveSpeed;
        rb.linearVelocity = velocity;

        // Rotation
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Animation
        bool isMoving = move != Vector3.zero;
        animator.SetBool("isWalking", isMoving);
    }
}
