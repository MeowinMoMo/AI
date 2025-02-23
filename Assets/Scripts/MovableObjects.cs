using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    [SerializeField] private float speedForward;
    [SerializeField] public int positive = 20;
    [SerializeField] public int negative = -20;
    private bool switchDir;

    private void Update()
    {
        if (!switchDir)
        {
            transform.Translate(Vector3.right * speedForward * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speedForward * Time.deltaTime);
        }

        if (transform.position.x >= positive)
        {
            switchDir = true;
        }
        else if(transform.position.x <= negative)
        {
            switchDir = false;
        }
    }
}
