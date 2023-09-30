using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class player : MonoBehaviour
{
    #region DLL

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_Forward(int p_id, string p_world);

    [DllImport("Basecode_DLL.dll")]
    private static extern int NN_GetOutput(int p_id);

    [DllImport("Basecode_DLL.dll")]
    private static extern float NN_GetScore(int p_id);

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_SetScore(int p_id, float m_score);

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_UpdateScore(int p_id, string p_world);

    #endregion

    #region Variables

    // Unity

    public GameObject m_object;
    public Rigidbody2D m_body;

    // IA

    public bool m_isIA;
    public int m_id;
    public int m_coin;

    // Monde

    public Vector2 m_worldSizeTile;
    public Vector2 m_worldSizeMatrix;
    public Vector2 m_worldSize;         
    public int[,] m_worldMatrix;
    public string m_worldString;

    // Entrées

    public bool m_left;
    public bool m_right;
    public bool m_jump;

    // Physique

    public Vector2 m_speed;
    public Vector2 m_positionStart;

    // États

    public bool m_onGround;
    public bool m_isDead;

    #endregion

    #region Fonctions

    // Unity

    private void Start()
    {
        // Unity

        m_body = GetComponent<Rigidbody2D>();

        // IA

        if (m_isIA)
        {
            Time.timeScale = 5;
        }

        m_coin = 0;

        // Monde

        m_worldSizeTile = new Vector2(16.0f, 16.0f);
        m_worldSizeMatrix = new Vector2(18, 10);
        m_worldSize = m_worldSizeTile * m_worldSizeMatrix;

        // Entrées

        m_left = false;
        m_right = false;
        m_jump = false;

        // Physique

        m_speed = new Vector2(128.0f, 256.0f);
        m_positionStart = m_body.position;

        // États

        m_onGround = false;
        m_isDead = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tag_wall"))
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y > 0.9f)
                {
                    m_onGround = true;
                }
            }
        }

        if (collision.gameObject.CompareTag("tag_attack"))
        {
            m_isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("tag_coin"))
        {
            m_coin++;
            Destroy(collider.gameObject); 
        }
    }

    private void Update()
    {
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateState();
    }

    // Player

    private void UpdateInput()
    {
        if (m_isIA)
        {
            // ...
        }
        else
        {
            m_left = Input.GetKey(KeyCode.LeftArrow);
            m_right = Input.GetKey(KeyCode.RightArrow);
            m_jump = Input.GetKey(KeyCode.UpArrow) && m_onGround;
        }
    }
    
    private void UpdateVelocity()
    {
        Vector2 velocity = m_body.velocity;
    
        if (m_left)
        {
            velocity.x = -m_speed.x;
        }
    
        if (m_right)
        {
            velocity.x = +m_speed.x;
        }
    
        if (!m_left && !m_right)
        {
            velocity.x *= 0.9f;
        }
    
        if (m_jump)
        {
            velocity.y = m_speed.y;
        }
        
        m_body.velocity = velocity;
    }

    private void UpdatePosition()
    {
        Vector2 position = m_body.position;

        if (m_isDead)
        {
            position = m_positionStart;
        }

        m_body.position = position;
    }

    private void UpdateState()
    {
        m_onGround = false;
        m_isDead = false;

        if (m_body.position.y <= -(m_worldSize.y / 2.0f))
        {
            m_isDead = true;
        }
    }

    // World

    public int[,] World_InitMatrix()
    {
        return new int[0, 0];
    }

    public string World_InitString()
    {
        return "";
    }

    #endregion
}
