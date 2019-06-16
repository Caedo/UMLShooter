using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour {

    public PickUpSpawner _spawner;

    public void Remove() {
        _spawner.Spawn();
        _spawner.RemoveFromList(this);
    }
}