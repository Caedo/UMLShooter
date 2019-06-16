using System.Collections;
using System.Collections.Generic;
using MLAgents;
using RoboRyanTron.Events;
using UnityEngine;

public class ShooterAgent : Agent {

    [Header("Health")]
    public float _dummyDamage = 10;
    public float _healAmount = 15;
    public float _maxHealth = 100;
    public float _invinsibilityTime = 1f;

    [Header("Rendering")]
    public Material _normalMaterial;
    public Material _damagedMaterial;
    public Renderer _bodyRenderer;
    private float _currentHealth;
    private bool _invinsible;

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
    PlayerShooting _shooting;

    RayPerception _rayPerception;

    bool dead;

    string[] _detectableObjects = new string[] { "Enemy", "HealthPickup" };
    float[] _rayAngles;

    void Awake() {
        _startPos = transform.position;
        _movement = GetComponent<PlayerMovement>();
        _body = GetComponent<Rigidbody>();
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

        _currentHealth = _maxHealth;

        StopCoroutine("InvinsibleRoutine");
        _bodyRenderer.material = _normalMaterial;
        _invinsible = false;

        OnAgentReset?.Invoke();
    }

    public override void CollectObservations() {
        
        //AddVectorObs(transform.position);
        var rot = transform.rotation.eulerAngles.y / 360f;
        var pos = transform.position - _originPoint.position;
        pos /= 14f;

        var perception = _rayPerception.Perceive(_rayDistance, _rayAngles, _detectableObjects, 1f, 0);
        AddVectorObs(rot);
        AddVectorObs(pos);
        AddVectorObs(perception);
        AddVectorObs(_currentHealth / _maxHealth);

        if (_useMonitor) {
            //Monitor.Log("Rotation: ", rot.ToString());
            //Monitor.Log("Position: ", pos.ToString());
            //Monitor.Log("Perception: ", perception.ToArray());

            Monitor.Log("Health: ", _currentHealth.ToString());
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

        // Rotate Agent
        // var lookPostion = new Vector3(vectorAction[2], 0, vectorAction[3]);
        // _movement.LookTowards(transform.position + lookPostion);

        // let's assume that 0 == 0 degrees and 1 == 360 degrees
        var rotation = vectorAction[2];
        var angle = (rotation * 360f + 90F) * Mathf.Deg2Rad;

        var xLook = Mathf.Cos(angle);
        var yLook = Mathf.Sin(angle);

        _movement.LookTowards(transform.position + new Vector3(xLook, 0, yLook));

        // Fire 
        var shootDecision = (int) vectorAction[3];
        if (shootDecision == 1) {
            _shooting.Fire();
        }

        if (transform.position.y < _startPos.y - 1f) {
            SetReward(-1f);
            Done();
        }

        // Existencial reward
        AddReward(1f / agentParameters.maxStep);
    }

    public void KilledEnemy() {
        AddReward(1f);
    }

    IEnumerator InvinsibleRoutine() {
        _invinsible = true;
        _bodyRenderer.material = _damagedMaterial;

        yield return new WaitForSeconds(_invinsibilityTime);

        _bodyRenderer.material = _normalMaterial;
        _invinsible = false;
    }

    // private void OnCollisionEnter(Collision other) {
    //     // if(other.gameObject.CompareTag("Enemy"))
    //     // {
    //     //     AddReward(-0.1f);
    //     // }
    // }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Enemy") && !_invinsible) {
            AddReward(-0.15f);

            _currentHealth -= _dummyDamage;

            StartCoroutine("InvinsibleRoutine");

            if (_currentHealth <= 0) {
                // Player Killed :<
                AddReward(-1f);
                Done();
            }
        }

        if (other.CompareTag("HealthPickup")) {
            var healthDelta = _maxHealth - _currentHealth;

            var heal = Mathf.Min(healthDelta, _healAmount);
            _currentHealth += heal;

            AddReward(heal / _maxHealth);

            var pickUp = other.GetComponent<HealthPickUp>();
            pickUp.Remove();
            Destroy(other.gameObject);
        }
    }
}