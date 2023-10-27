using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using System;

using _Graph;
using _Matrix;
using _Settings;
using _Utils;

// CLasse représentant un joueur.
public class Player : Agent
{
    #region Variables





    /////////////
    /// Monde ///
    /////////////

    // Monde associé au joueur. 
    public GameObject m_world = null;





    ////////////////
    /// Physique ///
    ////////////////

    // Ajoute +1 à chaque fois que la fonction FixedUpdate() est appelée.
    [HideInInspector] public int m_tick = 0;

    // Met à jour le joueur tous les 10 ticks.
    [HideInInspector] public int m_tickMax = 10;

    // Vitesse du joueur.
    [HideInInspector] public Vector2 m_speed = new Vector2(5.0f, 20.0f);





    ////////////
    /// État ///
    ////////////

    [HideInInspector] public bool m_collisionMonster = false;   // Update dans OnTriggerEnter2D().
    [HideInInspector] public bool m_collisionCoin = false;      // Update dans OnTriggerEnter2D().
    [HideInInspector] public bool m_collisionLever = false;     // Update dans OnTriggerEnter2D().
    [HideInInspector] public bool m_collisionExit = false;      // Update dans OnTriggerEnter2D().
    [HideInInspector] public bool m_onGround = false;           // Update dans OnCollisionStay2D().
     
    [HideInInspector] public bool m_outOfDimension = false;     // Update dans UpdateState().
    [HideInInspector] public bool m_timerIsOver = false;        // Update dans UpdateState().
    [HideInInspector] public bool m_isDead = false;             // Update dans UpdateState().





    /////////////
    /// Agent ///
    /////////////

    // Nombre de décisions prises par l'agent.
    [HideInInspector] public int m_step = 0;

    // Nombre de décisions max. prises par l'agent.
    [HideInInspector] public int m_stepMax = 1000;

    // Récompenses.
    [HideInInspector] public float m_rewardMove = -0.1f;
    [HideInInspector] public float m_rewardMonster = -5.0f;             // Monster + Spade + Lava.
    [HideInInspector] public float m_rewardCoin = 1.0f;
    [HideInInspector] public float m_rewardExit = 5.0f;                 // Lever + Exit.
    [HideInInspector] public float m_rewardTimerIsOver = -5.0f;
    [HideInInspector] public float m_rewardOutOfDimension = -10.0f;





    //////////////
    /// Entrée ///
    //////////////

    [HideInInspector] public bool m_left = false;
    [HideInInspector] public bool m_right = false;
    [HideInInspector] public bool m_jump = false;





    #endregion

    #region Functions





    /////////////
    /// Unity ///
    /////////////

    public void Awake()
    {
        World worldSrc = m_world.GetComponent<World>();
        worldSrc.Init();
    }

    public void FixedUpdate()
    {
        m_tick += 1;

        if (m_tick >= m_tickMax)
        {
            m_tick = 0;
            m_step++;
            RequestDecision();
        }
    }





    //////////
    /// IA ///
    //////////

    public override void OnEpisodeBegin()
    {
        // Monde.

        World worldScr = m_world.GetComponent<World>();
        worldScr.DestroyLevel();
        worldScr.m_levelCursor = 0;
        worldScr.CreateLevel();

        // Joueur.

        m_tick = 0;
        ResetState();
        m_step = 0;
        ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        World worldScr = m_world.GetComponent<World>();
        worldScr.MatrixBinUpdate();

        for (int j = 0; j < World.s_matrixSize.y; j++)
        {
            for (int i = 0; i < World.s_matrixSize.x; i++)
            {
                int value = worldScr.m_matrixBin.Get(i, j);

                int[] bin = Utils.IntToBin(value, 3);

                for (int k = 0; k < bin.Length; k++)
                {
                    sensor.AddObservation(bin[k]);
                }
            }
        }
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
        // FixedUpdate();

        // CollectObservations();

        // Heuristic();

        UpdateState();

        UpdateReward();

        if (m_isDead)
        {
            EndEpisode();
            // CollectObservations();
            // OnEpisodeBegin();
            return;
        }

        UpdateWorld();

        int index = actions.DiscreteActions[0];

        ResetInput();

        SetInput(index);

        UpdateVelocity();

        UpdatePosition();

        ResetState();

        // OnTriggerEnter2D();

        // OnCollisionStay2D();
    }





    ////////////
    /// État ///
    ////////////

    public void ResetState()
    {
        m_collisionMonster = false;
        m_collisionCoin = false;
        m_collisionLever = false;
        m_collisionExit = false;
        m_onGround = false;

        m_outOfDimension = false;
        m_timerIsOver = false;
        m_isDead = false;
    }

