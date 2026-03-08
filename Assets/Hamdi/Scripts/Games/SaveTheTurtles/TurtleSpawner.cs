using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleSpawner : MonoBehaviour
{
    public List<GameObject> turtlePrefabs;

    public int turtleAmount = 5;
    public float spawnRadius = 30f;

    private List<GameObject> turtles = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < turtleAmount; i++)
        {
            SpawnTurtle();
        }
    }

    void Update()
    {
        turtles.RemoveAll(t => t == null);

        while (turtles.Count < turtleAmount)
        {
            SpawnTurtle();
        }
    }

    void SpawnTurtle()
    {
        if (turtlePrefabs.Count == 0) return;

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        {
            GameObject prefab = turtlePrefabs[Random.Range(0, turtlePrefabs.Count)];

            GameObject turtle = Instantiate(prefab, hit.position, Quaternion.identity);

            turtles.Add(turtle);
        }
    }
}
