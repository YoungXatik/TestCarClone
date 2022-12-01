using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashProccessing : MonoBehaviour
{
    [SerializeField] private GameObject crashParticle;
    [SerializeField] private float force;
    private Rigidbody propRigidbody;

    private void Start()
    {
        propRigidbody = GetComponent<Rigidbody>();
    }

    private CarController car;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<CarController>(out car))
        {
            crashParticle.SetActive(true);
            propRigidbody.AddForce(transform.up * force);
            propRigidbody.AddForce(transform.forward * force);
        }
    }
}
