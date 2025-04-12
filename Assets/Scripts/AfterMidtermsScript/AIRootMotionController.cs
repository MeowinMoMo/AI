using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIRootMotionController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxtarget;
    [SerializeField] private bool isJumping = false;
    private bool wasOnOffMeshLink = false;

    private void OnValidate()
    {
        if(!agent) agent = GetComponent<NavMeshAgent>();
        if(!animator) animator = GetComponent<Animator>();
    }

    void OnLinkStarted()
    {
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    void OnLinkStopped() 
    {
        isJumping = false;
        animator.SetBool("isJumping", false); 
    }

    private void Update()
    {
        if (agent.hasPath)
        {
            var dir = (agent.steeringTarget - transform.position).normalized;
            var aniDir = transform.InverseTransformDirection(dir);
            var isFacingMoveDirection = Vector3.Dot(dir, transform.forward) > 0.5f;

            animator.SetFloat("Horizontal",isFacingMoveDirection ? aniDir.x : 0, 0.5f, Time.deltaTime);
            animator.SetFloat("Vertical", isFacingMoveDirection ? aniDir.z : 0, 0.5f, Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 100 * Time.deltaTime);

            if (Vector3.Distance(transform.position, agent.destination) < agent.radius)
            {
                agent.ResetPath();
            }
        }
        else
        {
            animator.SetFloat("Horizontal", 0, 0.25f, Time.deltaTime);
            animator.SetFloat("Vertical", 0, 0.25f, Time.deltaTime);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out RaycastHit hit, maxtarget);
            if (isHit)
            {
                agent.destination = hit.point;
            }
        }

        // Jump start detection
        if (agent.isOnOffMeshLink && !wasOnOffMeshLink)
        {
            OnLinkStarted();
            StartCoroutine(HandleJump(agent.currentOffMeshLinkData));
        }

        // Jump end detection
        if (!agent.isOnOffMeshLink && wasOnOffMeshLink)
        {
            OnLinkStopped();
        }

        wasOnOffMeshLink = agent.isOnOffMeshLink;
    }

    private IEnumerator HandleJump(OffMeshLinkData linkData)
    {
        var startPos = agent.transform.position;
        var endPos = linkData.endPos + Vector3.up * agent.baseOffset;

        float jumpDuration = 0.5f; // Duration of the jump animation
        float time = 0f;

        agent.isStopped = true; // Stop NavMeshAgent from moving during animation

        while (time < jumpDuration)
        {
            float t = time / jumpDuration;
            agent.transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * Mathf.Sin(t * Mathf.PI); // arc
            time += Time.deltaTime;
            yield return null;
        }

        agent.CompleteOffMeshLink(); // Let the agent know we're done with the jump
        agent.isStopped = false;
    }
    private void OnDrawGizmos()
    {
        if (agent.hasPath)
        {
            for (int i = 0; i < agent.path.corners.Length -1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.blue);
            }
        }
    }
}
