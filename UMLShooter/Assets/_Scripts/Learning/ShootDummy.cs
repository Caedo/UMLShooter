using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Random,
    ToAgent,
}

public class ShootDummy : Entity {

    public static event System.Action<ShootDummy> OnDummyKill;
    public MovementType _movementType;

    [HideInInspector]
    public int _index;

    [HideInInspector]
    public DummySpawner _spawner;

    //public Vector2 _speedRange;

    Vector3 _destination;
    float _speed;

    Transform _agentTransform;

    protected override void Start() {
        base.Start();

        _speed = Random.Range(ShootingLearnAcademy.Instance.resetParameters["speed_min"], ShootingLearnAcademy.Instance.resetParameters["speed_max"]);
        _destination = _spawner.RandomNewPosition();

        _agentTransform = _spawner._agent.transform;
    }

    private void FixedUpdate() {
        if(_movementType == MovementType.ToAgent)
            _destination = _agentTransform.position;

        var dir = (_destination - transform.position);
        transform.Translate(dir.normalized * _speed * Time.fixedDeltaTime);

        if (_movementType == MovementType.Random && dir.sqrMagnitude < 0.1f) {
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