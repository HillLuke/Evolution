using Assets.Scripts.Interfaces;
using Assets.Scripts.Manager;
using Assets.Scripts.Resources;
using Assets.Scripts.ScriptableObjects.Resources;
using Assets.Scripts.Utility;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Entity : MonoBehaviour
    {
        //    public ColliderEventRaiser InteractCollider;

        //    [Title("Serialized")]
        //    [SerializeField]
        //    private EntityProperties _entityProperties;

        //    [Title("Private Read Only")]
        //    [ShowInInspector, ReadOnly]
        //    private float _hunger;
        //    [ShowInInspector, ReadOnly]
        //    private float _thirst;
        //    //[ShowInInspector, ReadOnly]
        //    //private EAction _currentAction = EAction.Wandering;
        //    [ShowInInspector, ReadOnly]
        //    private float _interactTimer;
        //    [ShowInInspector, ReadOnly]
        //    private GameObject _interactGoal;
        //    [ShowInInspector, ReadOnly]
        //    private Vector3 _goal;
        //    [ShowInInspector, ReadOnly]
        //    private Vector3 _goalOriginal;
        //    [ShowInInspector, ReadOnly]
        //    private bool _isSelected;
        //    [ShowInInspector, ReadOnly]
        //    private NavMeshAgent _agent;
        //    [ShowInInspector, ReadOnly]
        //    private NavMeshPath _path;
        //    [ShowInInspector, ReadOnly]
        //    private EntityInteractor _entityInteractor;


        //    private void Awake()
        //    {
        //        _path = new NavMeshPath();
        //        _agent = GetComponent<NavMeshAgent>();
        //        if (_entityInteractor == null)
        //        {
        //            _entityInteractor = GetComponentInChildren<EntityInteractor>();
        //        }

        //        _isSelected = false;
        //        _thirst = _entityProperties.ThirstMax;
        //        _hunger = _entityProperties.HungerMax;


        //        InteractCollider.TriggerEnter.AddListener((other) =>
        //        {
        //            if (other.gameObject)
        //            {

        //            } 
        //        });
        //    }

        //    private void Start()
        //    {
        //        InteractCollider.SetSize(_entityProperties.InteractRange);
        //        if (_entityInteractor == null)
        //        {
        //            throw new ArgumentNullException(nameof(EntityInteractor));
        //        }
        //        _entityInteractor.Init(_entityProperties);
        //        _entityInteractor.ActoinInRange += InRange;

        //    }

        //    private void FixedUpdate()
        //    {
        //        if (_hunger <= 0f)
        //        {
        //            if (_interactGoal == null && !_currentAction.HasFlag(EAction.LookingForWater | EAction.GoingToWater | EAction.Drinking))
        //            {
        //                _currentAction = EAction.LookingForFood;
        //            }
        //        }
        //        else if (_currentAction != EAction.Eating)
        //        {
        //            _hunger -= Time.deltaTime * _entityProperties.HungerDecrease;
        //        }

        //        if (_thirst <= 0f)
        //        {
        //            if (_interactGoal == null && !_currentAction.HasFlag(EAction.LookingForFood | EAction.GoingToFood | EAction.Eating))
        //            {
        //                _currentAction = EAction.LookingForWater;
        //            }
        //        }
        //        else if (_currentAction != EAction.Drinking)
        //        {
        //            _thirst -= Time.deltaTime * _entityProperties.ThirstDecrease;
        //        }
        //    }

        //    void Update()
        //    {

        //        switch (_currentAction)
        //        {
        //            case EAction.Wandering:
        //                Wander();
        //                break;
        //            case EAction.LookingForWater:
        //                if (FindResource(EResourceType.Water))
        //                {
        //                    _currentAction = EAction.GoingToWater;
        //                }
        //                break;
        //            case EAction.GoingToWater:
        //                break;
        //            case EAction.Drinking:
        //                if (_interactTimer <= 0)
        //                {
        //                    if (_interactGoal == null)
        //                    {
        //                        _interactGoal = null;
        //                        _goal = Vector3.zero;
        //                        _currentAction = EAction.Wandering;
        //                        break;
        //                    }
        //                    _thirst += _interactGoal.GetComponent<Resource>().Value;
        //                    _thirst = Mathf.Clamp(_thirst, 0f, _entityProperties.ThirstMax);
        //                    _interactTimer = _entityProperties.InteractTime;

        //                    if (_thirst == _entityProperties.ThirstMax)
        //                    {
        //                        _interactGoal = null;
        //                        _goal = Vector3.zero;
        //                        _currentAction = EAction.Wandering;
        //                    }
        //                }
        //                else
        //                {
        //                    _interactTimer -= Time.deltaTime;
        //                }
        //                break;
        //            case EAction.LookingForFood:
        //                if (FindResource(EResourceType.Food))
        //                {
        //                    _currentAction = EAction.GoingToFood;
        //                }
        //                break;
        //            case EAction.GoingToFood:
        //                break;
        //            case EAction.Eating:
        //                if (_interactTimer <= 0)
        //                {
        //                    if (_interactGoal == null)
        //                    {
        //                        _interactGoal = null;
        //                        _goal = Vector3.zero;
        //                        _currentAction = EAction.Wandering;
        //                        break;
        //                    }

        //                    var resource = _interactGoal.GetComponent<Resource>();

        //                    if (resource == null)
        //                    {
        //                        _currentAction = EAction.Wandering;
        //                    }
        //                    else
        //                    {
        //                        //eat
        //                        _hunger += resource.Use(1f);
        //                        _hunger = Mathf.Clamp(_hunger, 0f, _entityProperties.HungerMax);
        //                        _interactTimer = _entityProperties.InteractTime;

        //                        if (_hunger == _entityProperties.HungerMax)
        //                        {
        //                            _interactGoal = null;
        //                            _goal = Vector3.zero;
        //                            _currentAction = EAction.Wandering;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    _interactTimer -= Time.deltaTime;
        //                }
        //                break;
        //        }
        //    }



        //    private void InRange()
        //    {
        //        _agent.isStopped = true;
        //        _agent.ResetPath();
        //        _agent.isStopped = false;
        //        _interactTimer = _entityProperties.InteractTime;

        //        switch (_currentAction)
        //        {
        //            case EAction.GoingToWater:
        //                _currentAction = EAction.Drinking;
        //                break;
        //            case EAction.GoingToFood:
        //                _currentAction = EAction.Eating;
        //                break;
        //        }
        //    }

        //    private bool DestinationReached()
        //    {
        //        if (!_agent.pathPending)
        //        {
        //            if (_agent.remainingDistance <= _agent.stoppingDistance)
        //            {
        //                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
        //                {
        //                    return true;
        //                }
        //            }
        //        }

        //        return false;
        //    }

        //    private void Wander()
        //    {
        //        if (_goal == Vector3.zero || DestinationReached())
        //        {
        //            Vector3 direction = Random.insideUnitSphere * Random.Range(_entityProperties.MinWalkable, _entityProperties.MaxWalkable);
        //            direction += gameObject.transform.position;

        //            NavMeshHit navMeshHit;
        //            NavMesh.SamplePosition(direction, out navMeshHit, _entityProperties.MaxWalkable, 1);
        //            _goalOriginal = navMeshHit.position;
        //            Vector3 pathDir = transform.position - _goalOriginal;
        //            var goal = _goalOriginal + (pathDir.normalized * (_agent.radius));

        //            if (_agent.CalculatePath(goal, _path))
        //            {
        //                _goal = goal;
        //                _agent.SetDestination(_goal);
        //            }
        //            else
        //            {
        //                _goal = Vector3.zero;
        //            }
        //        }
        //    }

        //    private bool FindResource(EResourceType resourceType)
        //    {
        //        LayerMask layerMask = -1;
        //        switch (resourceType)
        //        {
        //            case EResourceType.Food:
        //                layerMask = EvolutionManager.Instance.FoodMask;
        //                break;
        //            case EResourceType.Water:
        //                layerMask = EvolutionManager.Instance.WaterMask;
        //                break;
        //        }

        //        var resources = Physics.OverlapSphere(transform.position, 8f, layerMask);

        //        if (resources != null && resources.Length >= 1)
        //        {
        //            var clossestResource = resources[0];
        //            var dist = Vector3.Distance(transform.position, clossestResource.transform.position);

        //            foreach (var collider in resources)
        //            {
        //                var distCompare = Vector3.Distance(transform.position, collider.transform.position);
        //                if (distCompare < dist)
        //                {
        //                    clossestResource = collider;
        //                }
        //            }

        //            _goal = clossestResource.transform.position;
        //            _agent.SetDestination(_goal);
        //            _interactGoal = clossestResource.gameObject;

        //            _entityInteractor.SetInteractTarget(_interactGoal);
        //            return transform;
        //        }
        //        else
        //        {
        //            if (_interactGoal == null)
        //            {
        //                Wander();
        //            }
        //            return false;
        //        }
        //    }

        //    private void OnDrawGizmos()
        //    {
        //        if (!_isSelected)
        //            return;

        //        //Gizmos.color = Color.yellow;
        //        //Gizmos.DrawCube(_goalOriginal, new Vector3(0.5f, 0.5f, 0.5f));
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawCube(_goal, new Vector3(0.5f, 0.5f, 0.5f));

        //        //Gizmos.color = Color.black;
        //        //foreach (var corner in _path.corners)
        //        //{
        //        //    Gizmos.DrawCube(corner, new Vector3(0.5f, 0.5f, 0.5f));
        //        //}
        //    }

        //    #region IMonitorable

        //    public string GetData()
        //    {
        //        return $"Hunger : {_hunger.ToString("#.##")}\nThirst : {_thirst.ToString("#.##")}\nState: {_currentAction}";
        //    }

        //    public void Select()
        //    {
        //        _isSelected = true;
        //    }

        //    public void DeSelect()
        //    {
        //        _isSelected = false;
        //    }

        //    #endregion
    }

}
