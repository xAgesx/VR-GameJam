using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject damagePrefab;

    public int maxDamages = 5;
    public float spawnDelay = 3f;
    public float minDistanceBetweenDamages = 1.5f;
    public bool continuosSpawn = false;
    private Collider pipeCollider;
    public float Yoffset ;
    private List<GameObject> activeDamages = new List<GameObject>();

    void Start()
    {
        pipeCollider = GetComponent<Collider>();

        for (int i = 0; i < maxDamages; i++)
        {
            SpawnDamage();
            
        }
        //Spawn periodically for middle pipes
        if (continuosSpawn)
            {
                StartCoroutine("SpawnPerdiodically");
            }
    }
    public IEnumerator SpawnPerdiodically()
    {
        
        yield return new WaitForSeconds(20f);

    while (continuosSpawn)
    {
        yield return new WaitForSeconds(Random.Range(2, 6));
        SpawnDamage();
        Debug.Log("Periodic spawn");
    }
    }
    public void SpawnDamage()
    {
        if (activeDamages.Count >= maxDamages)
            return;

        Vector3 randomPos;
        int attempts = 0;

        do
        {
            randomPos = GetRandomPointInCollider();
            attempts++;
        }
        while (IsTooClose(randomPos) && attempts < 20);

        GameObject damage = Instantiate(damagePrefab, randomPos, Quaternion.identity);

        DamageFilter filter = damage.GetComponent<DamageFilter>();
        filter.pipe = this;

        activeDamages.Add(damage);
    }

    public void OnFilterRepaired(GameObject damage)
    {
        activeDamages.Remove(damage);

        //Invoke(nameof(SpawnDamage), spawnDelay);
    }

    bool IsTooClose(Vector3 position)
    {
        foreach (GameObject d in activeDamages)
        {
            if (Vector3.Distance(position, d.transform.position) < minDistanceBetweenDamages)
                return true;
        }

        return false;
    }

    Vector3 GetRandomPointInCollider()
    {
        Bounds bounds = pipeCollider.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Yoffset,
            (bounds.min.z + bounds.max.z) / 2
        );
    }
}
