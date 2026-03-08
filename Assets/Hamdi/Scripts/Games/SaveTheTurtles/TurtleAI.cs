using UnityEngine;
using UnityEngine.AI;

public class TurtleAI : MonoBehaviour
{
    NavMeshAgent agent;

    public float wanderRadius = 15f;
    public float wanderDelay = 5f;

    float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderDelay;
        agent.updateRotation = true;
    }

    void Update()
{
    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    {
        timer += Time.deltaTime;

        if (timer >= wanderDelay)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
}

    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }
}
