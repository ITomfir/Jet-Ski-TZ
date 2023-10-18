using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _rotateDecayRate;
    [SerializeField] private AnimationCurve _rotateProgression;
    [SerializeField] private float _maxRotateSpeed = 70f;


    [Header("Visual")]
    [SerializeField] private Transform _model;

    [SerializeField] private float _yawSpeed;
    [SerializeField] private float _yawDecayRate;
    [SerializeField] private AnimationCurve _yawProgression;
    [SerializeField] private float _maxYawAngle = 70f;

    [SerializeField] private float _pitchSpeed;
    [SerializeField] private float _pitchDecayRate;
    [SerializeField] private AnimationCurve _pitchProgression;
    [SerializeField] private float _maxPitchAngle = 30f;

    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollDecayRate;
    [SerializeField] private AnimationCurve _rollProgression;
    [SerializeField] private float _maxRollAngle = 30f;

    private float _rotateProgress;

    private float _pitchProgress;
    private float _yawProgress;
    private float _rollProgress;

    private Rigidbody _rigidbody;

    private Vector2 _direction;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate () {
        var directionX = RoundToNormalizedOrZero(_direction.x);
        var directionZ = RoundToNormalizedOrZero(_direction.y);

        Move(directionZ);
        Rotate(directionX);
    }

    private void Move (float direction) {
        Vector3 velocityOffset = Vector3.zero;
        velocityOffset += Vector3.forward * direction * _moveSpeed;
        velocityOffset += Vector3.right * _maxRotateSpeed * GetMultiplierByCurve(_rotateProgression, _rotateProgress);
        
        _rigidbody.AddForce(velocityOffset);
    }

    private void Rotate (float direction) {
        var rotationAnglePitch = -_maxPitchAngle * GetMultiplierByCurve(_pitchProgression, _pitchProgress);
        var rotationAngleYaw = _maxYawAngle * GetMultiplierByCurve(_yawProgression, _yawProgress);
        var rotationAngleRoll = -_maxRollAngle * GetMultiplierByCurve(_rollProgression, _rollProgress);

        _rotateProgress += _rotateSpeed * direction * Time.deltaTime;

        _pitchProgress += _pitchSpeed * Mathf.Abs(direction) * Time.deltaTime;
        _yawProgress += _yawSpeed * direction * Time.deltaTime;
        _rollProgress += _rollSpeed * direction * Time.deltaTime;

        _rotateProgress = Mathf.Clamp(_rotateProgress, -1, 1);

        _pitchProgress = Mathf.Clamp(_pitchProgress, 0, 1);
        _yawProgress = Mathf.Clamp(_yawProgress, -1, 1);
        _rollProgress = Mathf.Clamp(_rollProgress, -1, 1);

        if (direction == 0) {
            _rotateProgress = Mathf.Lerp(_rotateProgress, 0, _rotateDecayRate * Time.deltaTime);

            _pitchProgress = Mathf.Lerp(_pitchProgress, 0, _pitchDecayRate * Time.deltaTime);
            _yawProgress = Mathf.Lerp(_yawProgress, 0, _yawDecayRate * Time.deltaTime);
            _rollProgress = Mathf.Lerp(_rollProgress, 0, _rollDecayRate * Time.deltaTime);
        }
    
        _model.rotation = Quaternion.Euler(rotationAnglePitch, rotationAngleYaw, rotationAngleRoll);
    }

    public void SetDirection (Vector2 direction) {
        _direction = direction;
    }

    public void Stop () {
        _direction = Vector2.zero;
    }

    private int RoundToNormalizedOrZero (float value) {
        if (0 == value) return 0;

        return value > 0 ? 1 : -1;
    }

    private float GetMultiplierByCurve (AnimationCurve curve, float progress) {
        return curve.Evaluate(Math.Abs(progress)) * RoundToNormalizedOrZero(progress);
    }
}
