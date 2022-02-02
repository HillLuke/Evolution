using Assets.Scripts.Entities;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Entities
{
    [CreateAssetMenu(fileName = "EntityProperties", menuName = "ScriptableObjects/EntityProperties", order = 1)]
    public class EntityPropertiesSO : ScriptableObject
    {
        public EntityProperties Properties;
        public BaseEntity WorldObject;

        private void Reset()
        {
            Properties = new EntityProperties()
            {
                HungerMax = 10f,
                HungerDecrease = 0.02f,
                ThirstMax = 10f,
                ThirstDecrease = 0.01f,
                InteractRange = 1.5f,
                InteractTime = 1f,
                MaxWalkable = 10f,
                MinWalkable = 5f,
                MaxHealth = 10f
            };
        }
    }
}

