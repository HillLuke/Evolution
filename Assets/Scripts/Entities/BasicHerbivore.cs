using Assets.Scripts.Interfaces;
using Assets.Scripts.Manager;
using Assets.Scripts.Resources;
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
    public class BasicHerbivore : BaseEntity, IHerbivore
    {
        private Collider[] _colliders;

        public override void Awake()
        {
            base.Awake();

            _colliders = new Collider[4];
        }

        protected override void StateLoop()
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

        protected override void Goto()
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

        protected override void DoAction()
        {
            if (_interactGoal != null)
            {
                if (_interactTimer <= 0)
                {
                    switch (Action)
                    {
                        case Action.Eat:
                            { 
                                var resource = _interactGoal.GetComponent<Resource>();

                                if (resource != null)
                                {
                                    _hunger += _interactGoal.GetComponent<Resource>().Use(1f);
                                    _hunger = Mathf.Clamp(_hunger, 0f, _entityProperties.Properties.HungerMax);
                                    if (_hunger == _entityProperties.Properties.HungerMax)
                                    {
                                        _interactGoal = null;
                                        _goal = Vector3.zero;
                                        Action = Action.NONE;
                                        LookFor = LookFor.NONE;
                                        State = State.Wander;
                                    }
                                }
                                else
                                {
                                    Action = Action.NONE;
                                    LookFor = LookFor.NONE;
                                    State = State.Wander;
                                }
                            }
                            break;
                        case Action.Drink:
                            {
                                var resource = _interactGoal.GetComponent<Resource>();
                                if (resource != null)
                                {
                                    _thirst += _interactGoal.GetComponent<Resource>().Use(1f);
                                    _thirst = Mathf.Clamp(_thirst, 0f, _entityProperties.Properties.ThirstMax);
                                    if (_hunger == _entityProperties.Properties.ThirstMax)
                                    {
                                        _interactGoal = null;
                                        _goal = Vector3.zero;
                                        Action = Action.NONE;
                                        LookFor = LookFor.NONE;
                                        State = State.Wander;
                                    }
                                }
                                else
                                {
                                    Action = Action.NONE;
                                    LookFor = LookFor.NONE;
                                    State = State.Wander;
                                }
                            }
                            break;
                    }
                    _interactTimer = _entityProperties.Properties.InteractTime;
                }
                else
                {
                    _interactTimer -= Time.deltaTime;
                }
            }
            else
            {
                Action = Action.NONE;
                LookFor = LookFor.NONE;
                State = State.Wander;
            }
        }

        protected override void LookForTarget()
        {
            try
            {
                var found = 0;
                switch (LookFor)
                {
                    case LookFor.Food:
                        found = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _entityProperties.Properties.InteractRange * 4, _colliders, EvolutionManager.Instance.FoodMask.value);
                        break;
                    case LookFor.Water:
                        found = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _entityProperties.Properties.InteractRange * 4, _colliders, EvolutionManager.Instance.WaterMask.value);
                        break;
                }

                if (_colliders != null && found > 0)
                {
                    State = State.GoTo;
                    var path = GetPathToTarget(_colliders[0].gameObject.transform.position);
                    if (path != null && _agent.SetPath(path))
                    {
                        _goal = _colliders[0].gameObject.transform.position;
                        _interactGoal = _colliders[0].gameObject;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

    }
}