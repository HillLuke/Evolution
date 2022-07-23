using Assets.Scripts.ScriptableObjects.Identity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public class EntityProperties
    {
        public int ActionTicks;

        [Range(0, 1)]
        public float BirthChance;

        [Range(0, 1)]
        public float BirthHungerPercentageNeeded;

        [Range(0, 1)]
        public float BirthHungerPercentageUsed;

        public int BirthTicks;
        public List<IdentitySO> Drinks;
        public List<IdentitySO> Eats;
        public float HungerDecrease;
        public float HungerMax;
        public IdentitySO Identity;
        public float InteractRange;
        public float InteractTime;
        public float MaxHealth;
        public float MaxWalkable;
        public float MinWalkable;
        public string Name;
        public float ThirstDecrease;
        public float ThirstMax;
        public LayerMask HungerLayers;
    }
}