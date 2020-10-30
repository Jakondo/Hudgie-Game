using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;

    private Vector3 velocity;

    private void Awake()
    {
        transform.position = Target.position;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref velocity, 0.18f, 40);
    }
}
