using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Drawing;
using System.Net.NetworkInformation;

public class Player : Agent
{
    #region Variables

    // Random
    public System.Random m_randomGenerator = new System.Random(73);

    // Directions
    public bool m_left = false;
    public bool m_right = false;
    public bool m_up = false;
    public bool m_down = false;

    // Physics
    public Vector2 m_speed = new Vector2(1.0f, 1.0f);
    public int m_tick = 0;
    public int m_step = 0;

    // Matrix
    static public Vector2 m_tileSize = new Vector2(1.0f, 1.0f);
    static public Vector2Int m_matrixSize = new Vector2Int(10, 10);
    public int[] m_matrix = new int[m_matrixSize.x * m_matrixSize.y];
    
    // Prefabs
    public GameObject m_coinCopy = null;
    public GameObject m_monsterCopy = null;

    // Objects
    public GameObject m_coin = null;
    public int m_monstersSize = 3;
    public List<GameObject> m_monsters = new List<GameObject>();

    // Parent
    public Transform m_parent = null;

    // States
    public bool m_timeOut = false;
    public bool m_collisionCoin = false;
    public bool m_collisionMonster = false;

    #endregion

    #region Functions

    // Unity

    public void FixedUpdate()
    {
        m_tick++;

        if (m_tick >= 300)
        {
            m_tick = 0;
            m_step++;

            if (m_step >= 30) // >30 étapes ? On tue l'agent.
            {
                m_timeOut = true;
            }

            RequestDecision();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("tag_coin"))
        {
            m_collisionCoin = true;
        }

        if (collider.gameObject.CompareTag("tag_monster"))
        {
            m_collisionMonster = true;
        }
    }

    // Input

    public void ResetInput()
    {
        m_left = false;
        m_right = false;
        m_up = false;
        m_down = false;
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
                m_up = true;
                break;

            case 4:
                m_down = true;
                break;

            default:
                print("ERROR - SetInput()");
                break;
        }
    }

    // Matrix

    public void UpdateMatrix()
    {
        Array.Clear(m_matrix, 0, m_matrix.Length);

        // Player

        Vector3 position = transform.localPosition;

        int i = (int)position.x;
        int j = (int)position.y;

        int index = j * m_matrixSize.x + i;

        if (!OutOfDimension())
        {
            m_matrix[index] = 1;
        }

        // Coin

        position = m_coin.transform.localPosition;

        i = (int)position.x;
        j = (int)position.y;

        index = j * m_matrixSize.x + i;

        m_matrix[index] = 2;

        // Monsters

        foreach (GameObject obj in m_monsters)
        {
            position = obj.transform.localPosition;

            i = (int)position.x;
            j = (int)position.y;

            index = j * m_matrixSize.x + i;

            m_matrix[index] = 3;
        }
    }

    // Agent

    public override void OnEpisodeBegin()
    {
        m_tick = 0;
        m_step = 0;

        ResetPosition();
        ResetPositionCoin();
        ResetPositionMonsters();

        m_timeOut = false;
        m_collisionCoin = false;
        m_collisionMonster = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        UpdateMatrix();

        for (int k = 0; k < m_matrix.Length; k++)
        {
            switch (m_matrix[k])
            {
                case 0: // void
                    sensor.AddObservation(0.0f);
                    sensor.AddObservation(0.0f);
                    break;

                case 1: // player
                    sensor.AddObservation(0.0f);
                    sensor.AddObservation(1.0f);
                break;

                case 2: // coin
                    sensor.AddObservation(1.0f);
                    sensor.AddObservation(0.0f);
                break;

                case 3: // monster
                    sensor.AddObservation(1.0f);
                    sensor.AddObservation(1.0f);
                break;

                default:
                    print("ERROR - CollectObservations()");
                    break;
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-0.01f);

        if (m_timeOut)
        {
            print(GetCumulativeReward());
            EndEpisode();
            return;
        }

        if (m_collisionCoin)
        {
            AddReward(+3.0f);
            print(GetCumulativeReward());
            EndEpisode();
            return;
        }

        if (m_collisionMonster)
        {
            AddReward(-2.0f);
            print(GetCumulativeReward());
            EndEpisode();
            return;
        }

        int index = actionBuffers.DiscreteActions[0];

        ResetInput();
        SetInput(index);
        UpdatePosition();

        if (OutOfDimension())
        {
            AddReward(-5.0f);
            print(GetCumulativeReward());
            EndEpisode();
            return;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actionBuffers = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionBuffers[0] = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            actionBuffers[0] = 2;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            actionBuffers[0] = 3;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            actionBuffers[0] = 4;
        }
    }

    // Position

    public void ResetPosition()
    {
        int i = m_randomGenerator.Next(0, m_matrixSize.x);
        int j = m_randomGenerator.Next(0, m_matrixSize.y);

        Vector2 position = new Vector2((float)i, (float)j);
        
        transform.localPosition = (Vector3)position;
    }

    public void UpdatePosition()
    {
        Vector3 position = transform.localPosition;

        if (m_left)     position.x -= m_speed.x;
        if (m_right)    position.x += m_speed.x;
        if (m_up)       position.y += m_speed.y;
        if (m_down)     position.y -= m_speed.y;

        transform.localPosition = position;
    }

    public bool OutOfDimension()
    {
        Vector3 position = transform.localPosition;

        if (position.x < 0.0f)
        {
            return true;
        }

        if (position.x >= (float)m_matrixSize.x)
        {
            return true;
        }

        if (position.y < 0.0f)
        {
            return true;
        }

        if (position.y >= (float)m_matrixSize.y)
        {
            return true;
        }

        return false;
    }

    // Coin

    public void ResetPositionCoin()
    {
        if (!m_coin)
        {
            m_coin = Instantiate(m_coinCopy, m_parent);
        }

        Vector2 positionPlayer = transform.localPosition;
        int iPlayer = (int)positionPlayer.x;
        int jPlayer = (int)positionPlayer.y;

        while (true)
        {
            int i = m_randomGenerator.Next(0, m_matrixSize.x);
            int j = m_randomGenerator.Next(0, m_matrixSize.y);

            if ((i == iPlayer) && (j == jPlayer)) continue;

            Vector2 positionCoin = new Vector2((float)i, (float)j);
            
            m_coin.transform.localPosition = (Vector3)positionCoin;

            break;
        }
    }

    // Monsters

    public void ResetPositionMonsters()
    {
        Vector2 positionPlayer = transform.localPosition;
        int iPlayer = (int)positionPlayer.x;
        int jPlayer = (int)positionPlayer.y;

        Vector2 positionCoin = m_coin.transform.localPosition;
        int iCoin = (int)positionCoin.x;
        int jCoin = (int)positionCoin.y;

        // On supprime tous les monstres.

        foreach (GameObject obj in m_monsters)
        {
            Destroy(obj);
        }

        m_monsters.Clear();

        // On crée les monstres.

        for (int k = 0; k < m_monstersSize; k++)
        {
            int iMonster = m_randomGenerator.Next(0, m_matrixSize.x);
            int jMonster = m_randomGenerator.Next(0, m_matrixSize.y);

            if ((iMonster == iPlayer) && (jMonster == jPlayer)) continue;
            if ((iMonster == iCoin) && (jMonster == jCoin)) continue;

            Vector2 positionMonster = new Vector2((float)iMonster, (float)jMonster);

            GameObject obj = Instantiate(m_monsterCopy, m_parent);
            obj.transform.localPosition = (Vector3)positionMonster;

            m_monsters.Add(obj);
        }
    }
    
    #endregion
}
