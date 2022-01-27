using UnityEngine;

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

        private TerrainData _terrainData;

        public void Start()
        {
            _terrainData = Terrain.terrainData;
            Instance = this;
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
    }
}
