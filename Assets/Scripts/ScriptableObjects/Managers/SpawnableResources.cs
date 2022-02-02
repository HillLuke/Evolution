using Sirenix.OdinInspector;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Resources;
using Assets.Scripts.Utility;

namespace Assets.Scripts.ScriptableObjects.Managers
{
    [CreateAssetMenu(fileName = "SpawnableResources", menuName = "ScriptableObjects/WeightedList/Resources", order = 1)]
    public class SpawnableResources : ScriptableObject
    {
        public WeightedList<ResourceSO> SpawnableList;
    }
}
