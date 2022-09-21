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
    [SerializeField] [Range(4f, 10f)] private float maxSteeringAngle = 4f;
    [SerializeField] [Range(0f, 1f)] private float driftValue = 0.5f;
    [SerializeField] private float brakeStiffness = 0.25f;
    [SerializeField] private float topSpeed = 150f;
    [SerializeField] private float topReverseSpeed = 25f;
    [SerializeField] private List<AudioClip> EnggineSounds = new List<AudioClip>();
    public float currspeed = 0f;

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
    private AudioSource audioSource;
    private Quaternion startRot;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Start() {
        startPos = transform.position;
        startRot = transform.rotation;
        rigidbody.centerOfMass = centerOfMass;
        currentFrictionStiffness = BanBelakangKanan.sidewaysFriction.stiffness;
    }

    private void FixedUpdate(){
        if(PlayerManager.Instance.allowInput){
            x_input = Input.GetAxis("Horizontal");
            y_input = Input.GetAxis("Vertical");

            isBreaking = Input.GetButton("Jump");
            if(Input.GetButtonDown("Submit")){
                transform.position = startPos;
                transform.rotation = startRot;
            }
        // Debug.Log(isBreaking);

        HandleMotor();
        Steering();
        UpdateWheel();
        UpdateSound();
        }else{
            BanBelakangKanan.brakeTorque = 6000f;
            BanBelakangKiri.brakeTorque = 6000f;
            BanDepanKanan.brakeTorque = 6000f;
            BanDepanKiri.brakeTorque = 6000f;
        }
    }

    private void Update() {
        if(Input.GetButtonUp("Jump")){
            BanDepanKanan.brakeTorque = 0;
            BanDepanKiri.brakeTorque = 0;
            BanBelakangKanan.brakeTorque = 0;
            BanBelakangKiri.brakeTorque = 0;
        }
        if(Input.GetKeyDown(KeyCode.H)){
            SoundSystem.PlaySounds(SoundSystem.Tracks.Klakson);
        }
    }

    int currentGigi = -1;
    float pitchTarget = 0f;
    float motorSpeedNormalized = 0f;
    void UpdateSound(){
        motorSpeedNormalized = rigidbody.velocity.sqrMagnitude / topSpeed;
        if(motorSpeedNormalized < 0.25f){
            pitchTarget = Mathf.Lerp(0.08f, 1.2f,motorSpeedNormalized);
        }else if(motorSpeedNormalized < 0.5f){
            pitchTarget = Mathf.Lerp(0.08f, 1.2f,motorSpeedNormalized - 0.2f);
        }else if(motorSpeedNormalized < .75f){
            pitchTarget = Mathf.Lerp(0.08f, 1.2f,motorSpeedNormalized - 0.4f);
        }else if(motorSpeedNormalized <= 1f){
            pitchTarget = Mathf.Lerp(0.08f, 1.2f,motorSpeedNormalized - 0.6f);
        }
        // Debug.Log(motorSpeedNormalized);
        // audioSource.volume = Mathf.Clamp(audioSource.volume + 0.08f, 0.1f, 1f);
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, pitchTarget, Time.deltaTime * 4f);
    }
    

    private void HandleMotor(){
        if((rigidbody.velocity.sqrMagnitude < topSpeed && BanDepanKanan.rpm > 0) || (rigidbody.velocity.sqrMagnitude < topReverseSpeed && BanDepanKanan.rpm < 0)){
            if((y_input < -0.1f && BanDepanKanan.rpm > 1f) || (y_input > 0.1f && BanDepanKanan.rpm < -1f)){
                BanBelakangKanan.brakeTorque = breakForce/2f;
                BanBelakangKiri.brakeTorque = breakForce/2f;
            }else{
                BanBelakangKanan.motorTorque = y_input * motorForce;
                BanBelakangKiri.motorTorque =  y_input * motorForce;
                BanBelakangKanan.brakeTorque = 0f;
                BanBelakangKiri.brakeTorque = 0f;
            }
        }else{
            BanBelakangKanan.motorTorque = 0;
            BanBelakangKiri.motorTorque =  0;
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
            SoundSystem.PlaySounds(SoundSystem.Tracks.Belok);
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
            currspeed = BanDepanKanan.steerAngle;
    }

    private void changeWheelRotation(WheelCollider wheelCollider, Transform wheelTransform){
        Vector3 pos;
        Quaternion quat;
        wheelCollider.GetWorldPose(out pos, out quat);

        wheelTransform.rotation = quat;
        wheelTransform.position = pos;
    }
    private void OnCollisionEnter(Collision other) {
        if(!other.gameObject.CompareTag("Player") || !other.gameObject.CompareTag("Floor")){
            if(motorSpeedNormalized > .5f)
                SoundSystem.PlaySounds(SoundSystem.Tracks.TabrakBerat, 0.5f);
            else if(motorSpeedNormalized > .25f)
                SoundSystem.PlaySounds(SoundSystem.Tracks.TabrakSedang, 0.3f);
            else
                SoundSystem.PlaySounds(SoundSystem.Tracks.TabrakRingan);
        }
    }
}
