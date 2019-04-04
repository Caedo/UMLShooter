using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform _Target;
    public bool _UseStartOffset;
    public Vector3 _Offset;

    Vector3 _CurrentOffset;

    void Start() {
        _CurrentOffset = _UseStartOffset ? transform.position - _Target.position : _Offset;
    }

    private void LateUpdate() {
        if (_Target != null)
            transform.position = _Target.position + _CurrentOffset;
    }
}