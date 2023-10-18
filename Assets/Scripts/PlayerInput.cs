using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : IDisposable {
    public event Action<Vector2> OnMove;
    public event Action OnStop;

    private PlayerInputAsset _inputAsset;

    public PlayerInput () {
        _inputAsset = new PlayerInputAsset ();
        _inputAsset.Enable();

        _inputAsset.Player.Move.performed += Move;
        _inputAsset.Player.Move.canceled += EndMove;
    }

    private void Move (InputAction.CallbackContext context) {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void EndMove (InputAction.CallbackContext context) {
        OnStop?.Invoke();
    }

    public void Dispose () {
        _inputAsset.Player.Move.performed -= Move;
        _inputAsset.Player.Move.canceled -= EndMove;
    }
}
