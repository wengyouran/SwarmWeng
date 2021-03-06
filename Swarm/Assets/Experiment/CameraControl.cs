﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    public Transform[] m_Targets;
    public float m_ScreenEdgeBuffer = 4f;
    public float m_Minsize = 6.5f;

    private float m_DampTime = 0.2f;
    private Camera m_Camera;
    private float m_ZoomSpeed;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }
    void FixedUpdate()
    {
        Move();
        Zoom();
    }
    void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            averagePos += m_Targets[i].position;
            numTargets++;
        }
        if (numTargets > 0)
            averagePos = averagePos / numTargets;

        averagePos.y = transform.position.y;
        m_DesiredPosition = averagePos;
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }
    void Move()
    {
        FindAveragePosition();

    }
    float FindDesiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);
        float size = 0;
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }
        size += m_ScreenEdgeBuffer;
        size = Mathf.Max(size, m_Minsize);
        return size;
    }
    void Zoom()
    {
        float requiredSize = FindDesiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }
    public void SetStartPositionNSize()
    {
        FindAveragePosition();
        transform.position = m_DesiredPosition;
        m_Camera.orthographicSize = FindDesiredSize();

    }

}

