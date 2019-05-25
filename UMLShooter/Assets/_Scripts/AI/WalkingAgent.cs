using System.Collections;
using System.Collections.Generic;
using MLAgents;
using RoboRyanTron.Events;
using UnityEngine;

public class WalkingAgent : Agent
{
    [Header("Ray Perception")]
    [Range(0f, 360f)]
    public float _rayAngleRange;
    public float _rayAngleRangeOffset;
    public int _raysNumber;
    public float _rayDistance;

    [Header("Positions")]
    public Transform _originPoint;


    [Header("Debug")]
    public bool _useMonitor;

    public event System.Action OnAgentReset;

    Vector3 _startPos;

    Rigidbody _body;
    PlayerMovement _movement;
    PlayerEntity _entity;
    PlayerShooting _shooting;

    RayPerception _rayPerception;

    bool dead;

    string[] _detectableObjects = new string[] { "Enemy" };
    float[] _rayAngles;

    void Awake() {
        _startPos = transform.position;
        _movement = GetComponent<PlayerMovement>();
        _body = GetComponent<Rigidbody>();
        _entity = GetComponent<PlayerEntity>();
        _shooting = GetComponent<PlayerShooting>();
        _rayPerception = GetComponent<RayPerception>();

        // _entity.OnPlayerDamage += PlayerDamage;
        // _entity.OnPlayerDeath += PlayerDeath;

        CreateRayAngles();
    }

    void CreateRayAngles() {
        var startAngle = _rayAngleRangeOffset;
        var step = _rayAngleRange / (_raysNumber - 1);

        _rayAngles = new float[_raysNumber];
        for (int i = 0; i < _raysNumber; i++) {
            _rayAngles[i] = startAngle + i * step;
        }
    }

    public override void AgentReset() {
        transform.position = _startPos;
        _movement.ResetVelocity();
        _entity.ResetHealth();

        OnAgentReset?.Invoke();
    }

    public override void CollectObservations() {
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };

        //AddVectorObs(transform.position);
        var rot = transform.rotation.eulerAngles.y / 360f;
        var pos = transform.position - _originPoint.position;
        pos /= 14f;
        
        var perception = _rayPerception.Perceive(_rayDistance, _rayAngles, _detectableObjects, 1f, 0);
        AddVectorObs(rot);
        AddVectorObs(pos);
        AddVectorObs(perception);

        if (_useMonitor) {
            Monitor.Log("Rotation: ", rot.ToString());
            //Monitor.Log("Position: ", pos.ToString());
            Monitor.Log("Perception: ", perception.ToArray());
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction) {
        // 0 -> x move axis
        // 1 -> y move axis
        // 2 -> rotation
        // 3 -> fire

        // Move agent
        var moveVector = new Vector3(vectorAction[0], 0, vectorAction[1]);
        _movement.Move(moveVector);

        if (transform.position.y < _startPos.y - 1f) {
            SetReward(-1f);
            Done();
        }

        AddReward(-1f / agentParameters.maxStep);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            SetReward(-0.01f);
        }
    }

    void PlayerDeath() {
        // Debug.Log("Done by death");
        // SetReward(-100f);
        Done();
    }
}
