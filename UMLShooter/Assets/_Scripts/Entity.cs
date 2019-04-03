using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public float _Health;

    public virtual void TakeDamage(float dmg) {
        _Health -= dmg;
        if (_Health <= 0) {
            Death();
        }
    }

    protected virtual void Death() {

    }
}