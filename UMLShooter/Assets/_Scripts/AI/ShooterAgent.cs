using System.Collections;
using System.Collections.Generic;
using MLAgents;
using RoboRyanTron.Events;
using UnityEngine;

public class ShooterAgent : Agent {

    [Header("Ray Perception")]
    [Range(0f, 360f)]
    public float _rayAngleRange;
    public float _rayAngleRangeOffset;
    public float _rayAngleRangeStep;
    public float _rayDistance;

    [Header("Events")]
    public GameEvent _agentReset;

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
        int stepCount = Mathf.CeilToInt(_rayAngleRange / _rayAngleRangeStep);
        _rayAngles = new float[stepCount];

        for (int i = 0; i < stepCount; i++) {
            _rayAngles[i] = startAngle + i * _rayAngleRangeStep;
        }
    }

    public override void AgentReset() {
        transform.position = _startPos;
        _movement.ResetVelocity();
        _entity.ResetHealth();

        _agentReset.Raise();
    }

    public override void CollectObservations() {
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };

        //AddVectorObs(transform.position);
        AddVectorObs(transform.rotation.y);
        AddVectorObs(_body.velocity);
        AddVectorObs(_rayPerception.Perceive(_rayDistance, rayAngles, _detectableObjects, 1f, 0));
    }

    public override void AgentAction(float[] vectorAction, string textAction) {
        // 0 -> x move axis
        // 1 -> y move axis
        // 2 -> x look position
        // 3 -> y look position
        // 4 -> fire

        // Move agent
        var moveVector = new Vector3(vectorAction[0], 0, vectorAction[1]);
        _movement.Move(moveVector);

        // Rotate Agent
        var lookPostion = new Vector3(vectorAction[2], 0, vectorAction[3]);
        _movement.LookTowards(transform.position + lookPostion);

        // Fire 
        var shootDecision = (int) vectorAction[4];
        if (shootDecision == 1) {
            _shooting.Fire();
        }

        if (transform.position.y < _startPos.y - 1f) {
            SetReward(-1f);
            Done();
        }

        AddReward(-1f / agentParameters.maxStep);
    }

    // private void OnCollisionEnter(Collision other) {
    //     var dummy = other.collider.GetComponentInParent<ShootDummy>();
    //     if(dummy){
    //         dummy.TakeDamage(200f);
    //     }
    // }

    void PlayerDeath() {
        // Debug.Log("Done by death");
        // SetReward(-100f);
        Done();
    }

    void PlayerDamage() {
        SetReward(-1f);
    }

    public void OnEnemyDeath() {
        SetReward(5f);
    }

    public void OnEnemyDamage() {
        SetReward(1f);
    }
}