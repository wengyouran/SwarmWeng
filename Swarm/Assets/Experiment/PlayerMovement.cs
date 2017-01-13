using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody m_Rigidbody;

    public int m_PlayerNumber = 1;
    public int m_speed = 1;
    public int m_TurnSpeed = 1;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private float m_MovementInputValue;
    private float m_TurnInputValue;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }
    void FixedUpdate()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * Time.deltaTime * m_speed;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}
