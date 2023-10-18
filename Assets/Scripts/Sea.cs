using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Sea : MonoBehaviour {
    public static Sea instance;

    [SerializeField] private float _amplitude;
    [SerializeField] private float _lenght;
    [SerializeField] private float _speed;
    [SerializeField] private float _offset;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
            Debug.LogAssertion("There is more than one component sea on stage");
        }
    }

    private void Update () {
        _offset += _speed * Time.deltaTime;
    }

    public float GetWaveHeight (Vector3 position) {
        return _amplitude * Mathf.Sin(position.z / _lenght + _offset);
    }
}
