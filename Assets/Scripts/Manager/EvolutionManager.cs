using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Manager
{
    public class EvolutionManager : MonoBehaviour
    {
        public static EvolutionManager Instance;
        public LayerMask FoodMask;
        public LayerMask WaterMask;
        public LayerMask EntityMask;
        public Terrain Terrain;
        public LayerMask SpawnLayer;
        public UnityEvent OnTick;

        private TerrainData _terrainData;
        [ShowInInspector]
        private float _tickTimerMax = 0.2f;
        [ShowInInspector, ReadOnly]
        private int _tick;
        [ShowInInspector, ReadOnly]
        private float _tickTimer;

        private void Awake()
        {
            Instance = this;
            _tick = 0;
        }

        private void Update()
        {
            _tickTimer += Time.deltaTime;
            if (_tickTimer > _tickTimerMax) 
            {
                _tickTimer -= _tickTimerMax;
                _tick++;

                if (OnTick != null)
                {
                    OnTick.Invoke();
                }
            }
        }

        public void Start()
        {
            _terrainData = Terrain.terrainData;
        }

        public Vector3 GetRandomPointOnMesh()
        {
            float randomX = Random.Range(_terrainData.bounds.min.x, _terrainData.bounds.max.x);
            float randomZ = Random.Range(_terrainData.bounds.min.z, _terrainData.bounds.max.z);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomX, _terrainData.bounds.max.y + 5f, randomZ), -Vector3.up, out hit, 10f, SpawnLayer))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
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
