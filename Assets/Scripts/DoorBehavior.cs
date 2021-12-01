using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour {

    [SerializeField] private int lanternMax = 7;
    [SerializeField] private Material fireFlyMat;

    private Transform doorArch;
    private Animator anim;
    private int lanternCount;

    private void Awake() {
        anim = GetComponent<Animator>();
        doorArch = GameObject.FindWithTag("DoorArch").transform;
    }
    
    private void OnEnable() {
        EventManager.StartListening(EventManager.Event.LanternLit, LanternLit);
    }
    
    private void OnDisable() {
        EventManager.StopListening(EventManager.Event.LanternLit, LanternLit);
    }

    private void LanternLit() {
        doorArch.GetChild(lanternCount).GetComponent<MeshRenderer>().material = fireFlyMat;
        lanternCount++;
        
        if (lanternCount == lanternMax) {
            anim.SetTrigger("Open");
            enabled = false;
        }
    }
}
