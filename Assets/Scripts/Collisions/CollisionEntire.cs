using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEntire : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private CarController car;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<CarController>(out car))
        {
            particle.SetActive(true);
            particle.transform.position = gameObject.transform.position;
        }
    }
    
}
