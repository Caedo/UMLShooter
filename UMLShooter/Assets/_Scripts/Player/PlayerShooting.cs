using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    public Weapon[] _weaponsPrefabs;
    public Transform _weaponGrip;

    Weapon _equippedWeapon;
    Weapon[] _spawnedWeapons;

    void Start() {
        _spawnedWeapons = new Weapon[_weaponsPrefabs.Length];

        for (int i = 0; i < _weaponsPrefabs.Length; i++) {
            _spawnedWeapons[i] = Instantiate(_weaponsPrefabs[i], _weaponGrip);
            _spawnedWeapons[i].gameObject.SetActive(false);
        }

        EquipWeapon(0);

    }

    public void Fire() {
        if (_equippedWeapon != null) {
            _equippedWeapon.Fire();
        }
    }

    public void EquipWeapon(int weaponIndex) {
        var tryToEquip = _spawnedWeapons[weaponIndex];

        if (tryToEquip._avaible && (tryToEquip._currentAmmo > 0 || tryToEquip._infinityAmmo)) {
            _equippedWeapon?.gameObject.SetActive(false);
            _equippedWeapon = tryToEquip;
            _equippedWeapon.gameObject.SetActive(true);

            _equippedWeapon.Equip(_weaponGrip);
        }
    }
}