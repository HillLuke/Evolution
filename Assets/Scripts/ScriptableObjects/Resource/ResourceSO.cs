using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Resource
{
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
    public class ResourceSO : ScriptableObject
    {
        public EResourceType ResourceType;
        public float MaxValue;
        public Resource WorldObject;
    }

    public enum EResourceType
    {
        Food,
        Water
    }
}
