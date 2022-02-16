using Assets.Scripts.ScriptableObjects.Entities;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Managers
{
    [CreateAssetMenu(fileName = "SpawnableEntities", menuName = "ScriptableObjects/WeightedList/EntityProperties", order = 1)]
    public class SpawnableEntities : ScriptableObject
    {
        public WeightedList<EntityPropertiesSO> SpawnableList;
    }
}