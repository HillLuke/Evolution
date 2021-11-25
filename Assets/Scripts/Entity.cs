using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public float WalkableDistance;
    public LayerMask layerMask;

    private NavMeshAgent _agent;
    private Transform _goal;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!_agent.isStopped)
        {
            Vector3 direction = Random.insideUnitSphere * WalkableDistance;
            direction += gameObject.transform.position;

            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(direction, out navMeshHit, WalkableDistance, layerMask);

            _agent.SetDestination(navMeshHit.position);
        }   
    }
}
