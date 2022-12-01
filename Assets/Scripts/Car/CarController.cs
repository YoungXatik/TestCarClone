using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Components")] 
    private CarUI _carUI;
    [SerializeField] private Rigidbody sphereRigidbody;
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private Transform carTransform;
    [SerializeField] private Transform sphereTransform;
    [SerializeField] private LayerMask groundLayer;

    [Header("Move")]
    private float _moveInput;
    private float _trueForwardSpeed;
    private float _trueReverseSpeed;
    public float forwardSpeed { get; private set; }
    public float reverseSpeed { get; private set; }

    [SerializeField] private float breakSpeed;
    [SerializeField] private bool breaking;

    [Header("Turn")]
    private float turnInput;
    [SerializeField] private float turnSpeed;

    [Header("States")]
    private bool isCarGrounded;
    [SerializeField] private float modifiedDrag;
    private float normalDrag;
    [SerializeField] private float fallForce = 30;

    [SerializeField] private float alighToGroundTime;


    [SerializeField] private GameObject[] smoke;

    public float inputTimerCounter { get; private set; }

    public float inputSpeedTimer = 1;
    public float inputTimer = 3;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _carUI = GetComponent<CarUI>();
        sphereRigidbody.transform.parent = null;
        carRigidbody.transform.parent = null;
        normalDrag = sphereRigidbody.drag;
        _trueForwardSpeed = forwardSpeed;
        _trueReverseSpeed = reverseSpeed;
    }

    private void Breaking()
    {
        if (breaking)
        {
            return;
        }
        else
        {
            breaking = true;
            breakSpeed = forwardSpeed / 180;
        }
    }
    private void Update()
    {
       // _moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        _moveInput *= _moveInput > 0 ? forwardSpeed : reverseSpeed;

        carTransform.position = sphereTransform.position;

        if (_moveInput != 0)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime;
            carTransform.Rotate(0, newRotation, 0, Space.World);
        }

        RaycastHit hit;
        isCarGrounded = Physics.Raycast(carTransform.position, -carTransform.up, out hit, 1f, groundLayer);
        
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        carTransform.rotation = Quaternion.Slerp(carTransform.rotation, toRotateTo, alighToGroundTime * Time.deltaTime);
        

        sphereRigidbody.drag = isCarGrounded ? normalDrag : modifiedDrag;

        _moveInput = forwardSpeed;

        if (Input.GetKey(KeyCode.W))
        {
            breaking = false;
            inputTimerCounter += Time.deltaTime;
            forwardSpeed += (inputTimerCounter * Time.deltaTime) * 2;
            if (inputTimerCounter >= inputSpeedTimer) 
            {
                forwardSpeed += (inputTimerCounter * Time.deltaTime) * 5;
            }
        }
        else
        {
            _moveInput = forwardSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (_moveInput > 0)
            {
                Breaking();
                _moveInput = forwardSpeed;
                forwardSpeed -= breakSpeed;
                if (forwardSpeed <= _trueForwardSpeed)
                {
                    forwardSpeed = _trueForwardSpeed;
                    inputTimerCounter = 0;
                }
            }
            else if(_moveInput <= 0)
            {
                _moveInput = -reverseSpeed;
                inputTimerCounter += Time.deltaTime;
                reverseSpeed += (inputTimerCounter * Time.deltaTime) * 2;
                if (reverseSpeed >= _trueReverseSpeed)
                {
                    reverseSpeed = _trueReverseSpeed;
                    inputTimerCounter = 0;
                }
            }
        }
        _carUI.UpdateUI(_moveInput);

        if (isCarGrounded)
        {
            EnableParticles();
        }
        else
        {
            DisableParticles();
        }
    }

    private void EnableParticles()
    {
        for (int i = 0; i < smoke.Length; i++)
        {
            smoke[i].SetActive(true);
        }
    }

    private void DisableParticles()
    {
        for (int i = 0; i < smoke.Length; i++)
        { 
            smoke[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isCarGrounded)
        {
            sphereRigidbody.AddForce(transform.forward * _moveInput,ForceMode.Acceleration);   
        }
        else
        {
            sphereRigidbody.AddForce(transform.up * -fallForce);
        }
        
        carRigidbody.MoveRotation(carTransform.rotation);
    }

    public void SpeedBoost(Vector3 forceAngle, float forceValue)
    {
        sphereRigidbody.AddForce(forceAngle * forceValue,ForceMode.Impulse);
    }



}         
