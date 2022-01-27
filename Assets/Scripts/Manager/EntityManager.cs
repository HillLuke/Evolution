using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class EntityManager : MonoBehaviour
    {
        public List<Entity> SpawnableEntities;
        public int MaxSpawnableEntities;
        public Entity[] SpawnedEntities;
        public GameObject EntityParent;

        private void Awake()
        {
            SpawnedEntities = new Entity[MaxSpawnableEntities];
        }

        private void Update()
        {
            for (int i = 0; i < MaxSpawnableEntities; i++)
            {
                if (SpawnedEntities[i] == null)
                {
                    var point = EvolutionManager.Instance.GetRandomPointOnMesh();
                    if (point != Vector3.zero)
                    {
                        SpawnedEntities[i] = Instantiate(SpawnableEntities[0], EvolutionManager.Instance.GetRandomPointOnMesh(), Quaternion.identity, EntityParent.transform);
                    }
                }
            }
        }
    }
}