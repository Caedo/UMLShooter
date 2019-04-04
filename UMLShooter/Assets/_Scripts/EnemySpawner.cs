using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public EnemyEntity _enemy;
    public Transform[] _spawnPoints;
    public float _timeBetweenSpawns;
    public int _enemyCount;
    public GameEvent _gameEnded;

    void Start() {
        StartCoroutine("SpawnCourutine");
    }

    IEnumerator SpawnCourutine() {
        for (int i = 0; i < _enemyCount; i++) {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            var randIdx = Random.Range(0, _spawnPoints.Length);

            var pos = _spawnPoints[randIdx].position;
            Instantiate(_enemy, pos, Quaternion.identity);
        }

        _gameEnded.Raise();
    }

    public void OnReset() {
        StopCoroutine("SpawnCourutine");
        StartCoroutine("SpawnCourutine");

    }

}