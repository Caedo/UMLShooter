using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUp {
    public float _healAmount;

    public override void Affect(GameObject Player) {
        Player.GetComponent<PlayerEntity>().TakeDamage(-_healAmount);
    }
}