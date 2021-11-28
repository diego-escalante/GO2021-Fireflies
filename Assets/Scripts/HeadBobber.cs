using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour {

    private Animator anim;
    private PlayerController controller;

    private void Awake() {
        anim = GetComponent<Animator>();
        controller = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void Update() {
        anim.SetBool("isMoving", controller.IsMoving() && controller.IsGrounded());
    }

}
