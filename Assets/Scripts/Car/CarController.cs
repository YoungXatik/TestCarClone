using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody sphereRigidbody;
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private Transform carTransform;
    [SerializeField] private Transform sphereTransform;
    [SerializeField] private LayerMask groundLayer;

    [Header("Move")]
    private float moveInput;
    [SerializeField] private float trueForwardSpeed;
    [SerializeField] private float trueReverseSpeed;
    [SerializeField] private float maxForwardSpeed;
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
        sphereRigidbody.transform.parent = null;
        carRigidbody.transform.parent = null;
        normalDrag = sphereRigidbody.drag;
        trueForwardSpeed = forwardSpeed;
        trueReverseSpeed = reverseSpeed;
        maxForwardSpeed = forwardSpeed + (forwardSpeed * 0.5f);
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;

        carTransform.position = sphereTransform.position;

        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        carTransform.Rotate(0, newRotation, 0, Space.World);

        RaycastHit hit;
        isCarGrounded = Physics.Raycast(carTransform.position, -carTransform.up, out hit, 1f, groundLayer);

        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        carTransform.rotation = Quaternion.Slerp(carTransform.rotation, toRotateTo, alighToGroundTime * Time.deltaTime);

        sphereRigidbody.drag = isCarGrounded ? normalDrag : modifiedDrag;

        if (Input.GetKey(KeyCode.W))
        {
            inputTimerCounter += Time.deltaTime;
            if (inputTimerCounter >= inputTimer) 
            {
                forwardSpeed += (inputTimerCounter * Time.deltaTime);
                if (forwardSpeed >= maxForwardSpeed)
                {
                    forwardSpeed = maxForwardSpeed;
                }
            }
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            forwardSpeed = trueForwardSpeed;
            inputTimerCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (moveInput >= 0)
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
            forwardSpeed = trueForwardSpeed;
            reverseSpeed = trueReverseSpeed;
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
            sphereRigidbody.AddForce(transform.forward * moveInput,ForceMode.Acceleration);   
        }
        else
        {
            sphereRigidbody.AddForce(transform.up * -fallForce);
        }
        
        carRigidbody.MoveRotation(carTransform.rotation);
    }
}         
