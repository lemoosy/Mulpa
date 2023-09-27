using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class player : MonoBehaviour
{
    #region DLL

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_Init();

    [DllImport("Basecode_DLL.dll")]
    private static extern int NN_Create();

    #endregion

    #region Variables

    #region Variables -> Monde

    private Vector2 m_worldSizeTile;
    private Vector2 m_worldSizeMatrix;
    private Vector2 m_worldSize;

    #endregion

    #region Variables -> Entrées

    private bool m_left;
    private bool m_right;
    private bool m_jump;

    #endregion

    #region Variables -> Physiques

    private Rigidbody2D body;

    private Vector2 m_speed;

   #endregion

    #region Variables -> Physiques -> Collision
    #endregion

    #region Variables -> États

    private bool m_onGround;
    private bool m_isDead;

    #endregion

    #region Variables -> IA

    private int m_nnID = -1;

    #endregion

#endregion

    #region Fonctions

    #region Fonctions -> Unity

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();

        m_worldSizeTile = new Vector2(16.0f, 16.0f);
        m_worldSizeMatrix = new Vector2(18, 10);
        m_worldSize = m_worldSizeTile * m_worldSizeMatrix;

        m_left = false;
        m_right = false;
        m_jump = false;

        m_speed = new Vector2(128.0f, 256.0f);

        m_onGround = false;
        m_isDead = false;

        DLL_Init();
        m_nnID = NN_Create();
        m_nnID = NN_Create();

        print(m_nnID);
        print(m_nnID);
        print(m_nnID);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BoxWall"))
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                // Dessiner la normale en rouge
                Debug.DrawRay(point.point, point.normal, Color.red, 2.0f);

                // Si la normale est proche de (0, -1), cela signifie que le joueur est en collision par le haut.
                if (point.normal.y > 0.9f) // Utilisez 0.9f comme seuil, ajustez selon vos besoins.
                {
                    //Debug.Log("Collision par le haut !");
                    // Faites ce que vous avez besoin de faire ici
                }
            }

            m_onGround = true;
        }
    }

    private void Update()
    {
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateState();
    }

    #endregion

    #region Fonctions -> Player

    private void UpdateInput() 
    {
        m_left = Input.GetKey(KeyCode.LeftArrow);
        m_right = Input.GetKey(KeyCode.RightArrow);
        m_jump = Input.GetKey(KeyCode.UpArrow) && m_onGround;
    }
    
    private void UpdateVelocity()
    {
        Vector2 velocity = body.velocity;
    
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
        
        body.velocity = velocity;
    }

    private void UpdatePosition()
    {
        Vector2 position = body.position;

        body.position = position;
    }

    private void UpdateState()
    {
        m_onGround = false;
    }

    #endregion

    #endregion
}
