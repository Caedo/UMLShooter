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
    
    public abstract void Fire();
}
