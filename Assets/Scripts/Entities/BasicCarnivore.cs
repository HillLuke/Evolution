using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class BasicCarnivore : BaseEntity, ICarnivore
    {
        protected override void DoAction()
        {
            if (_interactGoal != null && TryDoAction())
            {
                switch (Action)
                {
                    case Action.Eat:
                        {
                            var prey = _interactGoal.GetComponent<IEntity>();

                            if (prey != null)
                            {
                                _hunger += prey.Damage(5f);
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