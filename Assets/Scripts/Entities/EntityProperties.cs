﻿using Assets.Scripts.ScriptableObjects.Identity;
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
        public IdentitySO Identity;
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
        public int ActionTicks;

        public List<IdentitySO> Eats;
        public List<IdentitySO> Drinks;

        public int BirthTicks;
        public int BirthChance;
        public float BirthHungerPercentageNeeded;
        public float BirthHungerPercentageUsed;
    }
}
