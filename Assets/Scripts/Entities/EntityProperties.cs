using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public class EntityProperties
    {
        public float HungerMax;
        public float HungerDecrease;
        public float ThirstMax;
        public float ThirstDecrease;
        public float InteractRange;
        public float InteractTime;
        public float MaxWalkable;
        public float MinWalkable;
        public float MaxHealth;
        public string Name;
    }
}
