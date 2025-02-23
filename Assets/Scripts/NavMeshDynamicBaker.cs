using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshDynamicBaker : MonoBehaviour
{
    [SerializeField] public NavMeshSurface surface;

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        InvokeRepeating("BakeNavMesh", 0f, 1f); 
    }

    void BakeNavMesh()
    {
        surface.BuildNavMesh();
    }
}
