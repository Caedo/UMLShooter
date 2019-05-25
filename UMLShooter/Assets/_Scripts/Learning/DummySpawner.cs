using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour {
    public ShootDummy _dummyPrefab;
    public ShooterAgent _shooterAgent;
    public WalkingAgent _walkingAgent;
    public Vector2 _spawnArea;
    public int _dummySpawnCount;
    public int _maxDummiesCount;

    // Need reference to spawned dummies
    public List<ShootDummy> _spawnedDummies = new List<ShootDummy>();

    int _dummiesToSpawn;

    private void Awake() {
        ShootDummy.OnDummyKill += DummyKilled;
        _shooterAgent.OnAgentReset += ResetSpawner;
        _walkingAgent.OnAgentReset += ResetSpawner;
    }

    public void ResetSpawner() {
        _dummiesToSpawn = _maxDummiesCount;

        for (int i = 0; i < _spawnedDummies.Count; i++) {
            DestroyImmediate(_spawnedDummies[i].gameObject);
        }

        _spawnedDummies.Clear();

        for (int i = 0; i < _dummySpawnCount; i++) {
            SpawnOneDummy();
        }
    }

    public void SpawnOneDummy() {
        if (_dummiesToSpawn <= 0)
            return;

        var pos = RandomNewPosition();
        var dummy = Instantiate(_dummyPrefab, pos, Quaternion.identity, transform);
        dummy._spawner = this;

        _spawnedDummies.Add(dummy);

        _dummiesToSpawn--;
    }

    void DummyKilled(ShootDummy dummy) {
        if (dummy._spawner != this)
            return;

        _shooterAgent.KilledEnemy();
        _spawnedDummies.Remove(dummy);

        if (_dummiesToSpawn == 0 && _spawnedDummies.Count <= 0) {
            Debug.Log("IT WORKED!!!");
            _shooterAgent.Done();
            _walkingAgent.Done();
        }

        SpawnOneDummy();
    }

    public Vector3 RandomNewPosition() {
        var horizontalSpawn = Random.value > 0.5f;

        var result = Vector3.zero;

        if (horizontalSpawn) {
            var z = (Random.value >.5f ? 1 : -1) * _spawnArea.y;
            var x = Random.Range(-_spawnArea.x, _spawnArea.x);

            result.x = x;
            result.z = z;
        } else {

            var x = (Random.value >.5f ? 1 : -1) * _spawnArea.x;
            var z = Random.Range(-_spawnArea.y, _spawnArea.y);

            result.x = x;
            result.z = z;
        }

        return result + transform.position;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(_spawnArea.x, .5f, _spawnArea.y) * 2f);
    }
}