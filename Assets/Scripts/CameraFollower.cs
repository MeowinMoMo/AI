using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float moveSmoothness;
    public float rotSmoothness;

    public Vector3 moveOffset;
    public Vector3 rotOffset;

    public Transform Target;

    void FixedUpdate()
    {
        FollowTarget();
    }
    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 targetPos = new Vector3();
        targetPos = Target.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        var direction = Target.position - transform.position;
        var rota = new Quaternion();

        rota = Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rota, rotSmoothness * Time.deltaTime);
    }
}