    public void UpdateState()
    {
        m_outOfDimension = OutOfDimension();
        m_timerIsOver = (m_step >= m_stepMax);
        m_isDead = (m_collisionMonster || m_outOfDimension || m_timerIsOver);
    }





    //////////////////
    /// Récompense ///
    //////////////////

    public void UpdateReward()
    {
        if (m_collisionMonster)
        {
            AddReward(m_rewardMonster);
        }

        if (m_collisionCoin)
        {
            AddReward(m_rewardCoin);
        }

        if (m_collisionLever)
        {
            AddReward(m_rewardExit);
        }

        if (m_collisionExit)
        {
            AddReward(m_rewardExit);
        }

        if (m_outOfDimension)
        {
            AddReward(m_rewardOutOfDimension);
            return; // Inutile de calculer le PCC.
        }

        if (m_timerIsOver)
        {
            AddReward(m_rewardTimerIsOver);
        }

        if (m_isDead)
        {
            World worldScr = m_world.GetComponent<World>();

            // Joueur.

            Vector2Int positionPlayer = GetPositionIJ();

            int iPlayer = positionPlayer.x;
            int jPlayer = positionPlayer.y;

            // Sortie.

            Vector3 positionExit = Vector3.zero;
            
            if (worldScr.m_lever) // Si un levier existe, la sortie sera le levier.
            {
                positionExit = worldScr.m_lever.transform.localPosition;
            }
            else
            {
                positionExit = worldScr.m_exit.transform.localPosition;
            }

            int iExit = (int)(positionExit.x / (float)World.s_tileSize.x);
            int jExit = (int)(positionExit.y / (float)World.s_tileSize.y);

            // PCC.

            float PCC = _Graph.Graph.GetSP(
                worldScr.m_matrixBin.m_matrix,
                World.s_matrixSize.x,
                World.s_matrixSize.y,
                iPlayer,
                jPlayer,
                iExit,
                jExit,
                false
            );

            Debug.Assert(PCC != -1.0f);

            AddReward(-PCC); // pire cas 2x24 + 2x14 = 76.
        }
    }





    /////////////
    /// Monde ///
    /////////////

    public void UpdateWorld()
    {
        World worldScr = m_world.GetComponent<World>();

        if (m_collisionLever)
        {
            worldScr.DestroyDoor();
        }

        if (m_collisionExit)
        {
            worldScr.DestroyLevel();
            worldScr.m_levelCursor++;
            worldScr.CreateLevel();
        }
    }





    ///////////////
    /// Entrée ////
    ///////////////

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
                m_jump = m_onGround;
                break;

            default:
                Debug.Assert(false);
                break;
        }
    }






    /////////////////
    /// Vélocité ////
    /////////////////

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





    /////////////////
    /// Position ////
    /////////////////

    public Vector2 GetPosition()
    {
        return (Vector2)transform.localPosition;
    }

    public Vector2Int GetPositionIJ()
    {
        Vector2 position = GetPosition();

        int i = (int)(position.x / (float)World.s_tileSize.x);
        int j = (int)(position.y / (float)World.s_tileSize.y);

        return (new Vector2Int(i, j));
    }

    public void SetPosition(Vector2 position)
    {
        transform.localPosition = (Vector3)position;
    }

    public void ResetPosition()
    {
        World worldScr = m_world.GetComponent<World>();

        Vector2 positionSpawn = (Vector2)worldScr.m_spawn.transform.localPosition;

        SetPosition(positionSpawn);
    }

    public bool OutOfDimension()
    {
        Vector2Int position = GetPositionIJ();

        int i = position.x;
        int j = position.y;

        return ((i < 0) || (i >= World.s_matrixSize.x) || (j < 0) || (j >= World.s_matrixSize.y));
    }

    public void UpdatePosition()
    {
        if (m_collisionExit)
        {
            ResetPosition();
        }
    }





    //////////////
    /// Unity ////
    //////////////

    public void OnTriggerEnter2D(Collider2D collider)
    {
        m_collisionMonster = false;
        m_collisionCoin = false;
        m_collisionLever = false;
        m_collisionExit = false;

        if (collider.gameObject.CompareTag("tag_monster"))
        {
            m_collisionMonster = true;
        }

        if (collider.gameObject.CompareTag("tag_spade"))
        {
            m_collisionMonster = true;
        }

        if (collider.gameObject.CompareTag("tag_lava"))
        {
            m_collisionMonster = true;
        }

        if (collider.gameObject.CompareTag("tag_coin"))
        {
            m_collisionCoin = true;
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.CompareTag("tag_lever"))
        {
            m_collisionLever = true;
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.CompareTag("tag_exit"))
        {
            m_collisionExit = true;
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
    }





    #endregion
}
