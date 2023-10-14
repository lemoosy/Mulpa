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

public class Player : Agent
{
    #region Variables

    // AI.
    public bool m_isAI = false;

    // Directions.
    public bool m_left = false;
    public bool m_right = false;
    public bool m_up = false;
    public bool m_down = false;

    // Physics.
    public Vector2 m_speed = new Vector2(0.2f, 0.2f);
    public int m_stepCount = 0;

    // Matrix.
    static public Vector2Int m_matrixSize = new Vector2Int(20, 10);
    public int[] m_matrix = new int[m_matrixSize.x * m_matrixSize.y];

    // Objects.
    public Transform m_parent = null;
    public GameObject m_coinCopy = null;
    public GameObject m_coinCurr = null;

    // Random.
    public static System.Random m_randomGenerator = new System.Random();

    #endregion

    #region Functions

    // Unity.

    public void Start()
    {
        if (m_isAI) return;
         
        ResetAll();
    }

    public void FixedUpdate()
    {
        if (m_isAI) return;
         
        UpdateInput();
        UpdatePosition();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("tag_coin"))
        {
            ResetCoin();

            m_stepCount = 0;

            if (m_isAI)
            {
                AddReward(+1.0f);
            }
        }
    }

    // Update.

    public void UpdateInput()
    {
        m_left = Input.GetKey(KeyCode.LeftArrow);
        m_right = Input.GetKey(KeyCode.RightArrow);
        m_up = Input.GetKey(KeyCode.UpArrow);
        m_down = Input.GetKey(KeyCode.DownArrow);
    }

    public void UpdatePosition()
    {
        Vector2 position = GetPosition();

        if (m_left)     position.x -= m_speed.x;
        if (m_right)    position.x += m_speed.x;
        if (m_up)       position.y += m_speed.y;
        if (m_down)     position.y -= m_speed.y;

        if (position.x < GetW() / 2.0f)
        {
            position.x = GetW() / 2.0f;

            if (m_isAI)
            {
                AddReward(-1.0f);
                EndEpisode();
            }
            else
            {
                ResetAll();
            }
                return;
        }

        if (position.x + GetW() / 2.0f > m_matrixSize.x)
        {
            position.x = m_matrixSize.x - GetW() / 2.0f;

            if (m_isAI)
            {
                AddReward(-1.0f);
                EndEpisode();
            }
            else
            {
                ResetAll();
            }
                return;
        }

        if (position.y < GetH() / 2.0f)
        {
            position.y = GetH() / 2.0f;

            if (m_isAI)
            {
                AddReward(-1.0f);
                EndEpisode();
            }
            else
            {
                ResetAll();
            }
                return;
        }

        if (position.y + GetH() / 2.0f > m_matrixSize.y)
        {
            position.y = m_matrixSize.y - GetH() / 2.0f;

            if (m_isAI)
            {
                AddReward(-1.0f);
                EndEpisode();
            }
            else
            {
                ResetAll();
            }
                return;
        }

        SetPosition(position);
    }

    public void UpdateMatrix()
    {
        Array.Clear(m_matrix, 0, m_matrix.Length);

        // Player.

        Vector2 position = GetPosition();

        float x = position.x;
        float y = position.y;

        int i = (int)x;
        int j = (int)y;

        int index = j * m_matrixSize.x + i;

        m_matrix[index] = 1;

        // Coin.

        if (!m_coinCurr) return;

        position = GetPosition();

        x = position.x;
        y = position.y;

        i = (int)x;
        j = (int)y;

        index = j * m_matrixSize.x + i;

        m_matrix[index] = 2;
    }

    // Physics.

    public float GetW()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        return collider.size.x;
    }

    public float GetH()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        return collider.size.y;
    }

    public Vector2 GetSize()
    {
        return (new Vector2(GetW(), GetH()));
    }

    public Vector2 GetPosition()
    {
        return transform.localPosition;
    }

    public void SetPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

    // ResetAll.

    public void ResetPlayer()
    {
        int i = m_randomGenerator.Next(0, m_matrixSize.x);
        int j = m_randomGenerator.Next(0, m_matrixSize.y);

        Vector2 position = new Vector2(i, j);
        Vector2 size = GetSize();
        position += (size / 2.0f);
        SetPosition(position);
    }

    public void ResetCoin()
    {
        if (m_coinCurr)
        {
            Destroy(m_coinCurr);
        }

        int i = m_randomGenerator.Next(0, m_matrixSize.x);
        int j = m_randomGenerator.Next(0, m_matrixSize.y);

        Vector2 position = new Vector2(i, j);
        Vector2 size = GetSize();
        position += (size / 2.0f);
        m_coinCurr = Instantiate(m_coinCopy, m_parent);
        m_coinCurr.transform.localPosition = position;
    }

    public void ResetAll()
    {
        m_stepCount = 0;
        ResetPlayer();
        ResetCoin();
    }

    // Agent.

    public override void OnEpisodeBegin()
    {
        ResetAll();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        print("in : " + "x=" + transform.localPosition.x.ToString() + " y=" + transform.localPosition.y.ToString() + " x=" + m_coinCurr.transform.localPosition.x.ToString() + " y=" + m_coinCurr.transform.localPosition.y.ToString());
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(m_coinCurr.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float speedX = actionBuffers.ContinuousActions[0];
        float speedY = actionBuffers.ContinuousActions[1];

        print("out : " + speedX.ToString() + " " + speedY.ToString());

        transform.localPosition += new Vector3(speedX, speedY, 0.0f) * 10.0f * Time.deltaTime;

        UpdatePosition();

        m_stepCount++;

        if (m_stepCount / 60 > 10)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }

    #endregion
}
