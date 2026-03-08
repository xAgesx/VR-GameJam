using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Settings")]
    public List<GameObject> trashPrefabs;
    public int trashAmount = 10;

    [Header("Spawn Area")]
    public Collider spawnPlatform;

    [Header("Ignore Collisions")]
    public List<Collider> trashCans;

    private List<GameObject> spawnedTrash = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < trashAmount; i++)
        {
            SpawnTrash();
        }
    }

    void Update()
    {
        spawnedTrash.RemoveAll(t => t == null);

        while (spawnedTrash.Count < trashAmount)
        {
            SpawnTrash();
        }
    }

    void SpawnTrash()
    {
        if (trashPrefabs.Count == 0 || spawnPlatform == null) return;

        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Count)];

        Vector3 spawnPos = GetRandomPointOnPlatform();

        GameObject trash = Instantiate(prefab, spawnPos, Quaternion.identity);

        SetupCollisions(trash);

        spawnedTrash.Add(trash);
    }

    Vector3 GetRandomPointOnPlatform()
    {
        Bounds bounds = spawnPlatform.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, bounds.max.y + 0.2f, z);
    }

    void SetupCollisions(GameObject trash)
    {
        Collider trashCollider = trash.GetComponent<Collider>();

        if (trashCollider == null) return;

        // Ignore trashcan collisions
        foreach (Collider can in trashCans)
        {
            Physics.IgnoreCollision(trashCollider, can);
        }

        // Ignore collision with other trash
        foreach (GameObject other in spawnedTrash)
        {
            if (other == null) continue;

            Collider otherCol = other.GetComponent<Collider>();
            if (otherCol != null)
            {
                Physics.IgnoreCollision(trashCollider, otherCol);
            }
        }
    }
}
