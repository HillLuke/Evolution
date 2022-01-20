using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public float WalkableDistance;
    public LayerMask layerMask;

    [SerializeField]
    private EntityProperties _entityProperties;
    [SerializeField]
    private EntityInteractor _entityInteractor;

    [SerializeField]
    private float _hunger;
    [SerializeField]
    private float _thirst;
    [SerializeField]
    private EAction _currentAction = EAction.Wandering;

    [Header("Interacting")]
    [SerializeField]
    private float _interactTimer;
    [SerializeField]
    private GameObject _interactGoal;
    [SerializeField]
    private Vector3 _goal;

    private NavMeshAgent _agent;

    public enum EAction
    {
        Wandering,
        LookingForWater,
        GoingToWater,
        Drinking,
        LookingForFood,
        GoingToFood,
        Eating
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_entityInteractor == null)
        {
            _entityInteractor = GetComponentInChildren<EntityInteractor>();
        }
        _entityInteractor.ActoinInRange += InRange;
    }

    private void FixedUpdate()
    {
        if (_hunger <= 0f)
        {
            if (_interactGoal == null && !_currentAction.HasFlag(EAction.LookingForWater | EAction.GoingToWater | EAction.Drinking))
            {
                _currentAction = EAction.LookingForFood;
            }
        }
        else if (_currentAction != EAction.Eating)
        {
            _hunger -= Time.deltaTime * _entityProperties.HungerDecrease;
        }
        
        if (_thirst <= 0f)
        {
            if (_interactGoal == null && !_currentAction.HasFlag(EAction.LookingForFood | EAction.GoingToFood | EAction.Eating))
            {
                _currentAction = EAction.LookingForWater;
            }
        }
        else if (_currentAction != EAction.Drinking)
        {
            _thirst -= Time.deltaTime * _entityProperties.ThirstDecrease;
        }
    }

    void Update()
    {
        switch (_currentAction)
        {
            case EAction.Wandering:
                Wander();
                break;
            case EAction.LookingForWater:
                if (FindResource(EResourceType.Water))
                {
                    _currentAction = EAction.GoingToWater;
                }
                break;
            case EAction.GoingToWater:
                break;
            case EAction.Drinking:
                if (_interactTimer <= 0)
                {
                    _thirst += _interactGoal.GetComponent<Resource>().Value;
                    _thirst = Mathf.Clamp(_thirst, 0f, _entityProperties.ThirstMax);
                    _interactTimer = _entityProperties.InteractTime;

                    if (_thirst == _entityProperties.ThirstMax)
                    {
                        _interactGoal = null;
                        _goal = Vector3.zero;
                        _currentAction = EAction.Wandering;
                    }
                }
                else
                {
                    _interactTimer -= Time.deltaTime;
                }
                break;
            case EAction.LookingForFood:
                if (FindResource(EResourceType.Food))
                {
                    _currentAction = EAction.GoingToFood;
                }
                break;
            case EAction.GoingToFood:
                break;
            case EAction.Eating:
                if (_interactTimer <= 0)
                {
                    //eat
                    _hunger += _interactGoal.GetComponent<Resource>().Value;
                    _hunger = Mathf.Clamp(_hunger, 0f, _entityProperties.HungerMax);
                    _interactTimer = _entityProperties.InteractTime;

                    if (_hunger == _entityProperties.HungerMax)
                    {
                        _interactGoal = null;
                        _goal = Vector3.zero;
                        _currentAction = EAction.Wandering;
                    }
                }
                else
                {
                    _interactTimer -= Time.deltaTime;
                }
                break;
        }
    }


    private void InRange()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.isStopped = false;
        _interactTimer = _entityProperties.InteractTime;

        switch (_currentAction)
        {
            case EAction.GoingToWater:
                _currentAction = EAction.Drinking;
                break;
            case EAction.GoingToFood:
                _currentAction = EAction.Eating;
                break;
        }
    }

    private bool DestinationReached()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Wander()
    {
        if (DestinationReached())
        {
            Vector3 direction = Random.insideUnitSphere * WalkableDistance;
            direction += gameObject.transform.position;

            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(direction, out navMeshHit, WalkableDistance, layerMask);
            _goal = navMeshHit.position;
            _agent.SetDestination(navMeshHit.position);
        }
    }

    private bool FindResource(EResourceType resourceType)
    {
        LayerMask layerMask = -1;
        switch (resourceType)
        {
            case EResourceType.Food:
                layerMask = EvolutionManager.Instance.FoodMask;
                break;
            case EResourceType.Water:
                layerMask = EvolutionManager.Instance.WaterMask;
                break;
        }

        var resources = Physics.OverlapSphere(transform.position, 8f, layerMask);

        if (resources != null && resources.Length >= 1)
        {
            var clossestResource = resources[0];
            var dist = Vector3.Distance(transform.position, clossestResource.transform.position);

            foreach (var collider in resources)
            {
                var distCompare = Vector3.Distance(transform.position, collider.transform.position);
                if (distCompare < dist)
                {
                    clossestResource = collider;
                }
            }

            _goal = clossestResource.transform.position;
            _agent.SetDestination(_goal);
            _interactGoal = clossestResource.gameObject;

            _entityInteractor.SetInteractTarget(_interactGoal);
            return transform;
        }
        else
        {
            if (_interactGoal == null)
            {
                Wander();
            }
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_goal, new Vector3(0.5f, 0.5f, 0.5f));
    }


}
