using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : Weapon {

    public Bullet _bulletPrefab;
    public float _roundsPerSecond;
    public float _dispresionAngle;

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
            var bullet = Instantiate(_bulletPrefab, _gunPoint.position, _gunPoint.rotation);
            bullet._damage = _damage;

            _shootTimer = 0;
        }
    }
}