using Assets.Scripts.Interfaces;
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
    public class BasicCarnivore : BaseEntity, ICarnivore
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();

            State = State.Wander;
        }

        protected override void DoAction()
        {
            throw new NotImplementedException();
        }

        protected override void Goto()
        {
            throw new NotImplementedException();
        }

        protected override void Wander()
        {
            throw new NotImplementedException();
        }

        protected override void StateLoop()
        {
            switch (State)
            {
                case State.NONE:
                    break;
                case State.Wander:
                    if (_goal == Vector3.zero || DestinationReached())
                    {
                        Vector3 direction = Random.insideUnitSphere * Random.Range(_entityProperties.Properties.MinWalkable, _entityProperties.Properties.MaxWalkable);
                        direction += gameObject.transform.position;

                        NavMeshHit navMeshHit;
                        NavMesh.SamplePosition(direction, out navMeshHit, _entityProperties.Properties.MaxWalkable, 1);
                        _goalOriginal = navMeshHit.position;
                        Vector3 pathDir = transform.position - _goalOriginal;
                        var goal = _goalOriginal + (pathDir.normalized * (_agent.radius));

                        if (_agent.CalculatePath(goal, _path))
                        {
                            _goal = goal;
                            _agent.SetDestination(_goal);
                        }
                        else
                        {
                            _goal = Vector3.zero;
                        }
                    }
                    break;
                case State.GoTo:
                    break;
                case State.Do:
                    break;
            }
        }

    }
}
