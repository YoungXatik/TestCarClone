using System;
using System.Collections;
using System.Collections.Generic;
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
    private float _maxForwardSpeed;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float reverseSpeed;

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
    public float inputTimer = 3;
    
    private void Start()
    {
        _carUI = GetComponent<CarUI>();
        sphereRigidbody.transform.parent = null;
        carRigidbody.transform.parent = null;
        normalDrag = sphereRigidbody.drag;
        _trueForwardSpeed = forwardSpeed;
        _trueReverseSpeed = reverseSpeed;
        _maxForwardSpeed = forwardSpeed * 2;
    }

    private void Update()
    {
        _moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        _moveInput *= _moveInput > 0 ? forwardSpeed : reverseSpeed;

        carTransform.position = sphereTransform.position;

        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        carTransform.Rotate(0, newRotation, 0, Space.World);

        RaycastHit hit;
        isCarGrounded = Physics.Raycast(carTransform.position, -carTransform.up, out hit, 1f, groundLayer);

        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        carTransform.rotation = Quaternion.Slerp(carTransform.rotation, toRotateTo, alighToGroundTime * Time.deltaTime);

        sphereRigidbody.drag = isCarGrounded ? normalDrag : modifiedDrag;

        _carUI.UpdateUI(_moveInput);
        
        if (Input.GetKey(KeyCode.W))
        {
            inputTimerCounter += Time.deltaTime;
            if (inputTimerCounter >= inputTimer) 
            {
                forwardSpeed += (inputTimerCounter * Time.deltaTime) * 2;
                if (forwardSpeed >= _maxForwardSpeed)
                {
                    forwardSpeed = _maxForwardSpeed;
                }
            }
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            forwardSpeed = _trueForwardSpeed;
            inputTimerCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_moveInput >= 0)
            {
                inputTimerCounter += Time.deltaTime;
                if (inputTimerCounter >= inputTimer)
                {
                    reverseSpeed = 0;
                    forwardSpeed = 0;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            forwardSpeed = _trueForwardSpeed;
            reverseSpeed = _trueReverseSpeed;
            inputTimerCounter = 0;
        }
        

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

    public void SpeedBoost(float speedBoostValue)
    {
        StartCoroutine(SpeedBoostCoroutine(speedBoostValue));
    }

    private IEnumerator SpeedBoostCoroutine(float speedBoost)
    {
        forwardSpeed += speedBoost;
        _maxForwardSpeed += speedBoost;
        yield return new WaitForSeconds(2f);
        forwardSpeed -= speedBoost;
        _maxForwardSpeed -= speedBoost;
    }
    
}         
