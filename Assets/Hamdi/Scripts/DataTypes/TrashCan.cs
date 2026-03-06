using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public int canID;

    public void TryThrowTrash(PlayerController player)
    {
        if (player.currentTrash == null)
            return;

        if (player.currentTrash.trashID == canID)
        {
            player.AddScore(10);
        }
        else
        {
            player.AddScore(-30);
        }

        Destroy(player.currentTrash.gameObject);
        player.currentTrash = null;
    }
}
