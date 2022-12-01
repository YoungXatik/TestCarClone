using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsRotation : MonoBehaviour
{
    [SerializeField] private Transform[] wheels;
    [SerializeField] private float wheelsSpeed;
    private CarController _car;
    [SerializeField] private Animator wheelsAnimator;

    private void Awake()
    {
        _car = GetComponent<CarController>();
    }

    private void Update()
    {
        wheelsAnimator.SetFloat("rotation", Input.GetAxis("Horizontal"));

        if (_car.forwardSpeed != 0 || _car.reverseSpeed != 0)
        {
            float verticalAxes = Input.GetAxisRaw("Vertical");
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].Rotate(Time.deltaTime * 100 * wheelsSpeed, 0, 0, Space.Self);
            }
        }
    }
}
