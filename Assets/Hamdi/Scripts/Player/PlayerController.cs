using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerScore = 0;

    public Trash currentTrash;

    public Transform holdPoint; // assign a point above the head in inspector

    public void PickTrash(Trash trash)
    {
        if (currentTrash != null)
            return;

        currentTrash = trash;

        // Attach trash to player head
        trash.transform.SetParent(holdPoint);
        trash.transform.position = holdPoint.transform.position;
    }

    public void AddScore(int amount)
    {
        playerScore += amount;

        if (playerScore < 0)
            playerScore = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Trash trash = other.GetComponent<Trash>();

        if (trash != null)
        {
            PickTrash(trash);
        }

        TrashCan can = other.GetComponent<TrashCan>();

        if (can != null)
        {
            can.TryThrowTrash(this);
        }
    }
}
