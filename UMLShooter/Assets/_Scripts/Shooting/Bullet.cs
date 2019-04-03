using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float _startSpeed;
    public float _damage;

    protected Rigidbody _body;

    void Awake() {
        _body = GetComponent<Rigidbody>();
    }
}