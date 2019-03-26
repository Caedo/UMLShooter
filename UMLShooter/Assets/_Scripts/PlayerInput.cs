using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    Transform _cameraTransform;

    PlayerMovement _movement;

    private void Awake() {
        _cameraTransform = Camera.main.transform;
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        var forward = _cameraTransform.forward;
        forward.y = 0;

        var right = _cameraTransform.right;

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var velocity = forward.normalized * v + right.normalized * h;
        _movement.Move(velocity);
    }

}