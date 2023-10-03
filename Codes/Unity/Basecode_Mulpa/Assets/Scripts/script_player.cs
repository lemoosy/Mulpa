using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    #region DLL

    [DllImport("Basecode_DLL.dll")]
    private static extern bool DLL_NN_Forward(int p_nnIndex, int[] p_world, int p_w, int p_h);

    [DllImport("Basecode_DLL.dll")]
    private static extern int DLL_NN_GetOutput(int p_nnIndex);

    [DllImport("Basecode_DLL.dll")]
    private static extern float DLL_World_GetShortestPath(int[] p_world, int p_w, int p_h);

    #endregion

    #region Variables

    // Unity

    public GameObject m_objSpawn;
    public Rigidbody2D m_objSpawnBody;

    public Rigidbody2D m_objSelfBody;

    // Monde

    public Vector2 m_worldSizeTile;
    public Vector2 m_worldSizeMatrix;
    public Vector2 m_worldSize;

    // IA

    public bool m_isIA = false;
    public int m_nnIndex;
    public float m_score;

    // Entrées

    public bool m_left;
    public bool m_right;
    public bool m_jump;

    // Objets

    public int m_coin;

    // Physique

    public Vector2 m_speed;

    public int m_tick = 0;

    // États

    public bool m_onGround;
    public bool m_isDead;

    // Pièce

    public bool m_sceneIsUpdated;
    public int m_sceneIndex;

    #endregion

    #region Fonctions

    // Unity

    private void FixedUpdate()
    {
        if (m_sceneIsUpdated)
        {
            m_objSpawn = GameObject.Find("obj_spawn");
            m_objSpawnBody = m_objSpawn.GetComponent<Rigidbody2D>();
            
            m_objSelfBody.position = m_objSpawnBody.position;

            m_tick = 0;
         
            m_sceneIsUpdated = false;
        }
        else
        {
            m_tick++;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Unity

        m_objSpawn = GameObject.Find("obj_spawn");
        m_objSpawnBody = m_objSpawn.GetComponent<Rigidbody2D>();
        
        m_objSelfBody = GetComponent<Rigidbody2D>();
        m_objSelfBody.position = m_objSpawnBody.position;

        // IA

        if (m_isIA)
        {
            //Time.timeScale = 5;
        }

        m_score = 0.0f;

        // Monde

        m_worldSizeTile = new Vector2(16.0f, 16.0f);
        m_worldSizeMatrix = new Vector2(18.0f, 10.0f);
        m_worldSize = m_worldSizeTile * m_worldSizeMatrix;

        // Entrées

        m_left = false;
        m_right = false;
        m_jump = false;

        // Objets

        m_coin = 0;

        // Physique

        m_speed = new Vector2(128.0f, 256.0f);

        // États

        m_onGround = false;
        m_isDead = false;

        // Pièce

        m_sceneIsUpdated = false;
        m_sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
            m_sceneIsUpdated = true;
            m_sceneIndex++;

            SceneManager.LoadScene(m_sceneIndex);
        }

        // tag_button

        // tag_door
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

    private void Update()
    {
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateState();
    }

    // ##################
    // ##### Joueur #####
    // ##################

    private void UpdateInput()
    {
        if (m_isIA)
        {
            //DLL_NN_Forward(m_nnIndex, m_worldMatrix, (int)m_worldSize.x, (int)m_worldSize.y);

            //int res = DLL_NN_GetOutput(m_nnIndex);

            int res = 1;

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
                    Debug.Assert(false, "ERROR - UpdateInput()");
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
        Vector2 velocity = m_objSelfBody.velocity;
    
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

        m_objSelfBody.velocity = velocity;
    }

    private void UpdatePosition()
    {
        Vector2 position = m_objSelfBody.position;

        if (m_isDead)
        {
            if (m_isIA)
            {
                // ...
            }
            else
            {
                position = m_objSpawnBody.position;
            }
        }

        m_objSelfBody.position = position;
    }

    private void UpdateState()
    {
        m_onGround = false;
        m_isDead = false;

        if (m_objSelfBody.position.y < -(m_worldSize.y / 2.0f))
        {
            m_isDead = true;
        }

        if (m_isIA)
        {
            if ((float)m_tick / 50.0f > 5.0f) // 5 secondes
            {
                m_isDead = true;
            }
        }
    }

    // #################
    // ##### Monde #####
    // #################

    public void World_UpdateMatrix()
    {
        //int [,] m_worldMatrix = new int[(int)m_worldSizeMatrix.x * (int)m_worldSizeMatrix.y];

        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            continue;
        }
    }

    #endregion
}
