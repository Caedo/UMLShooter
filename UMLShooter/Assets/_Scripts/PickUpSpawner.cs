using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour {
    public ShooterAgent _agent;
    public HealthPickUp _healthPickUpPrefab;
    public int _pickupCount;
    public Vector2 _pickupSpawnArea;

    List<HealthPickUp> _pickupsList = new List<HealthPickUp>();

    private void Awake() {
        _agent.OnAgentReset += ResetSpawner;
    }

    public void ResetSpawner() {
        for (int i = 0; i < _pickupsList.Count; i++) {
            Destroy(_pickupsList[i].gameObject);
        }
        _pickupsList.Clear();

        for (int i = 0; i < _pickupCount; i++) {
            Spawn();
        }
    }

    public void Spawn() {
        var pos = RandomNewPosition();

        var pickup = Instantiate(_healthPickUpPrefab, pos, Quaternion.identity);
        pickup._spawner = this;

        _pickupsList.Add(pickup);
    }

    public void RemoveFromList(HealthPickUp pickup) {
        _pickupsList.Remove(pickup);
    }

    Vector3 RandomNewPosition() {
        var horizontalSpawn = Random.value > 0.5f;

        var result = Vector3.zero;

        if (horizontalSpawn) {
            var z = (Random.value >.5f ? 1 : -1) * _pickupSpawnArea.y;
            var x = Random.Range(-_pickupSpawnArea.x, _pickupSpawnArea.x);

            result.x = x;
            result.z = z;
        } else {

            var x = (Random.value >.5f ? 1 : -1) * _pickupSpawnArea.x;
            var z = Random.Range(-_pickupSpawnArea.y, _pickupSpawnArea.y);

            result.x = x;
            result.z = z;
        }

        result.y = 0.5f;
        return result + transform.position;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_pickupSpawnArea.x, 0.1f, _pickupSpawnArea.y) * 2);
    }
}