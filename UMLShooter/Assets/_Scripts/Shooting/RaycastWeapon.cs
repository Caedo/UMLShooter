using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon {
    public float _roundsPerSecond;
    public LineRenderer _line;

    float _shootTimer;
    float _timeBetweenBullets;

    void Start() {
        _timeBetweenBullets = 1 / _roundsPerSecond;
    }

    private void Update() {
        _shootTimer += Time.deltaTime;
    }

    public override void Fire() {
        if (_shootTimer > _timeBetweenBullets && (_currentAmmo > 0 || _infinityAmmo)) {

            RaycastHit hit;

            //if (Physics.Raycast(_gunPoint.position, _gunPoint.forward, out hit)) {
            if (Physics.SphereCast(_gunPoint.position, 0.15f, _gunPoint.forward, out hit)) {
                var entity = hit.collider.GetComponentInParent<Entity>();
                if (entity) {
                    entity.TakeDamage(_damage);
                }

                _line.SetPosition(0, _gunPoint.position);
                _line.SetPosition(1, hit.point);

                StartCoroutine(LineRoutine());
            }

            _shootTimer = 0;
        }
    }

    IEnumerator LineRoutine() {
        _line.enabled = true;
        yield return new WaitForSeconds(_timeBetweenBullets / 2);
        _line.enabled = false;
    }

}