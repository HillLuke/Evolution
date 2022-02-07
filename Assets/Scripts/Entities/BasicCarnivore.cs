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

        protected override void StateLoop()
        {
            switch (State)
            {
                case State.NONE:
                    break;
                case State.Wander:
                    Wander();
                    break;
                case State.GoTo:
                    break;
                case State.Do:
                    break;
            }
        }

    }
}
