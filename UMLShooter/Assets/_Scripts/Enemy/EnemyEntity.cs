using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class EnemyEntity : Entity {

    public GameEvent _enemyKillEvent;
    public GameEvent _enemyDamageEvent;

    protected override void Death() {
        _enemyKillEvent.Raise();
    }

    public override void TakeDamage(float dmg) {
        _enemyDamageEvent.Raise();

        base.TakeDamage(dmg);
    }
}