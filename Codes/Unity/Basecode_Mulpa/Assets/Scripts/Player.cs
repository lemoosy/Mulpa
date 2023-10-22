using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;

using _DLL;
using _Settings;
using System;

public class Player : Agent
{
    #region Variables

    // Monde associé au joueur. 
    public GameObject m_world = null;                       // Référence vers le monde

    // Physique
    private int m_tick = 0;                                 // Ajoute +1 à chaque FixedUpdate()
    private int m_tickMax = 15;                             // MAJ du joueur toutes les 'm_tickMax' frames
    
    // IA
    private int m_step = 0;                                 // Nombre de décisions + actions prises par l'IA
    private int m_stepMax = 1000;                           // Nombre de décisions + actions maximum prises par l'IA

    // Entrées
    public bool m_left = false;
    public bool m_right = false;
    public bool m_jump = false;

    // Vitesse du joueur
    public Vector2 m_speed = new Vector2(128.0f, 256.0f);

    // Récompenses
    public float m_rewardCoin = 1.0f;                       // Récompense lorsque l'agent récupère une pièce
    public float m_rewardMonster = -5.0f;                   // Récompense lorsque l'agent se fait toucher par un monstre
    public float m_rewardOutOfDimension = -5.0f;            // Récompense lorsque le joueur sort de la dimension du monde
    public float m_rewardExit = 5.0f;                       // Récompense lorsque le joueur atteint la sortie
    public float m_rewardTotal = 0.0f;                      // Récompense totale de l'agent

    // États
    public bool m_onGround = false;
    public bool m_isDead = false;
    public bool m_atExit = false;

    #region

    #region Functions

    public void FixedUpdate()
    {
        m_tick += 1;

        if (m_tick >= m_tickMax)
        {
            m_tick = 0;

            m_step++;

            ResetState();
            RequestDecision();
            RequestAction();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("tag_coin"))
        {
            Destroy(collider.gameObject);
            AddReward2(m_rewardCoin);
        }

        if (collider.gameObject.CompareTag("tag_exit"))
        {
            m_atExit = true;
            AddReward2(m_rewardExit);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
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
        
        print("wow 2");

        if (collision.gameObject.CompareTag("tag_monster"))
        {
            m_isDead = true;
            AddReward2(m_rewardMonster);
        }
    }

    // État

    public void ResetState()
    {
        print("wow 1");
        m_onGround = false;
        m_isDead = false;
        m_atExit = false;
    }

    public void UpdateState()
    {
        if (m_step >= m_stepMax)
        {
            m_isDead = true;
        }

        if (OutOfDimension())
        {
            m_isDead = true;
        }
    }

    // Récompense

    public void SetReward2(float p_value)
    {
        SetReward(p_value);
        m_rewardTotal = p_value;
    }

    public void AddReward2(float p_value)
    {
        AddReward(p_value);
        m_rewardTotal += p_value;
    }

    public void ResetReward()
    {
        SetReward2(0.0f);
    }

    public void UpdateReward()
    {
        if (m_isDead)
        {
            World worldScr = m_world.GetComponent<World>();

            float PCC = DLL.DLL_PCC(
                worldScr.m_matrix,
                World.m_matrixSize.x,
                World.m_matrixSize.y,
                worldScr.m_matrixPlayerPosition.x,
                worldScr.m_matrixPlayerPosition.y,
                worldScr.m_matrixExitPosition.x,
                worldScr.m_matrixExitPosition.y,
                false
            );

            // Si le joueur est hors dimension
            if (PCC == -1.0f)
            {
                AddReward2(m_rewardOutOfDimension);
            }
            else
            {
                AddReward2(100.0f - PCC);
            }
        }
    }

    // Entrées

    public void ResetInput()
    {
        m_left = false;
        m_right = false;
        m_jump = false;
    }

    public void SetInput(int index)
    {
        switch (index)
        {
            case 0:
                break;

            case 1:
                m_left = true;
                break;

            case 2:
                m_right = true;
                break;

            case 3:
                print("wow 3" + m_onGround.ToString());
                m_jump = m_onGround;
                break;

            default:
                Debug.Assert(false);
                break;
        }
    }

    // Vélocité

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

    public void ResetVelocity()
    {
        SetVelocity(Vector2.zero);
    }

    public void UpdateVelocity()
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

    // Position

    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }

    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void ResetPosition()
    {
        World world = m_world.GetComponent<World>();

        Debug.Assert(world.m_matrixPlayerPosition.x != -1);

        Vector3 position = (Vector3Int)(world.m_matrixPlayerPosition * World.m_tileSize);

        SetPosition(position);
    }

    public bool OutOfDimension()
    {
        return false;
    }

   // IA

    public override void OnEpisodeBegin()
    {
        m_tick = 0;

        m_step = 0;

        m_rewardTotal = 0.0f;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actions[0] = 1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            actions[0] = 2;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            actions[0] = 3;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // ResetState()
        // FixedUpdate()
        // OnTriggerEnter2D()
        // OnCollisionStay2D()
        UpdateState();
        UpdateReward();
        int index = actions.DiscreteActions[0];
        ResetInput();
        SetInput(index);
        UpdateVelocity();
    }

    // Outils

    public Vector2 GetSize()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        return collider.size;
    }

    public void InitNewLevel()
    {
        World worldScr = m_world.GetComponent<World>();

        m_tick = 0;

        m_step = 0;

        Vector2Int matrixPositionSpawn = worldScr.m_matrixSpawnPosition;
        matrixPositionSpawn *= World.m_tileSize;

        SetPosition((Vector3Int)matrixPositionSpawn);
    }

    #endregion
}
