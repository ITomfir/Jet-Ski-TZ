using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private PlayerMovement _playerMovement;

    private PlayerInput _input;

    private void Awake() {
        _input = new PlayerInput ();
    }

    private void OnEnable() {
        _input.OnMove += Move;
        _input.OnStop += Stop;
    }

    private void OnDisable() {
        _input.OnMove -= Move;
        _input.OnStop -= Stop;
    }

    private void Move (Vector2 direction) {
        _playerMovement.SetDirection(direction);
    }

    private void Stop () {
        _playerMovement.Stop();
    }
}
