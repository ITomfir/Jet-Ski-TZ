using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class FloatingObject : MonoBehaviour {
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _centerBodyYOffset;
    [SerializeField] private float depthBeforeSubmerged = 1f;
    [SerializeField] private float displacementAmount = 3f;
    [SerializeField] private int floaterCount = 1;

    private void FixedUpdate() {
        float centerBodyY = transform.position.y + _centerBodyYOffset;

        float waveHeight = Sea.instance.GetWaveHeight(transform.position);
        _rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        if (centerBodyY < waveHeight) {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - centerBodyY) / depthBeforeSubmerged) * displacementAmount;

            _rigidbody.AddForceAtPosition(Vector3.up * Mathf.Abs(Physics.gravity.y) * displacementMultiplier, transform.position, ForceMode.Force);
        }
    }
}
