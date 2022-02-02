using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public enum State
    {
        NONE,
        Wander,
        GoTo,
        Do
    }

    public enum LookFor
    {
        NONE,
        Food,
        Water,
        Prey,
        Mate
    }

    public enum Action
    {
        NONE,
        Eat,
        Drink,
        Mate
    }
}
