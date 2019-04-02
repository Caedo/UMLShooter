using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class ShooterAgent : Agent {

    public Transform _target;

    Vector3 _startPos;
    PlayerMovement _movement;
    Rigidbody _body;

    bool _targetReached = false;

    void Awake() {
        _startPos = transform.position;
        _movement = GetComponent<PlayerMovement>();
        _body = GetComponent<Rigidbody>();
    }

    public override void AgentReset() {
        transform.position = _startPos;
        _movement.ResetVelocity();

        _target.position = new Vector3(Random.value * 15 - 7, 0.5f, Random.value * 15 - 7);
        _targetReached = false;
    }

    public override void CollectObservations() {
        AddVectorObs(transform.position);
        AddVectorObs(_target.position);

        AddVectorObs(_body.velocity);
    }

    public override void AgentAction(float[] vectorAction, string textAction) {
        var moveVector = new Vector3(vectorAction[0], 0, vectorAction[1]);
        _movement.Move(moveVector);

        if (_targetReached) {
            SetReward(2f);
            Done();
        }

        if (transform.position.y < -2f) {
            SetReward(-2f);
            Done();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Target") {
            _targetReached = true;
        }
    }
}