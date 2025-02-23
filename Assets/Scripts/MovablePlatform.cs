using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private bool movingToB = true;

    void Update()
    {
        if (movingToB)
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            movingToB = false;
        if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
            movingToB = true;
    }
}
