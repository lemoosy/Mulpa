using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

using _World;

public class Player : MonoBehaviour
{



    #region DLL

    [DllImport("Basecode_DLL.dll")]
    private static extern bool DLL_NN_Forward(int p_nnIndex, int[] p_world, int p_w, int p_h);

    [DllImport("Basecode_DLL.dll")]
    private static extern int DLL_NN_GetOutput(int p_nnIndex);

    [DllImport("Basecode_DLL.dll")]
    private static extern float DLL_World_GetShortestPath(int[] p_world, int p_w, int p_h);

    [DllImport("Basecode_DLL.dll")]
    private static extern IntPtr DLL_TEST(int[] p_world, int p_w, int p_h);

    #endregion



    #region Variables


    public World m_world = null;


    // Permet de savoir si le joueur est un IA.
    public bool m_isAI = false;

    // Index du réseau de neurones du joueur.
    public int m_populationIndex = -1;
    
    // Score de l'IA.
    public float m_score = 0.0f;



    // Permet de savoir si le joueur se déplace à gauche.
    public bool m_left = false;

    // Permet de savoir si le joueur se déplace à droite.
    public bool m_right = false;

    // Permet de savoir si le joueur saute.
    public bool m_jump = false;



    // Nombre de pièces ramassées.
    public int m_coin = 0;

    // Physique
    private int m_tick = 0;



    // Vitesses du joueur (x = horizontale, y = verticale).
    public Vector2 m_speed = new Vector2(128.0f, 256.0f);



    // Permet de savoir si le joueur est au sol.
    public bool m_onGround = false;

    // Permet de savoir si le joueur est mort.
    public bool m_isDead = false;

    // Permet de savoir si le joueur est à la fin du niveau.
    public bool m_atExit = false;



    #endregion



    #region Fonctions



    // Unity.



    private void FixedUpdate()
    {
        int sceneIndexCurr = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndexCurr != m_sceneIndex)
        {
            ResetPosition();

            m_tick = 0;
         
            m_sceneIndex = sceneIndexCurr;
        }
        else
        {
            

            m_tick++;
        }
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

        if (collider.gameObject.CompareTag("tag_button"))
        {
            // ...
        }

        if (collider.gameObject.CompareTag("tag_door"))
        {
            // ...
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
        if (m_isAI)
        {
            Debug.Assert(m_nnIndex != -1, "ERROR - Player::Start()");


        }



    }

    private void Update()
    {
        UpdateWorld();
        UpdateInput();
        UpdateVelocity();
        UpdatePosition();
        UpdateState();
        UpdateAI();
    }



    // Joueur.



    private void UpdateWorld()
    {
        m_worldMatrix = World_GetMatrix();
    }

    private void UpdateInput()
    {
        if (m_isAI)
        {
            m_left = false;
            m_right = false;
            m_jump = false;

            int w = (int)m_worldSizeMatrix.x;
            int h = (int)m_worldSizeMatrix.y;

            bool _res = DLL_NN_Forward(m_nnIndex, m_worldMatrix, w, h);

            Debug.Assert(_res == false, "ERROR - DLL_NN_Forward()");

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
        Vector2 velocity = GetVelocity();
    
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

        SetVelocity(velocity);
    }

    private void UpdatePosition()
    {
        if (m_isDead)
        {
            if (m_isAI)
            {
                // ...
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

        Vector2 position = GetPosition();

        float w = GetWidth();
        float h = GetHeight();

        if ((position.y - h) < -(m_worldSize.y / 2.0f))
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
            Time.timeScale = m_timeScale;

            if (m_isDead)
            {
                // Fitness = PCC - 2 x Pièces - 100 x Niveaux

                int w = (int)m_worldSizeMatrix.x;
                int h = (int)m_worldSizeMatrix.y;

                //print("Calcule PCC...");

                float PCC = DLL_World_GetShortestPath(m_worldMatrix, w, h);

                if (PCC == -1.0f)
                {
                    PCC = 100.0f;
                }

                //print("PCC=" + PCC);

                Debug.Assert(PCC != -1.0f, "ERROR - DLL_World_GetShortestPath()");

                m_score = PCC - 5.0f * (float)m_coin - 20.0f * (float)(m_sceneIndex - 1);
                
                //print("FIT=" + m_score);
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

    public void ResetPosition()
    {
        GameObject obj = GameObject.Find("obj_spawn");

        Debug.Assert(obj != null, "ERROR - ResetPosition()");

        SetPosition(obj.transform.position);
    }



    // #################
    // ##### Monde #####
    // #################



    public int[] World_GetMatrix()
    {
        int w = (int)m_worldSizeMatrix.x;
        int h = (int)m_worldSizeMatrix.y;

        int[] matrix = new int[w * h];

        GameObject[] ALL = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in ALL)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                continue;
            }

            float x = obj.transform.position.x;
            float y = obj.transform.position.y;

            x += spriteRenderer.size.x / 2.0f;
            y += spriteRenderer.size.y / 2.0f;

            x += (m_worldSize.x / 2.0f);
            y += (m_worldSize.y / 2.0f);

            int i = (int)(x / m_worldSizeTile.x);
            int j = (int)(y / m_worldSizeTile.y);

            if (i < 0)
            {
                continue;
            }

            if (j < 0)
            {
                continue;
            }

            if (i >= w)
            {
                continue;
            }

            if (j >= h)
            {
                continue;
            }

            int index = (j * w) + i;

            switch (obj.tag)
            {
                case "tag_settings":
                    break;

                case "tag_attack":
                    matrix[index] = 2;
                    break;

                case "tag_coin":
                    matrix[index] = 3;
                    break;

                case "tag_player":
                    matrix[index] = 4;
                    break;

                case "tag_exit":
                    matrix[index] = 5;
                    break;

                case "tag_button":
                    matrix[index] = 6;
                    break;

                case "tag_door":
                    matrix[index] = 7;
                    break;

                default:
                    Debug.Assert(false, "ERROR - World_GetMatrix() - " + obj.tag);
                    break;
            }
        }

        Tilemap walls = GameObject.Find("obj_walls").GetComponent<Tilemap>();

        if (!walls)
        {
            return matrix;
        }

        foreach (Vector3Int position in walls.cellBounds.allPositionsWithin)
        {
            TileBase tile = walls.GetTile(position);

            if (tile)
            {
                int i = position.x;
                int j = position.y;


                i += (w / 2);
                j += (h / 2);

                if (i < 0 || i >= w || j < 0 || j >= h) continue;
                
                int index = (j * w) + i;

                matrix[index] = 1;
            }
        }

        //string res = "";

        //for (int level = 0; level < h; level++)
        //{
        //    res += World_DEBUG_Print(matrix, w, h, level) + "\n";
        //}

        //print(res);

        return matrix;
    }


    private string World_DEBUG_Print(int[] matrix, int w, int h, int level)
    {
        //string res = "LEVEL (" + j + ") >> ";

        //IntPtr tab = DLL_TEST(matrix, w, h);

        //int[] result = new int[w * h * 7];

        //Marshal.Copy(tab, result, 0, w * h * 7);

        //int block = w * h;

        //for (int i = 0; i < w * h; i++)
        //{
        //    res += result[i + level * block].ToString();
        //}

        //return res;

        return "";
    }



    #endregion



}
