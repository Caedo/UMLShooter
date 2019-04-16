using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour {
    public ShootDummy _dummyPrefab;
    public ShooterAgent _agent;
    public Vector2 _spawnArea;
    public int _dummySpawnCount;
    public int _maxDummiesCount;

    // Need reference to spawned dummies
    public List<ShootDummy> _spawnedDummies = new List<ShootDummy>();

    int _dummiesToSpawn;

    private void Awake() {
        ShootDummy.OnDummyKill += DummyKilled;
        _agent.OnAgentReset += ResetSpawner;
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

        _agent.KilledEnemy();
        _spawnedDummies.Remove(dummy);

        if (_dummiesToSpawn == 0 && _spawnedDummies.Count <= 0) {
            Debug.Log("IT WORKED!!!");
            _agent.Done();
        }

        SpawnOneDummy();
    }

    public Vector3 RandomNewPosition() {
        return new Vector3(Random.Range(-_spawnArea.x, _spawnArea.x), 0, Random.Range(-_spawnArea.y, _spawnArea.y)) + transform.position;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(_spawnArea.x, .5f, _spawnArea.y) * 2f);
    }
}