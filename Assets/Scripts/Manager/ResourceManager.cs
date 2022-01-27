using Assets.Scripts.Utility;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{

    public class ResourceManager : MonoBehaviour
    {
        public List<ResourceSO> SpawnableResources;
        [OdinSerialize]
        public WeightedList<ResourceSO> WeightedList;
        public int MaxSpawnableResources;
        public GameObject ResourceParent;

        private Resource[] _spawnedResources;

        private void Awake()
        {
            _spawnedResources = new Resource[MaxSpawnableResources];
        }
        private void Start()
        {
            WeightedList.Init();
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
                        _spawnedResources[i] = Instantiate(SpawnableResources[0].WorldObject, EvolutionManager.Instance.GetRandomPointOnMesh(), Quaternion.identity, ResourceParent.transform);

                        //_spawnedResources[i] = Instantiate(WeightedList.GetRandom().WorldObject, EvolutionManager.Instance.GetRandomPointOnMesh(), Quaternion.identity, ResourceParent.transform);
                    }
                }
            }
        }

        //public Vector3 GetRandomNavmeshPoint()
        //{
        //    Vector3 pos = Vector3.zero;
        //    var triangulation = NavMesh.CalculateTriangulation();
        //    int vertexIndex = Random.Range(0, triangulation.vertices.Length);

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, 1))
        //    {
        //        pos = hit.position;
        //    }

        //    return pos;
        //}

    }
}
