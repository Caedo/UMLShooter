using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float _damage;
    public int _maxAmmo;
    public int _currentAmmo;

    public bool _infinityAmmo;
    public bool _avaible;

    public Transform _gunPoint;
    
    public abstract void Fire();

    public void Equip(Transform grip){
        transform.parent = grip;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
