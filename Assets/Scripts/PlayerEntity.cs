
using UnityEngine;
using UnityEngine.AI;

public class PlayerEntity : NetworkIdentity
{
    public NavMeshAgent agent;
    public Vector3 targetPosition;
    public Vector3 currPosition
    {
        set { gameObject.transform.position = value; }
        get { return gameObject.transform.position; }
    }
    
    public void UpdateDestination()
    {
        agent.SetDestination(targetPosition);
    }
}
