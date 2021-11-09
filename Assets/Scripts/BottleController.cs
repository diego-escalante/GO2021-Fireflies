using System;
using UnityEditor;
using UnityEngine;

public class BottleController : MonoBehaviour {

    [SerializeField] private LayerMask fliesLayer;
    [SerializeField] private Transform coneBaseCenter;
    [SerializeField] private float coneBaseRadius;
    
    private Animator anim;
    private float cosOfHalfAperture;

    private void Awake() {
        anim = GetComponent<Animator>();
        cosOfHalfAperture = Mathf.Cos(Mathf.Atan2(coneBaseRadius, Vector3.Distance(transform.position, coneBaseCenter.position)));
    }
    private void Update() {
        if (Input.GetKey(KeyCode.Minus)) coneBaseRadius -= Time.deltaTime * 0.1f;
        if (Input.GetKey(KeyCode.Plus)) coneBaseRadius += Time.deltaTime * 0.1f;
        cosOfHalfAperture = Mathf.Cos(Mathf.Atan2(coneBaseRadius, Vector3.Distance(transform.position, coneBaseCenter.position)));
        
        anim.SetBool("isHoldingOut", Input.GetButton("Fire1"));
        GameObject[] objs = GameObject.FindGameObjectsWithTag("test");

        for (int i = 0; i < objs.Length; i++) {
            objs[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        
        Collider[] colls = getFliesInBroadBox();
        for (int i = 0; i < colls.Length; i++) {
            colls[i].GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
        }

        for (int i = 0; i < objs.Length; i++) {
            if (isInCone(objs[i].transform.position)) {
                objs[i].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
        }
    }

    // https://stackoverflow.com/questions/10768142/verify-if-point-is-inside-a-cone-in-3d-space
    private bool isInCone(Vector3 point) {
        Vector3 apexToPoint = transform.position - point;
        Vector3 coneAxis = transform.position - coneBaseCenter.position;
        float apexToPointDotConeAxis = Vector3.Dot(apexToPoint, coneAxis);

        // Return early if the point is not inside a "infinite" version of the cone.
        if (apexToPointDotConeAxis / apexToPoint.magnitude / coneAxis.magnitude < cosOfHalfAperture) {
            return false;
        }
        
        // At this point, we know that angle-wise, the point is inside the code, but is it distance-wise?
        return apexToPointDotConeAxis / coneAxis.magnitude < coneAxis.magnitude;
    }
    
    private Collider[] getFliesInBroadBox() {
        Vector3 boxCenter = Vector3.Lerp(transform.position, coneBaseCenter.position, 0.5f);
        Vector3 boxHalfExtents = new Vector3(coneBaseRadius, coneBaseRadius, Vector3.Distance(boxCenter, coneBaseCenter.position));
        return Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.LookRotation(transform.position - coneBaseCenter.position), fliesLayer);
    }
}
