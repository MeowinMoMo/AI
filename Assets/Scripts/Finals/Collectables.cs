using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private float collectDistance = 2f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= collectDistance)
        {
            Collect();
        }
    }

    void Collect()
    {
        Debug.Log("Item collected!");
        // Add your custom logic here, like increasing score or inventory
        Destroy(gameObject);
    }
}
