using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private void Start() {
        EventManager.TriggerEvent(EventManager.Event.StartGame);
    }
}
