using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private Vector3 cameraOffside;

    [Header("Ban Belakang")]
    [SerializeField] private WheelCollider banKanan;
    [SerializeField] private WheelCollider banKiri;

    private float speed;
    private void Start() {
        speed = maxSpeed;
    }
    void FixedUpdate()
    {

        Vector3 _targetPos = targetTransform.TransformPoint(cameraOffside);
        transform.position = Vector3.Lerp(transform.position, _targetPos, speed * Time.deltaTime);

        transform.LookAt(targetTransform, Vector3.up);
    }
}
