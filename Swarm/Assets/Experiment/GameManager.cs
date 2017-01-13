using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject m_PlayerPrefab;
    //public DroneManager[] m_Drones;
    public PlayerManager[] m_Player;
    public GameObject[] m_SpawnPoint;
    public CameraControl m_CameraControl;


    // Use this for initialization
    void Start () {
        SpawnPlayer();
   //     SpawnDrone();
        SetCameraTargets();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    void SpawnDrone()
    {
        for (int i = 0; i < m_Drone1.Length; i++)
        {
            m_Drone1[i].m_Instance = Instantiate(m_DronePrefab, m_SpawnPoint[i].transform.position, m_SpawnPoint[i].transform.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].m_SpawnPoint = m_SpawnPoint[i].transform;
            m_Tanks[i].Setup();
        }
        for (int i = 0; i < m_Drone2.Length; i++)
        {
            m_Tanks[i].m_Instance = Instantiate(m_DronePrefab, m_SpawnPoint[i].transform.position, m_SpawnPoint[i].transform.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].m_SpawnPoint = m_SpawnPoint[i].transform;
            m_Tanks[i].Setup();
        }
    }
    */
    void SpawnPlayer()
    {
        for (int i = 0; i < m_Player.Length; i++)
        {
            m_Player[i].m_Instance = Instantiate(m_PlayerPrefab, m_SpawnPoint[i].transform.position, m_SpawnPoint[i].transform.rotation) as GameObject;
            m_Player[i].m_PlayerNumber = i + 1;
            m_Player[i].m_SpawnPoint = m_SpawnPoint[i].transform;
            m_Player[i].Setup();

        }
    }

        void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Player.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Player[i].m_Instance.transform;
        }
        m_CameraControl.m_Targets = targets;
    }
}
