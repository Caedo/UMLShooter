using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDummy : Entity {

    public static event System.Action<ShootDummy> OnDummyKill;

    [HideInInspector]
    public int _index;

    [HideInInspector]
    public DummySpawner _spawner;

    //public Vector2 _speedRange;

    Vector3 _destination;
    float _speed;

    protected override void Start() {
        base.Start();

        _speed = Random.Range(ShootingLearnAcademy.Instance.resetParameters["speed_min"], ShootingLearnAcademy.Instance.resetParameters["speed_max"]);
        _destination = _spawner.RandomNewPosition();
    }

    private void FixedUpdate() {
        var dir = (_destination - transform.position);
        transform.Translate(dir.normalized * _speed * Time.fixedDeltaTime);

        if (dir.sqrMagnitude < 0.1f) {
            _destination = _spawner.RandomNewPosition();
        }
    }

    protected override void Death() {
        Debug.Log("Dummy zdech");
        OnDummyKill?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, _destination);
    }
}