using UnityEngine;

public class DamageFilter : MonoBehaviour
{
    public float repairProgress = 0f;
    public float repairSpeed = 25f;

    private PlayerController repairingPlayer;

    public Pipe pipe;

    void Update()
    {
        if (repairingPlayer != null)
        {
            
            
            repairProgress += repairSpeed * Time.deltaTime;

            if (repairProgress >= 100f)
            {
                RepairComplete();
            }
        }
    }

    void RepairComplete()
    {
        repairingPlayer.AddScore(20);

        pipe.OnFilterRepaired(gameObject);

        repairingPlayer.transform.GetChild(1).GetComponent<Animator>().SetBool("Fixing",false);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            repairingPlayer = player;
            repairingPlayer.transform.GetChild(1).GetComponent<Animator>().SetBool("Fixing",true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == repairingPlayer)
        {
            repairingPlayer.transform.GetChild(1).GetComponent<Animator>().SetBool("Fixing",false);
            repairingPlayer = null;
            
        }
    }
}
