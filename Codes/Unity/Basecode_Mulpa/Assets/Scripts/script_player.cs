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

    public bool m_isINIT = true;

    #region Variables -> Unity

    public GameObject m_object;
    public Rigidbody2D m_body;

    #endregion

    #region Variables -> IA

    public bool m_IA = false;
    public int m_id;

    #endregion

    #region Variables -> Monde

    public Vector2 m_worldSizeTile;
    public Vector2 m_worldSizeMatrix;
    public Vector2 m_worldSize;
    public int[,] m_worldMatrix;
    public string m_worldString;

    #endregion

    #region Variables -> Entrées

    public bool m_left;
    public bool m_right;
    public bool m_jump;

    #endregion

    #region Variables -> Physiques

    public Vector2 m_speed;

   #endregion

    #region Variables -> Physiques -> Collisions
    #endregion

    #region Variables -> États

    public bool m_onGround;
    public bool m_isDead;

    #endregion

    #endregion

    #region Fonctions

    #region Fonctions -> Unity

    private void Start()
    {
        if (m_IA)
        {
            Time.timeScale = 5;
        }

        m_body = GetComponent<Rigidbody2D>();

        m_worldSizeTile = new Vector2(16.0f, 16.0f);
        m_worldSizeMatrix = new Vector2(18, 10);
        m_worldSize = m_worldSizeTile * m_worldSizeMatrix;

        m_left = false;
        m_right = false;
        m_jump = false;

        m_speed = new Vector2(128.0f, 256.0f);

        m_onGround = false;
        m_isDead = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BoxWall"))
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y > 0.9f)
                {
                    m_onGround = true;
                }
            }

        }
    }

    private void Update()
    {
        if (m_isINIT) return;

        // Player.
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateState();

        // World.
        World_InitMatrix();
        World_InitString();
    }

    #endregion

    #region Fonctions -> Player

    private void UpdateInput() 
    {
        if (m_IA)
        {
            NN_Forward(m_id, m_worldString);

            //int res = NN_GetOutput(m_id);

            //m_left = false;
            //m_right = false;
            //m_jump = false;

            //switch (res)
            //{
            //    case 0:
            //        m_left = true;
            //        break;

            //    case 1:
            //        m_right = true;
            //        break;

            //    case 2:
            //        m_jump = m_onGround;
            //        break;

            //    default:
            //        break;
            //}
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
            Vector2 position = new Vector2(10, 20);
            Instantiate(m_object, position, Quaternion.identity);

            velocity.y = m_speed.y;
        }
        
        m_body.velocity = velocity;
    }

    private void UpdatePosition()
    {
        Vector2 position = m_body.position;

        m_body.position = position;
    }

    private void UpdateState()
    {
        m_onGround = false;

        //if (m_body.position.y <= -(m_worldSize.y / 2.0f))
        //{
        //    m_isDead = true;
        //}

        if (m_isDead)
        {
            NN_UpdateScore(m_id, m_worldString);
        }
    }

    #endregion

    #region Functions -> World

    public void World_InitMatrix()
    {

    }

    public string World_InitString()
    {
        return "";
    }

    #endregion

    public float GetScore()
    {
        return NN_GetScore(m_id);
    }

    #endregion
}
