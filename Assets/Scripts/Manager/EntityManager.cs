using Assets.Scripts.Entities;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Managers;
using Assets.Scripts.ScriptableObjects.Entities;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class EntityManager : MonoBehaviour
    {
        public static EntityManager Instance;
        public SpawnableEntities SpawnableEntities;
        public int MaxSpawnableEntities;
        public GameObject EntityParent;

        private List<BaseEntity> _spawnedEntities;

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
    }
}