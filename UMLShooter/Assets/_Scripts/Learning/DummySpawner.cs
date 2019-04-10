using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour {
    public ShootDummy _dummyPrefab;
    public Transform[] _spawnPoints;
    public ShooterAgent _agent;
    public Vector2 _spawnArea;
    public int _dummySpawnCount;
    public int _maxDummiesCount;

    private ShootDummy[] _dummies;

    int _dummiesToSpawn;

    private void Awake() {
        _dummies = new ShootDummy[_spawnPoints.Length];
        ShootDummy.OnDummyKill += DummyKilled;
    }

    public void ResetSpawner() {
        _dummiesToSpawn = _maxDummiesCount;
        for (int i = 0; i < _dummies.Length; i++) {
            if (_dummies[i] != null) {
                DestroyImmediate(_dummies[i].gameObject);
                _dummies[i] = null;
            }
        }

        for (int i = 0; i < _dummySpawnCount; i++) {
            SpawnOneDummy();
        }
    }

    public void SpawnOneDummy() {
        if (_dummiesToSpawn <= 0)
            return;

        int index;
        do {
            index = Random.Range(0, _spawnPoints.Length);
        } while (_dummies[index] != null);

        var pos = RandomNewPosition();
        _spawnPoints[index].position = pos;
        var dummy = Instantiate(_dummyPrefab, pos, Quaternion.identity, transform);
        dummy._index = index;
        dummy._spawner = this;

        _dummies[index] = dummy;

        _dummiesToSpawn--;
    }

    void DummyKilled(ShootDummy dummy) {
        if (dummy._spawner != this)
            return;

        _dummies[dummy._index] = null;
        _agent.AddReward(1f);

        if (_dummiesToSpawn == 0) {
            bool allKilled = true;
            for (int i = 0; i < _dummies.Length; i++) {
                if (_dummies[i] != null) {
                    allKilled = false;
                    break;
                }
            }

            if (allKilled) {
                Debug.Log("IT WORKED!!!");
                _agent.Done();
            }
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