using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour {

    public float _moveSpeed;
    public float _attackTreshold;
    PlayerEntity _player;

    void Awake() {
        _player = GameObject.FindObjectOfType<PlayerEntity>();
    }

    void Update() {
        var dir = transform.position - _player.transform.position;
        transform.Translate(dir.normalized * _moveSpeed * Time.deltaTime);

        if (dir.sqrMagnitude < _attackTreshold) {
            _player.TakeDamage(15f);
            Destroy(gameObject);
        }
    }
}