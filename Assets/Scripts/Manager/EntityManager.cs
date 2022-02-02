using Assets.Scripts.Entities;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Managers;
using Assets.Scripts.ScriptableObjects.Entities;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class EntityManager : MonoBehaviour
    {
        public SpawnableEntities SpawnableEntities;
        public int MaxSpawnableEntities;
        public GameObject EntityParent;

        private BaseEntity[] _spawnedEntities;

        private void Awake()
        {
            _spawnedEntities = new BaseEntity[MaxSpawnableEntities];
        }

        private void Update()
        {
            for (int i = 0; i < MaxSpawnableEntities; i++)
            {
                if (_spawnedEntities[i] == null)
                {
                    var point = EvolutionManager.Instance.GetRandomPointOnMesh();
                    if (point != Vector3.zero)
                    {
                        _spawnedEntities[i] = Instantiate(SpawnableEntities.SpawnableList.GetRandom().WorldObject, EvolutionManager.Instance.GetRandomPointOnMesh(), Quaternion.identity, EntityParent.transform);
                    }
                }
            }
        }
    }
}