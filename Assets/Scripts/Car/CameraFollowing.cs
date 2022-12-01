using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    private CarController _car;

    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private CinemachineTransposer _cameraOffset;

    private void Start()
    {
        _cameraOffset = FindObjectOfType<CinemachineTransposer>();
    }

    private void Update()
    {
        _cameraOffset.m_FollowOffset.x = Input.GetAxis("Horizontal");
    }
}
