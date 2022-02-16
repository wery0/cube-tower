using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Transform camTransform;
    private float shakeDuration = 1f;
    private float shakePower = 0.05f;
    private float decreaseFactor = 1.5f;

    private Vector3 originCamPos;

    private void Start() {
        camTransform = GetComponent<Transform>();
        originCamPos = camTransform.localPosition;
    }

    private void Update() {
        if (shakeDuration > 0) {
            camTransform.localPosition = originCamPos + Random.insideUnitSphere * shakePower;
            shakeDuration -= decreaseFactor * Time.deltaTime;
        }
        else {
            shakeDuration = 0;
            camTransform.localPosition = originCamPos;
        }
    }
}
