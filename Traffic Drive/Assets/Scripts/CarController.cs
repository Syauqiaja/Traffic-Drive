using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{   
    private float x_input;
    private float y_input;
    private bool isBreaking;

    [SerializeField] private Vector3 centerOfMass;
    private Rigidbody rigidbody;

    [Header("Motor Settings")]
    [SerializeField] private float breakForce = 600f;
    [SerializeField] private float motorForce = 500f;
    [SerializeField] [Range(0, 90)] private float maxSteeringAngle = 45f;
    [SerializeField] [Range(0f, 1f)] private float driftValue = 0.5f;
    [SerializeField] private float brakeStiffness = 0.25f;
    [SerializeField] private float topSpeed = 150f;
    [SerializeField] private float topReverseSpeed = 25f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider BanDepanKiri;
    [SerializeField] private WheelCollider BanBelakangKiri;
    [SerializeField] private WheelCollider BanDepanKanan;
    [SerializeField] private WheelCollider BanBelakangKanan;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform BanDepanKiriTransform;
    [SerializeField] private Transform BanBelakangKiriTransform;
    [SerializeField] private Transform BanDepanKananTransform;
    [SerializeField] private Transform BanBelakangKananTransform;

    private float currentFrictionStiffness;

    private Vector3 startPos;
    private Quaternion startRot;
    private void Start() {
        startPos = transform.position;
        startRot = transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass;
        currentFrictionStiffness = BanBelakangKanan.sidewaysFriction.stiffness;
    }

    private void FixedUpdate(){
            x_input = Input.GetAxis("Horizontal");
            y_input = Input.GetAxis("Vertical");

            isBreaking = Input.GetButton("Jump");
            if(Input.GetButtonDown("Submit")){
                transform.position = startPos;
                transform.rotation = startRot;
            }
        Debug.Log(isBreaking);

        HandleMotor();
        Steering();
        UpdateWheel();

    }

    private void Update() {
        if(Input.GetButtonUp("Jump")){
            BanDepanKanan.brakeTorque = 0;
            BanDepanKiri.brakeTorque = 0;
            BanBelakangKanan.brakeTorque = 0;
            BanBelakangKiri.brakeTorque = 0;
        }        
    }
    

    private void HandleMotor(){
        if((rigidbody.velocity.sqrMagnitude < topSpeed && BanDepanKanan.rpm > 0) || (rigidbody.velocity.sqrMagnitude < topReverseSpeed && BanDepanKanan.rpm < 0)){
            if((y_input < -0.1f && BanDepanKanan.rpm > 1f) || (y_input > 0.1f && BanDepanKanan.rpm < -1f)){
                BanBelakangKanan.brakeTorque = breakForce/2f;
                BanBelakangKiri.brakeTorque = breakForce/2f;
            }else{
                BanDepanKanan.motorTorque = y_input * motorForce;
                BanDepanKiri.motorTorque =  y_input * motorForce;
                BanBelakangKanan.brakeTorque = 0f;
                BanBelakangKiri.brakeTorque = 0f;
            }
        }else{
            BanDepanKanan.motorTorque = 0;
            BanDepanKiri.motorTorque =  0;
        }

        float _currentBreak = isBreaking ? breakForce : 0f;
        if(isBreaking){
            BanBelakangKanan.brakeTorque = _currentBreak;
            BanBelakangKiri.brakeTorque = _currentBreak;
            Debug.Log("Braking");

            Drifting();
        }else{
            ControlDrift();
        }
    }

    private void Drifting(){
        WheelFrictionCurve frictionCurve = BanBelakangKanan.sidewaysFriction;
            frictionCurve.stiffness = brakeStiffness;
            BanBelakangKanan.sidewaysFriction = frictionCurve;
            BanBelakangKiri.sidewaysFriction = frictionCurve;
    }
    private void ControlDrift(){
        WheelFrictionCurve frictionCurve = BanBelakangKanan.sidewaysFriction;
        frictionCurve.stiffness = Mathf.Lerp(BanBelakangKanan.sidewaysFriction.stiffness, currentFrictionStiffness, driftValue * Time.deltaTime *2f);
        BanBelakangKanan.sidewaysFriction = frictionCurve;
        BanBelakangKiri.sidewaysFriction = frictionCurve;
    }

    private void UpdateWheel(){
        changeWheelRotation(BanDepanKanan, BanDepanKananTransform);
        changeWheelRotation(BanDepanKiri, BanDepanKiriTransform);
        changeWheelRotation(BanBelakangKanan, BanBelakangKananTransform);
        changeWheelRotation(BanBelakangKiri, BanBelakangKiriTransform);
    }

    private void Steering(){
        float _currentAngle = x_input * maxSteeringAngle;
        if (x_input > 0 ) {
            BanDepanKanan.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (maxSteeringAngle + (1.5f / 2))) * x_input;
            BanDepanKiri.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (maxSteeringAngle - (1.5f / 2))) * x_input;
        } else if (x_input < 0 ) {                                                          
            BanDepanKanan.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (maxSteeringAngle - (1.5f / 2))) * x_input;
            BanDepanKiri.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (maxSteeringAngle + (1.5f / 2))) * x_input;
        } else {
            BanDepanKanan.steerAngle =0;
            BanDepanKiri.steerAngle =0;
        }
    }

    private void changeWheelRotation(WheelCollider wheelCollider, Transform wheelTransform){
        Vector3 pos;
        Quaternion quat;
        wheelCollider.GetWorldPose(out pos, out quat);

        wheelTransform.rotation = quat;
        wheelTransform.position = pos;
    }
}
