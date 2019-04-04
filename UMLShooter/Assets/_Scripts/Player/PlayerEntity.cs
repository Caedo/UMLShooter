using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity {
    public System.Action OnPlayerDeath;
    public System.Action OnPlayerDamage;

    public override void TakeDamage(float dmg) {
        OnPlayerDamage?.Invoke();
        base.TakeDamage(dmg);
    }

    protected override void Death() {
        OnPlayerDeath?.Invoke();
        Debug.Log("Death");
        //Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}