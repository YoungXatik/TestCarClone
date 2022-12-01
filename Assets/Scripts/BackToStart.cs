using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToStart : MonoBehaviour
{
    private CarController car;
    [SerializeField] private Vector3 startPos;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = startPos;
        if (other.gameObject.GetComponent<CarController>())
        {
            other.gameObject.GetComponent<CarController>().SetSpeedToZero();
        }
    }
}
