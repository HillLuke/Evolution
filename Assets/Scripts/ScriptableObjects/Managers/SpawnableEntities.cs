using Sirenix.OdinInspector;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Resources;
using Assets.Scripts.Utility;
using Assets.Scripts.ScriptableObjects.Entities;

namespace Assets.Scripts.ScriptableObjects.Managers
{
    [CreateAssetMenu(fileName = "SpawnableEntities", menuName = "ScriptableObjects/WeightedList/EntityProperties", order = 1)]
    public class SpawnableEntities : ScriptableObject
    {
        public WeightedList<EntityPropertiesSO> SpawnableList;
    }
}
