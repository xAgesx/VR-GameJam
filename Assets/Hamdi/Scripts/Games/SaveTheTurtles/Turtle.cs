using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float fillSpeed = 20f; // % per second

    public float player1Progress = 0f;
    public float player2Progress = 0f;

    private PlayerController player1;
    private PlayerController player2;

    private bool freed = false;

    public GameObject wire; // assign child object in inspector

    void Update()
    {
        if (freed)
            return;

        if (player1 != null)
            player1Progress += fillSpeed * Time.deltaTime;

        if (player2 != null)
            player2Progress += fillSpeed * Time.deltaTime;

        float total = player1Progress + player2Progress;

        if (total >= 100f)
        {
            FreeTurtle();
        }
    }

    void FreeTurtle()
    {
        freed = true;

        // remove wire
        if (wire != null)
            wire.SetActive(false);

        // determine winner
        PlayerController winner = null;

        if (player1Progress > player2Progress)
            winner = player1;
        else
            winner = player2;

        if (winner != null)
            winner.AddScore(50); // reward points

        // despawn after 5 seconds
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
            return;

        if (player.name.Contains("1"))
            player1 = player;
        else
            player2 = player;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
            return;

        if (player == player1)
            player1 = null;

        if (player == player2)
            player2 = null;
    }
}
