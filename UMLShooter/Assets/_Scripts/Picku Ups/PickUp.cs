using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour {

    public abstract void Affect(GameObject Player);

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Affect(other.gameObject);
        }
    }
}