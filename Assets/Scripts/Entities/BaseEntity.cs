using Assets.Scripts.Interfaces;
using Assets.Scripts.Manager;
using Assets.Scripts.ScriptableObjects.Entities;
using Assets.Scripts.ScriptableObjects.Identity;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public abstract class BaseEntity : MonoBehaviour, IEntity, IMonitorable, IIdentity
    {
        public string Name => _entityProperties.Properties.Name;

        [ShowInInspector, ReadOnly]
        public State State { get; protected set; }
        [ShowInInspector, ReadOnly]
        public LookFor LookFor { get; protected set; }
        [ShowInInspector, ReadOnly]
        public Action Action { get; protected set; }

        [Title("Private Serialized")]
        [SerializeField]
        protected EntityPropertiesSO _entityProperties;

        [Title("Private Read Only")]
        [ShowInInspector, ReadOnly]
        protected bool _isSelected;
        [ShowInInspector, ReadOnly]
        protected float _hunger;
        [ShowInInspector, ReadOnly]
        protected float _health;
        [ShowInInspector, ReadOnly]
        protected float _thirst;
        [ShowInInspector, ReadOnly]
        protected GameObject _interactGoal;
        [ShowInInspector, ReadOnly]
        protected Vector3 _goal;
        [ShowInInspector, ReadOnly]
        protected Vector3 _goalOriginal;
        [ShowInInspector, ReadOnly]
        protected NavMeshAgent _agent;
        [ShowInInspector, ReadOnly]
        protected NavMeshPath _path;
        [ShowInInspector, ReadOnly]
        protected int _birthticks;
        [ShowInInspector, ReadOnly]
        protected int _actionticks;
        [ShowInInspector, ReadOnly]
        protected Collider[] _colliders;

        protected virtual void Tick() 
        { 
            _birthticks++;
            _actionticks++;
        }

        protected virtual void Goto() { }

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
        protected virtual void LookForTarget()
        {
            try
            {
                Array.Clear(_colliders, 0, _colliders.Length);
                Physics.OverlapSphereNonAlloc(gameObject.transform.position, _entityProperties.Properties.InteractRange * 4, _colliders);

                GameObject target = null;

                switch (LookFor)
                {
                    case LookFor.Food:
                        {
                            var food = _colliders.Where(x => x?.GetComponent<IIdentity>() != null && _entityProperties.Properties.Eats.Contains(x?.GetComponent<IIdentity>()?.GetIdentity())).Select(x => x.gameObject).ToList();
                            target = food.FirstOrDefault();
                        }
                        break;
                    case LookFor.Water:
                        {
                            var water = _colliders.Where(x => x?.GetComponent<IIdentity>() != null && _entityProperties.Properties.Drinks.Contains(x?.GetComponent<IIdentity>()?.GetIdentity())).Select(x => x.gameObject).ToList();
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

        protected virtual void DoAction() { }
        protected virtual void StateLoop() { }

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
            _colliders = new Collider[4];
        }


        public virtual void Start()
        {
            EvolutionManager.Instance.OnTick.AddListener(Tick);
        }

        public virtual void Update()
        {
            if (!Action.HasFlag(Action.Eat | Action.Drink) && (_hunger <= 0 || _thirst <= 0))
            {
                _health -= Time.deltaTime * 0.04f;
                if (_health < 0)
                {
                    _health = 0;
                    Death();
                }
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

        public virtual string GetData()
        {
            return $"State: {State}\nLookFor: {LookFor}\nAction: {Action}"
                + $"\nHealth: {_health.ToString("0.##")}\nThirst: {_thirst.ToString("0.##")}\nHunger: {_hunger.ToString("0.##")}\n";
        }

        public virtual string GetName()
        {
            return Name;
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void DeSelect()
        {
            _isSelected = false;
        }

        public void Death()
        {
            Destroy(gameObject);
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


        #region IIdentity

        public IdentitySO GetIdentity()
        {
            return _entityProperties.Properties.Identity;
        }

        #endregion

    }
}
