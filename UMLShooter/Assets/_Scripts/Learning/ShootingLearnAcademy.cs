using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class ShootingLearnAcademy : Academy {
    static ShootingLearnAcademy _instance;
    public static ShootingLearnAcademy Instance => _instance;

    public override void InitializeAcademy() {
        if (_instance != null) {
            Debug.LogError("ANOTHER INSTANCE OF ACADEMY!!!");
            return;
        }

        _instance = this;
    }
}