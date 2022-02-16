using Assets.Scripts.Resources;
using Assets.Scripts.ScriptableObjects.Managers;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public int MaxSpawnableResources;
        public GameObject ResourceParent;
        public SpawnableResources SpawnableResources;
        private Resource[] _spawnedResources;

        private void Awake()
        {
            _spawnedResources = new Resource[MaxSpawnableResources];
        }

        private void Update()
        {
            for (int i = 0; i < MaxSpawnableResources; i++)
            {
                if (_spawnedResources[i] == null)
                {
                    var point = EvolutionManager.Instance.GetRandomPointOnMesh();
                    if (point != Vector3.zero)
                    {
                        _spawnedResources[i] = Instantiate(SpawnableResources.SpawnableList.GetRandom().WorldObject, EvolutionManager.Instance.GetRandomPointOnMesh(), Quaternion.identity, ResourceParent.transform);
                    }
                }
            }
        }
    }
}