using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

using _DLL;
using _World;

public class Player : MonoBehaviour
{



    #region Variables



    // #######################
    // ##### à définir ! #####
    // #######################

    // Monde auxquels le joueur appartient.
    public GameObject m_world = null;

    // Permet de savoir si le joueur est une IA.
    public bool m_isAI = false;

    // Index de la population.
    public int m_populationIndex = -1;

    // #######################
    // ##### à définir ! #####
    // #######################



    // Score du joueur.
    public float m_score = 0.0f;



    // Entrées du joueur.
    public bool m_left = false;
    public bool m_right = false;
    public bool m_jump = false;



    // Nombre de pièces ramassées.
    public int m_coin = 0;



    // Ajoute +1 à chaque FixedUpdate().
    private int m_tick = 0;



    // Vitesses du joueur.
    public const float m_speedX = 128.0f;
    public const float m_speedY = 256.0f;



    // États du joueur.
    public bool m_onGround = false;
    public bool m_isDead = false;
    public bool m_atExit = false;



    #endregion



    #region Fonctions



    private void FixedUpdate()
    {
        m_tick++;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("tag_coin"))
        {
            m_coin++;
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.CompareTag("tag_exit"))
        {
            m_atExit = true;
        }
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

    private void Start()
    {
        Debug.Assert(m_world, "ERROR (1) - Player::Start()");

        if (m_isAI)
        {
            Debug.Assert(m_populationIndex != -1, "ERROR (2) - Player::Start()");
        }
    }

    private void Update()
    {
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateState();
        UpdateAI();
    }
    


    private void UpdateInput()
    {
        m_left = false;
        m_right = false;
        m_jump = false;

        if (m_isAI)
        {
            World worldScr = m_world.GetComponent<World>();

            DLL.DLL_PG_Forward(m_populationIndex, worldScr.m_matrixForNN, World.m_w, World.m_h);

            int res = DLL.DLL_PG_GetOutput(m_populationIndex);

            switch (res)
            {
                case 0:
                    m_left = true;
                    break;

                case 1:
                    m_right = true;
                    break;

                case 2:
                    m_jump = m_onGround;
                    break;

                default:
                    Debug.Assert(false, "ERROR - Player::UpdateInput()");
                    break;
            }
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
        Vector2 velocity = GetVelocity();
    
        if (m_left)
        {
            velocity.x = -m_speedX;
        }
    
        if (m_right)
        {
            velocity.x = +m_speedX;
        }
    
        if (!m_left && !m_right)
        {
            velocity.x *= 0.9f;
        }
    
        if (m_jump)
        {
            velocity.y = m_speedY;
        }

        SetVelocity(velocity);
    }

    private void UpdatePosition()
    {
        if (m_isDead)
        {
            if (m_isAI)
            {
                // World.cs s'en occupe.
            }
            else
            {
                ResetPosition();
            }
        }
    }

    private void UpdateState()
    {
        m_onGround = false;
        m_isDead = false;

        if (OutOfDimenseion())
        {
            m_isDead = true;
        }

        if (m_isAI)
        {
            if ((float)m_tick / 50.0f > 5.0f) // 5 secondes
            {
                m_isDead = true;
            }
        }
    }

    private void UpdateAI()
    {
        if (m_isAI)
        {
            if (m_isDead)
            {
                World worldScr = m_world.GetComponent<World>();

                // Fitness = PCC - 2 x Pièces - 100 x Niveaux

                float PCC = DLL.DLL_PCC(
                    worldScr.m_matrixForPCC,
                    World.m_w,
                    World.m_h,
                    worldScr.m_playerPositionI,
                    worldScr.m_playerPositionJ,
                    worldScr.m_exitPositionI,
                    worldScr.m_exitPositionJ,
                    false
                );

                // Si le joueur est hors dimension.
                if (PCC == -1.0f)
                {
                    m_score = 1000.0f;
                }
                else
                {
                    m_score = PCC - 5.0f * (float)m_coin - 100.0f * (float)(worldScr.m_levelsCursor);
                }
            }
        }
    }



    public float GetWidth()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        return collider.size.x;
    }

    public float GetHeight()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        return collider.size.y;
    }



    public Vector2 GetVelocity()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        return body.velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.velocity = velocity;
    }



    public Vector2 GetPosition()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        
        return body.position;
    }

    public void SetPosition(Vector2 position)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.position = position;
    }

    public bool OutOfDimenseion()
    {
        float w = GetWidth();
        float h = GetHeight();

        if ((position.y - h) < -(World.m_h / 2.0f))
        {
            m_isDead = true;
        }

        if (m_isAI)
        {
            if ((float)m_tick / 50.0f > 5.0f) // 5 secondes
            {
                m_isDead = true;
            }
        }

        return false;
    }

    public void ResetPosition()
    {
        GameObject obj = GameObject.Find("obj_spawn");

        Debug.Assert(obj != null, "ERROR - ResetPosition()");

        SetPosition(obj.transform.position);
    }



    #endregion



}
