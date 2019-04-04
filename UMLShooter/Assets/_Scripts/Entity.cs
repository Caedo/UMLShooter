using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float _MaxHealth;
    protected float _health;

    protected virtual void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _health = _MaxHealth;
    }

    public virtual void TakeDamage(float dmg) {
        _health -= dmg;
        if (_health <= 0) {
            Death();
        }
    }

    protected virtual void Death() {

    }
}