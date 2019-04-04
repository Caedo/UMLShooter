using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public LayerMask _enemyMask;
    public float _startSpeed;
    public float _damage;

    protected Rigidbody _body;

    void Awake() {
        _body = GetComponent<Rigidbody>();
    }

    protected void Start() {
        _body.velocity = transform.forward * _startSpeed;
    }

    public void OnReset() {
        Destroy(gameObject);
    }
}