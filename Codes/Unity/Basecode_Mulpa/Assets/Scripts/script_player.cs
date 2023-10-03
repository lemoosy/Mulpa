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

    public bool m_left = false;
    public bool m_right = false;
    public bool m_jump = false;

    // Objets

    public int m_coin = 0;

    // Physique

    public Vector2 m_speed;

    public int m_tick = 0;

    // États

    public bool m_onGround = false;
    public bool m_isDead = false;

    // Pièce

    public bool m_sceneIsUpdated = false;
    public int m_sceneIndex;

    #endregion

    #region Fonctions

    // Unity

    private void FixedUpdate()
    {
        if (m_sceneIsUpdated)
        {
            m_objSpawn = GameObject.Find("obj_spawn");
            
            m_objSelfBody.position = m_objSpawn.transform.position;

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
        
        m_objSelfBody = GetComponent<Rigidbody2D>();
        m_objSelfBody.position = m_objSpawn.transform.position;

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

        // Physique

        m_speed = new Vector2(128.0f, 256.0f);

        // Pièce

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
            int[] matrix = World_GetMatrix();

            DLL_NN_Forward(m_nnIndex, matrix, (int)m_worldSize.x, (int)m_worldSize.y);

            int res = DLL_NN_GetOutput(m_nnIndex);

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
                position = m_objSpawn.transform.position;
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

            if (m_isDead)
            {
                m_score = 10;
            }
        }
    }

    // #################
    // ##### Monde #####
    // #################

    public int[] World_GetMatrix()
    {
        int[] matrix = new int[(int)m_worldSize.x * (int)m_worldSize.y];

        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            float x = obj.transform.position.x + 4.0f;
            float y = obj.transform.position.y + 4.0f;

            int i = (int)(x / m_worldSizeTile.x);
            int j = (int)(y / m_worldSizeTile.y);

            int index = j * (int)m_worldSizeMatrix.x + i;

            if ((index < 0) || (index >= m_worldSizeMatrix.x * m_worldSizeMatrix.y))
            {
                continue;
            }

            switch (obj.tag)
            {
                case "tag_settings":
                    break;

                case "tag_attack":
                    matrix[index] = 1;
                    break;

                case "tag_coin":
                    matrix[index] = 2;
                    break;

                case "tag_player":
                    matrix[index] = 3;
                    break;

                case "tag_exit":
                    matrix[index] = 4;
                    break;

                case "tag_button":
                    matrix[index] = 5;
                    break;

                case "tag_door":
                    matrix[index] = 6;
                    break;

                default:
                    Debug.Assert(false, "ERROR - World_GetMatrix()");
                    break;
            }
        }

        return matrix;
    }

    #endregion
}
