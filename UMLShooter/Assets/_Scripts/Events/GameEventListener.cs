// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoboRyanTron.Events {
    public class GameEventListener : MonoBehaviour {
        [Tooltip("Events list to register with.")]
        public List<GameEvent> Events;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        private void OnEnable() {
            foreach (var item in Events) {
                item.RegisterListener(this);

            }
        }

        private void OnDisable() {
            foreach (var item in Events) {
                item.UnregisterListener(this);
            }
        }

        public void OnEventRaised() {
            Response.Invoke();
        }
    }
}