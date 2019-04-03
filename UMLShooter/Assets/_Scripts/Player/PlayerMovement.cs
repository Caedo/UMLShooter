using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    public float _Speed;
    Rigidbody _body;

    void Awake() {
        _body = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 velocity) {
        _body.MovePosition(_body.position + velocity * Time.deltaTime * _Speed);
    }

    public void LookTowards(Vector3 pos) {
        pos.y = transform.position.y;

        transform.LookAt(pos, Vector3.up);
    }

    public void ResetVelocity() {
        _body.velocity = Vector3.zero;
    }
}