using Unity.AI.Navigation;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private NavMeshSurface meshSurface;

    [SerializeField] private int numOfSpawn;
    //[SerializeField] private Vector2 spawnPosition; //randomize spawning position
    [SerializeField] private Vector2 spawnSize;
    [SerializeField] private LayerMask FloorMask;

    [SerializeField] private Transform spawnTransform; // Use a GameObject's transform position
    [SerializeField] private float minSpawnDistance = 2.0f; // Minimum distance between obstacles
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private void Start()
    {
        Vector3 spawnScale = spawnTransform.localScale;
        int attempts = 0;
        int maxAttempts = numOfSpawn * 10; // Limit the number of tries

        for (int i = 0; i < numOfSpawn && attempts < maxAttempts; attempts++)
        {
            float xPos = Random.Range(-spawnScale.x / 2, spawnScale.x / 2) + spawnTransform.position.x;
            float zPos = Random.Range(-spawnScale.z / 2, spawnScale.z / 2) + spawnTransform.position.z;
            Vector3 newPos = new Vector3(xPos, spawnTransform.position.y, zPos);

            float xSize = Random.Range(spawnSize.x, spawnSize.y);
            float zSize = Random.Range(spawnSize.x, spawnSize.y);

            Vector3 newSize = new Vector3(xSize, 1, zSize);

            Ray ray = new Ray(newPos, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, FloorMask))
            {
                Vector3 spawnPoint = new Vector3(hit.point.x, spawnTransform.position.y, hit.point.z);

                if (!IsTooClose(spawnPoint, newSize))
                {
                    GameObject newObstacle = Instantiate(obstaclePrefab, spawnPoint, Quaternion.identity);
                    newObstacle.transform.localScale = newSize;
                    spawnedPositions.Add(spawnPoint);
                    i++;
                }
            }
        }
        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Obstacle spawning stopped due to too many failed attempts.");
        }

        meshSurface.BuildNavMesh();
    }

    //float SphereRadius(float xSize, float zSize)
    //{
    //    return Mathf.Max(xSize, zSize);
    //}
    private bool IsTooClose(Vector3 position, Vector3 size)
    {
        foreach (Vector3 spawnedPos in spawnedPositions)
        {
            if (Vector3.Distance(spawnedPos, position) < minSpawnDistance + Mathf.Max(size.x, size.z))
            {
                return true; // Too close to another obstacle
            }
        }
        return false; // Safe to spawn
    }

}
