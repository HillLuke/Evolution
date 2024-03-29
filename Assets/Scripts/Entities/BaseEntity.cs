﻿using Assets.Scripts.Interfaces;
using Assets.Scripts.Manager;
using Assets.Scripts.ScriptableObjects.Entities;
using Assets.Scripts.ScriptableObjects.Identity;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public abstract class BaseEntity : MonoBehaviour, IEntity, IMonitorable, IIdentity
    {
        [ShowInInspector, ReadOnly]
        public Action Action { get; protected set; }

        [ShowInInspector, ReadOnly]
        public LookFor LookFor { get; protected set; }

        [ShowInInspector, ReadOnly]
        public State State { get; protected set; }

        public string Name => _entityProperties.Properties.Name;

        public float Health => _health;

        [ShowInInspector, ReadOnly]
        protected int _actionticks;

        [ShowInInspector, ReadOnly]
        protected NavMeshAgent _agent;

        [ShowInInspector, ReadOnly]
        protected int _birthticks;

        [ShowInInspector, ReadOnly]
        protected Collider[] _colliders;

        [SerializeField]
        protected EntityPropertiesSO _entityProperties;

        [ShowInInspector, ReadOnly]
        protected Vector3 _goal;

        [ShowInInspector, ReadOnly]
        protected Vector3 _goalOriginal;

        [ShowInInspector, ReadOnly]
        protected float _health;

        [ShowInInspector, ReadOnly]
        protected float _hunger;

        [ShowInInspector, ReadOnly]
        protected GameObject _interactGoal;

        [ShowInInspector, ReadOnly]
        protected bool _isPregnant;

        [ShowInInspector, ReadOnly]
        protected bool _isSelected;

        [ShowInInspector, ReadOnly]
        protected NavMeshPath _path;

        [ShowInInspector, ReadOnly]
        protected float _thirst;

        public virtual void Awake()
        {
            State = State.Wander;
            LookFor = LookFor.NONE;
            Action = Action.NONE;
            _path = new NavMeshPath();
            _agent = GetComponent<NavMeshAgent>();
            _agent.updatePosition = false;
            _isSelected = false;
            _thirst = _entityProperties.Properties.ThirstMax;
            _hunger = _entityProperties.Properties.HungerMax;
            _health = _entityProperties.Properties.MaxHealth;
            _birthticks = 0;
            _colliders = new Collider[20];
        }

        public void Death()
        {
            Destroy(gameObject);
        }

        public void DeSelect()
        {
            _isSelected = false;
        }

        public virtual void FixedUpdate()
        {
            if (_agent.hasPath)
            {
                transform.position = _agent.nextPosition;
            }
        }

        public virtual string GetData()
        {
            return $"State: {State}\nLookFor: {LookFor}\nAction: {Action}"
                + $"\nHealth: {_health.ToString("0.##")}\nThirst: {_thirst.ToString("0.##")}\nHunger: {_hunger.ToString("0.##")}\n";
        }

        public IdentitySO GetIdentity()
        {
            return _entityProperties.Properties.Identity;
        }

        public virtual string GetName()
        {
            return Name;
        }

        public void Select()
        {
            _isSelected = true;
        }

        public virtual void Start()
        {
            EvolutionManager.Instance.OnTick.AddListener(Tick);
        }

        public virtual void Update()
        {
            if (_health <= 0)
            {
                Death();
            }

            if (!Action.HasFlag(Action.Eat | Action.Drink) && (_hunger <= 0 || _thirst <= 0))
            {
                _health -= Time.deltaTime * 0.04f;
            }
            else if (_health <= _entityProperties.Properties.MaxHealth)
            {
                _health += Time.deltaTime * 0.01f;
                _health = Mathf.Clamp(_health, 0f, _entityProperties.Properties.MaxHealth);
            }

            if (!Action.HasFlag(Action.Eat) && _hunger > 0)
            {
                _hunger -= Time.deltaTime * _entityProperties.Properties.HungerDecrease;
                if (_hunger < 0)
                {
                    _hunger = 0;
                }
            }

            if (!Action.HasFlag(Action.Drink) && _thirst > 0)
            {
                _thirst -= Time.deltaTime * _entityProperties.Properties.ThirstDecrease;
                if (_thirst < 0)
                {
                    _thirst = 0;
                }
            }

            if (Action == Action.NONE && LookFor == LookFor.NONE)
            {
                if (_hunger <= 0)
                {
                    State = State.Wander;
                    LookFor = LookFor.Food;
                }
                else if (_thirst <= 0)
                {
                    State = State.Wander;
                    LookFor = LookFor.Water;
                }
            }

            StateLoop();
        }

        public float Damage(float damage)
        {
            if (_health >= damage)
            {
                _health -= damage;
                return damage;
            }
            else
            {
                _health -= damage;
                return damage - Math.Abs(_health);
            }
        }

        protected bool DestinationReached()
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

        protected virtual void DoAction()
        { }

        protected NavMeshPath GetPathToTarget(Vector3 target)
        {
            NavMeshPath path = new NavMeshPath();
            if (_agent.CalculatePath(target, path))
            {
                return path;
            }
            else
            {
                return null;
            }
        }

        protected virtual void Goto()
        {
            if (DestinationReached())
            {
                switch (LookFor)
                {
                    case LookFor.Food:
                        Action = Action.Eat;
                        break;

                    case LookFor.Water:
                        Action = Action.Drink;
                        break;
                }
                LookFor = LookFor.NONE;
                State = State.Do;
            }
        }

        protected virtual void LookForTarget()
        {
            try
            {
                Array.Clear(_colliders, 0, _colliders.Length);
                Physics.OverlapSphereNonAlloc(gameObject.transform.position, _entityProperties.Properties.InteractRange * 2, _colliders, _entityProperties.Properties.HungerLayers.value);

                GameObject target = null;

                switch (LookFor)
                {
                    case LookFor.Food:
                        {
                            var food = _colliders
                                .Where(x => x?.GetComponent<IIdentity>() != null && _entityProperties.Properties.Eats.Contains(x?.GetComponent<IIdentity>()?.GetIdentity()))
                                .OrderBy(x => Vector3.Distance(gameObject.transform.position, x.transform.position))
                                .Select(x => x.gameObject).ToList();
                            target = food.FirstOrDefault();
                        }
                        break;

                    case LookFor.Water:
                        {
                            var water = _colliders
                                .Where(x => x?.GetComponent<IIdentity>() != null && _entityProperties.Properties.Drinks.Contains(x?.GetComponent<IIdentity>()?.GetIdentity()))
                                .OrderBy(x => Vector3.Distance(gameObject.transform.position, x.transform.position))
                                .Select(x => x.gameObject).ToList();
                            target = water.FirstOrDefault();
                        }
                        break;
                }

                Array.Clear(_colliders, 0, _colliders.Length);

                if (target != null)
                {
                    State = State.GoTo;
                    var path = GetPathToTarget(target.transform.position);
                    if (path != null && _agent.SetPath(path))
                    {
                        _goal = target.transform.position;
                        _interactGoal = target;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        protected virtual void StateLoop()
        {
            switch (State)
            {
                case State.NONE:
                    break;

                case State.Wander:
                    Wander();
                    if (LookFor != LookFor.NONE)
                    {
                        LookForTarget();
                    }
                    break;

                case State.GoTo:
                    Goto();
                    break;

                case State.Do:
                    DoAction();
                    break;
            }
        }

        protected virtual void Tick()
        {
            _birthticks++;
            _actionticks++;

            if (_birthticks >= _entityProperties.Properties.BirthTicks)
            {
                _birthticks = 0;

                if (_isPregnant && (_hunger * _entityProperties.Properties.BirthHungerPercentageNeeded) <= _hunger)
                {
                    _isPregnant = false;
                    var rand = Random.value;
                    Debug.Log($"rand {rand} - {rand <= _entityProperties.Properties.BirthChance}");
                    if (rand <= _entityProperties.Properties.BirthChance)
                    {
                        _hunger -= (_hunger * _entityProperties.Properties.BirthHungerPercentageUsed);
                        EntityManager.Instance.SpawnEntityAtPosition(_entityProperties.WorldObject, gameObject.transform.position);
                    }
                }
            }
        }

        protected bool TryDoAction()
        {
            if (_actionticks >= _entityProperties.Properties.ActionTicks)
            {
                _actionticks = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void Wander()
        {
            if (TryDoAction() && (_goal == Vector3.zero || DestinationReached()))
            {
                Vector3 direction = Random.insideUnitSphere * Random.Range(_entityProperties.Properties.MinWalkable, _entityProperties.Properties.MaxWalkable);
                direction += gameObject.transform.position;

                NavMeshHit navMeshHit;
                NavMesh.SamplePosition(direction, out navMeshHit, _entityProperties.Properties.MaxWalkable, 1);
                Vector3 goalOriginal = navMeshHit.position;
                Vector3 pathDir = transform.position - goalOriginal;
                var goal = goalOriginal + (pathDir.normalized * (_agent.radius));

                var path = GetPathToTarget(goal);

                if (path != null && _agent.SetPath(path))
                {
                    _goal = goal;
                }
            }
        }
    }
}