using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    GameObject cameraObj;

    void Awake() {
        cameraObj = GameObject.Find("Player Camera");
    }

    // Update is called once per frame
    void Update() {
        transform.localRotation = cameraObj.transform.rotation;
    }
}
