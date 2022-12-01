using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEntire : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private void OnCollisionEnter(Collision other)
    {
        particle.SetActive(true);
        particle.transform.position = gameObject.transform.position;
    }
}
