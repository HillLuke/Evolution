using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class EntityManager : MonoBehaviour
    {
        public static EntityManager Instance;
        public GameObject EntityParent;
        public int MaxSpawnableEntities;
        public SpawnableEntities SpawnableEntities;
        private List<BaseEntity> _spawnedEntities;

        public void SpawnEntityAtPosition(BaseEntity entity, Vector3 position)
        {
            _spawnedEntities.Add(
                Instantiate(
                    entity,
                    position,
                    Quaternion.identity,
                    EntityParent.transform
                    )
                );
        }

        private void Awake()
        {
            _spawnedEntities = new List<BaseEntity>();
            Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < MaxSpawnableEntities; i++)
            {
                var point = EvolutionManager.Instance.GetRandomPointOnMesh();
                if (point != Vector3.zero)
                {
                    SpawnEntityAtPosition(
                        SpawnableEntities.SpawnableList.GetRandom().WorldObject,
                        EvolutionManager.Instance.GetRandomPointOnMesh()
                        );
                }
            }
        }
    }
}