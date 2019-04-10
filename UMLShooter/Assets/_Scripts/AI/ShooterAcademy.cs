using System.Collections;
using System.Collections.Generic;
using MLAgents;
using RoboRyanTron.Events;
using UnityEngine;

public class ShooterAcademy : Academy {

    public GameEvent _gameEndedEvent;

    private void Start() {
        FindObjectOfType<PlayerEntity>().OnPlayerDeath += OnPlayerDeath;
    }

    public override void AcademyReset() {
        //_gameEndedEvent.Raise();
    }

    void OnPlayerDeath(){
        _gameEndedEvent.Raise();
    }

    public override void InitializeAcademy()
    {
        Monitor.SetActive(true);
    }
}