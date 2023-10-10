using UnityEngine;

using _DLL;
using _Settings;
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



    public int m_tick = 0; // +1 par frame.
    public const float m_maxSeconds = 5.0f; // max. 5 secondes pour l'IA.

    // Gestionnaire de collisions.
    public GameObject[] m_collisions = new GameObject[(int)Settings.CaseID.CASE_COUNT];

    // Entrées du joueur.
    public bool m_left = false;
    public bool m_right = false;
    public bool m_jump = false;

    // Vitesses du joueur.
    public const float m_speedX = 128.0f;
    public const float m_speedY = 256.0f;

    // Items du joueur.
    public int m_coin = 0;
    
    // États du joueur.
    public bool m_onGround = false;
    public bool m_isDead = false;
    public bool m_atExit = false;

    // Score du joueur.
    public float m_score = 0.0f;



    #endregion



    #region Fonctions



    // Unity.

    private void FixedUpdate()
    {
        m_tick++;

        int size = m_collisions.Length;

        for (int i = 0; i < size; i++)
        {
            m_collisions[i] = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("tag_coin"))
        {
            m_collisions[(int)Settings.CaseID.CASE_COIN] = collider.gameObject;
        }

        if (collider.gameObject.CompareTag("tag_spawn"))
        {
            m_collisions[(int)Settings.CaseID.CASE_SPAWN] = collider.gameObject;
        }

        if (collider.gameObject.CompareTag("tag_exit"))
        {
            m_collisions[(int)Settings.CaseID.CASE_EXIT] = collider.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tag_wall"))
        {
            m_collisions[(int)Settings.CaseID.CASE_WALL] = collision.gameObject;

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
            m_collisions[(int)Settings.CaseID.CASE_ATTACK] = collision.gameObject;
        }
    }

    private void Start()
    {
        Debug.Assert(m_world);

        if (m_isAI)
        {
            Debug.Assert(m_populationIndex != -1);

            if (m_populationIndex > 0)
            {
                //gameObject.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    private void Update()
    {
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateItem();
        UpdateState();
        UpdateAI();
    }
    


    // Input.

    public void ResetInput()
    {
        m_left = false;
        m_right = false;
        m_jump = false;
    }

    public void UpdateInput()
    {
        ResetInput();

        if (m_isAI)
        {
            World worldScr = m_world.GetComponent<World>();

            DLL.DLL_PG_Forward(m_populationIndex, worldScr.m_matrix, World.m_matrixW, World.m_matrixH);

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
                    Debug.Assert(false);
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



    // Velocity.

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

    public void UpdateVelocity()
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



    // Position.

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

    public void ResetPosition()
    {
        World world = m_world.GetComponent<World>();

        int i = world.m_spawnPositionI;
        int j = world.m_spawnPositionJ;

        Debug.Assert((i != -1) && (j != -1));

        Vector2 origin = world.m_origin;
        Vector2 position = new Vector2(i * World.m_tileW, j * World.m_tileH);

        SetPosition(origin + position);
    }

    public void UpdatePosition()
    {
        // World.cs s'en occupe.
    }



    // Item.

    public void UpdateItem()
    {
        if (m_collisions[(int)Settings.CaseID.CASE_COIN])
        {
            m_coin++;
            
            GameObject obj = m_collisions[(int)Settings.CaseID.CASE_COIN];
            Destroy(obj);
        }
    }



    // State.

    public void ResetState()
    {
        m_onGround = false;
        m_isDead = false;
        m_atExit = false;
    }

    private void UpdateState()
    {
        ResetState();

        if (OutOfDimension() || m_collisions[(int)Settings.CaseID.CASE_ATTACK])
        {
            m_isDead = true;
        }

        if (m_isAI)
        {
            if ((float)m_tick / 50.0f > m_maxSeconds)
            {
                m_isDead = true;
            }
        }

        if (m_collisions[(int)Settings.CaseID.CASE_EXIT])
        {
            m_atExit = true;
        }
    }



   // AI.

    private void UpdateAI()
    {
        if (m_isAI)
        {
            if (m_isDead && m_score == 0.0f)
            {
                //World worldScr = m_world.GetComponent<World>();

                //// Fitness = PCC - 2 x Pièces - 100 x Niveaux

                //float PCC = DLL.DLL_PCC(
                //    worldScr.m_matrix,
                //    World.m_matrixW,
                //    World.m_matrixH,
                //    worldScr.m_playerPositionI,
                //    worldScr.m_playerPositionJ,
                //    worldScr.m_exitPositionI,
                //    worldScr.m_exitPositionJ,
                //    false
                //);

                //// Si le joueur est hors dimension.
                //if (PCC == -1.0f)
                //{
                //    m_score = 1000.0f;
                //}
                //else
                //{
                //    m_score = PCC - 5.0f * (float)m_coin - 100.0f * (float)(worldScr.m_levelsCursor);
                //}
            }
        }
    }



    // Collision.

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

    public bool OutOfDimension()
    {
        World worldScr = m_world.GetComponent<World>();

        // Position Gauche-Basse du monde.
        float x0 = worldScr.m_origin.x;
        float y0 = worldScr.m_origin.y;

        // Position Droite-Haute du monde.
        float x1 = x0 + World.m_w;
        float y1 = y0 + World.m_h;

        // Taille du joueur.
        float w = GetWidth();
        float h = GetHeight();

        // Position centrale du joueur.
        float x = GetPosition().x + w / 2.0f;
        float y = GetPosition().y + h / 2.0f;

        return (x < x0 || x > x1 || y < y0 || y > y1);
    }



    #endregion



}
