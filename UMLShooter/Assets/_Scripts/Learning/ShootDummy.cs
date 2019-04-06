using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDummy : Entity {
    
    public static event System.Action<ShootDummy> OnDummyKill;
    public int _index;
    public DummySpawner _spawner;

    protected override void Death() {
        Debug.Log("Dummy zdech");
        OnDummyKill?.Invoke(this);
        Destroy(gameObject);
    }
}