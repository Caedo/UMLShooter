using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBullet : Bullet {

    public Transform _hitPoint;

    void FixedUpdate() {
        RaycastHit hit;
        var dist = _body.velocity.magnitude + 0.001f;
        if (Physics.Raycast(_hitPoint.position, _hitPoint.forward, out hit, dist, _enemyMask)) {
            var entity = hit.collider.GetComponent<Entity>();
            if (entity) {
                entity.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }

}