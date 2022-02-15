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
        protected override void Tick()
        {
            base.Tick();

            if (_birthticks >= _entityProperties.Properties.BirthTicks)
            {
                _birthticks = 0;

                if ((_hunger * _entityProperties.Properties.BirthHungerPercentageNeeded) <= _hunger)
                {
                    if (Random.Range(1, 100) <= _entityProperties.Properties.BirthChance)
                    {
                        _hunger -= (_hunger * _entityProperties.Properties.BirthHungerPercentageUsed);
                        EntityManager.Instance.SpawnEntityAtPosition(_entityProperties.WorldObject, gameObject.transform.position);
                    }
                }
            }
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
            if (_interactGoal != null && TryDoAction())
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
            }
            else if (_interactGoal == null)
            {
                Action = Action.NONE;
                LookFor = LookFor.NONE;
                State = State.Wander;
            }
        }

    }
}