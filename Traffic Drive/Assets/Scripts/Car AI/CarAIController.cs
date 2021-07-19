using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIController : MonoBehaviour
{
    private float x_input;
    private float y_input;
    [SerializeField] private Vector3 centerOfMass;
    private Rigidbody rigidbody;

    [Header("Motor Settings")]
    [SerializeField] private float breakForce = 600f;
    [SerializeField] private float motorForce = 500f;
    [SerializeField] [Range(0, 90)] private float maxSteeringAngle = 4f;
    [SerializeField] private float topSpeed = 150f;


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

    public Waypoint currentWaypoint;
    public Vector3 waypointPos;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass;
        if(currentWaypoint != null) SetDestination(currentWaypoint);
    }

    private void FixedUpdate(){
            y_input = 0.5f;
            Vector3 destination = transform.InverseTransformPoint(waypointPos);
            destination /= destination.magnitude;
            x_input = (destination).x * 2f / (destination).magnitude; 
            
            CheckDestination();

        HandleMotor();
        Steering();
        UpdateWheel();
        BrakeHandler();
    }

    void BrakeHandler(){
        if(currentWaypoint.LampuMerah != null && !currentWaypoint.LampuMerah.isHijau){
            BanBelakangKanan.brakeTorque = breakForce * 5f;
            BanBelakangKiri.brakeTorque = breakForce * 5f;
            Debug.Log("Braking Lampu Merah "+Time.time);
        }else if(x_input > 0.7f && rigidbody.velocity.sqrMagnitude > topSpeed /2f){
            BanBelakangKanan.brakeTorque = breakForce;
            BanBelakangKiri.brakeTorque = breakForce;
        }else{
            BanBelakangKanan.brakeTorque = 0f;
            BanBelakangKiri.brakeTorque = 0f;
        }        
    }

    void CheckDestination(){
        if((waypointPos - transform.position).magnitude < 3f){
            if(currentWaypoint.LampuMerah != null && !currentWaypoint.LampuMerah.isHijau){
                BanDepanKanan.brakeTorque = breakForce * 10f;
                BanDepanKiri.brakeTorque = breakForce * 10f;
            }else{
                ReachedDestination();
            }
        }
    }

    public void ReachedDestination(){
        BanDepanKanan.brakeTorque = 0f;
                BanDepanKiri.brakeTorque = 0f;
        if(currentWaypoint.Branches.Count > 0){
            if(Random.Range(0f, 1f) < currentWaypoint.branchRatio){
                currentWaypoint = currentWaypoint.Branches[Random.Range(0, currentWaypoint.Branches.Count)];
            }else{
                if(currentWaypoint.nextWaypoint != null){
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
            }
        }else if(currentWaypoint.nextWaypoint != null){
            currentWaypoint = currentWaypoint.nextWaypoint;
        }

        waypointPos = currentWaypoint.GetPosition();
    }

    public void SetDestination(Waypoint waypoint){
        currentWaypoint = waypoint;
        waypointPos = currentWaypoint.GetPosition();
    }

    private void HandleMotor(){
        if(rigidbody.velocity.sqrMagnitude < topSpeed){
            BanDepanKanan.motorTorque = y_input * motorForce;
            BanDepanKiri.motorTorque =  y_input * motorForce;
        }else{
            BanDepanKanan.motorTorque = y_input * 0f;
            BanDepanKiri.motorTorque =  y_input * 0f;
        }
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
