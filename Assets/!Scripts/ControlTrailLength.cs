using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTrailLength : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    TrailRenderer trailRenderer;
    private void OnDisable()
    {
        trailRenderer.time = 0f;
    }
    private void Update()
    {
        trailRenderer.time = Mathf.Abs(1f / rb.velocity.magnitude) * 2;
    }
}
