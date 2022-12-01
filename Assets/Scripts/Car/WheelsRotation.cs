using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsRotation : MonoBehaviour
{
    [SerializeField] private Transform[] wheels;
    [SerializeField] private float wheelsSpeed;

    [SerializeField] private Animator wheelsAnimator;

    private void Update()
    {
        wheelsAnimator.SetFloat("rotation",Input.GetAxis("Horizontal"));
        
        float verticalAxes = Input.GetAxisRaw("Vertical");

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Rotate(Time.deltaTime * verticalAxes * wheelsSpeed,0,0, Space.Self);
        }
    }
}
