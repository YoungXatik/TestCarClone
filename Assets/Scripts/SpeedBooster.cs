using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    private CarController car;
    [SerializeField] private float speedBoost;
    [SerializeField] private Vector3 forceAngle;
    [SerializeField] private GameObject particle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<CarController>(out car))
        {
            particle.SetActive(true);
            car.SpeedBoost(forceAngle,speedBoost);
        }
    }
}
