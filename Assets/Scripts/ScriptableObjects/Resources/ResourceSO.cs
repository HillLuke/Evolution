using Assets.Scripts.ScriptableObjects.Identity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Resources
{
    public enum EResourceType
    {
        Food,
        Water
    }

    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
    public class ResourceSO : ScriptableObject
    {
        public IdentitySO Identity;
        public float MaxValue;
        public EResourceType ResourceType;

        [Required]
        public Assets.Scripts.Resources.Resource WorldObject;
    }
}