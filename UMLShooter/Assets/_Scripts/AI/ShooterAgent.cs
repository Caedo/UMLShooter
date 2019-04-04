using System.Collections;
using System.Collections.Generic;
using MLAgents;
using RoboRyanTron.Events;
using UnityEngine;

public class ShooterAgent : Agent {

    Vector3 _startPos;

    Rigidbody _body;
    PlayerMovement _movement;
    PlayerEntity _entity;
    PlayerShooting _shooting;

    RayPerception _rayPerception;

    bool dead;

    string[] _detectableObjects = new string[] { "Enemy" };

    void Awake() {
        _startPos = transform.position;
        _movement = GetComponent<PlayerMovement>();
        _body = GetComponent<Rigidbody>();
        _entity = GetComponent<PlayerEntity>();
        _shooting = GetComponent<PlayerShooting>();
        _rayPerception = GetComponent<RayPerception>();

        _entity.OnPlayerDamage += PlayerDamage;
        _entity.OnPlayerDeath += PlayerDeath;
    }

    public override void AgentReset() {
        transform.position = _startPos;
        _movement.ResetVelocity();
        _entity.ResetHealth();
    }

    public override void CollectObservations() {

        float rayDistance = 20f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };

        AddVectorObs(transform.position);
        AddVectorObs(_body.velocity);
        AddVectorObs(_rayPerception.Perceive(rayDistance, rayAngles, _detectableObjects, 0.5f, .1f));
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
        _movement.LookTowards(lookPostion);

        // Fire 
        var shootDecision = (int) vectorAction[4];
        if (shootDecision == 1) {
            _shooting.Fire();
        }

        if (transform.position.y < -2f) {
            SetReward(-2f);
            Done();
        }
    }

    void PlayerDeath() {
        Debug.Log("Done by death");
        SetReward(-100f);
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