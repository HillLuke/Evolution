using Assets.Scripts.Interfaces;
using Assets.Scripts.ScriptableObjects.Entities;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public abstract class BaseEntity : MonoBehaviour, IEntity, IMonitorable
    {
        public string Name => _entityProperties.Properties.Name;
        public State State { get; protected set; }
        public LookFor LookFor { get; protected set; }
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
        protected float _interactTimer;
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

        protected abstract void Goto();
        protected abstract void Wander();
        protected abstract void DoAction();
        protected abstract void StateLoop();

        public virtual void Awake()
        {
            State = State.NONE;
            LookFor = LookFor.NONE;
            Action = Action.NONE;
            _path = new NavMeshPath();
            _agent = GetComponent<NavMeshAgent>();
            _isSelected = false;
            _thirst = _entityProperties.Properties.ThirstMax;
            _hunger = _entityProperties.Properties.HungerMax;
            _health = _entityProperties.Properties.MaxHealth;
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            if (!Action.HasFlag(Action.Eat | Action.Drink) && (_hunger <= 0 || _thirst <= 0))
            {
                _health -= Time.deltaTime * 0.01f;
                if (_health < 0)
                {
                    _health = 0;
                }
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

            StateLoop();
        }

        public virtual string GetData()
        {
            return $"Name: {Name}\nState: {State}\nLookFor: {LookFor}\nAction: {Action}"
                + $"\nHealth: {_health.ToString("#.##")}\nThirst: {_thirst.ToString("#.##")}\nHunger: {_hunger.ToString("#.##")}\n";
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void DeSelect()
        {
            _isSelected = false;
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
    }
}
