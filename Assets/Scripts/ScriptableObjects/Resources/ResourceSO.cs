using Assets.Scripts.ScriptableObjects.Identity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Resources
{
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
    public class ResourceSO : ScriptableObject
    {
        public EResourceType ResourceType;
        public float MaxValue;
        [Required]
        public Assets.Scripts.Resources.Resource WorldObject;
        public IdentitySO Identity;
    }

    public enum EResourceType
    {
        Food,
        Water
    }
}
