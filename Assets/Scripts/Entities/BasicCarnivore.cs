using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Entities
{
    public class BasicCarnivore : BaseEntity, ICarnivore
    {
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