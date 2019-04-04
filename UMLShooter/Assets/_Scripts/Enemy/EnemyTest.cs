using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour {

    public float _moveSpeed;
    public float _attackTreshold;
    public float _damage = 26f;
    PlayerEntity _player;

    void Awake() {
        _player = GameObject.FindObjectOfType<PlayerEntity>();
    }

    void Update() {
        if (_player == null)
            return;

        var dir = _player.transform.position - transform.position;
        transform.Translate(dir.normalized * _moveSpeed * Time.deltaTime);

        if (dir.sqrMagnitude < _attackTreshold) {
            _player.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

    public void OnReset() {
        Debug.Log("dupa");
        Destroy(gameObject);
    }
}