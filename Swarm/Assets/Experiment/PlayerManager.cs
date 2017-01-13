using UnityEngine;
using System.Collections;
[System.Serializable]
public class PlayerManager
{

    public int m_PlayerNumber = 1;
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    public string m_ColorPlayerText;

    private PlayerMovement m_Movement;
    private PlayerAction m_Action;

    public GameObject m_Instance;

    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<PlayerMovement>();
        m_Action = m_Instance.GetComponent<PlayerAction>();
        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Action.m_PlayerNumber = m_PlayerNumber;
        m_ColorPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER" + m_PlayerNumber + "</color>";
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
