using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Unity.MLAgents;

public class PlayerAI : Agent, Player
{
    public int m_step = 0;
    public int m_stepMax = 1000;

    public bool m_isTraining = false;

    float distanceSpadesAndMonsters = 1.0f;

    [HideInInspector] public int m_SP;
    [HideInInspector] public int m_nCases;
    [HideInInspector] public int m_nSpades;
    [HideInInspector] public int m_nMonsters;

    public Matrix m_matrix_nCases = new Matrix(
        LevelInformation.matrixSize.x,
        LevelInformation.matrixSize.y
    );

    public Matrix m_matrix_nSpades = new Matrix(
        LevelInformation.matrixSize.x,
        LevelInformation.matrixSize.y
    );

    public Matrix m_matrix_nMonsters = new Matrix(
        LevelInformation.matrixSize.x,
        LevelInformation.matrixSize.y
    );

    public override void ResetLevel()
    {
        base.ResetLevel();

        m_step = 0;
    }

    public override void Start()
    {
        m_impulse = true;
    }

    public override void Update()
    {
        if (m_resetPosition) return;

        UpdateStates();
        UpdateRewards();
        UpdateFitness();

        if (!m_isDead && !m_atExit)
        {
            RequestDecision();
        }
        else
        {
            if (!m_isTraining)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // States.

    public override void UpdateStates()
    {
        base.UpdateStates();

        if (m_step > m_stepMax)
        {
            m_isDead = true;
        }
    }

    // Rewards.

    public void UpdateRewards()
    {
        if (m_collisionCoin)
        {
            AddReward(+10.0f);
        }

        if (m_collisionLever)
        {
            AddReward(+100.0f);
        }

        if (m_collisionExit)
        {
            AddReward(+100.0f);
        }

        if (m_collisionBonus)
        {
            AddReward(+5.0f);
        }
    }

    // Fitness.

    public void UpdateFitness()
    {
        if (m_isDead || m_atExit)
        {
            Init_SP();
            Init_nCases();
            Init_nSpades();
            Init_nMonsters();
        }
        else
        {
            Update_nCases();
            Update_nSpades();
            Update_nMonsters();
        }
    }

    public void Init_SP()
    {
        if (m_atExit)
        {
            m_SP = 0;
            return;
        }

        // Objects.

        LevelEditor levelEditor = m_objectLevelEditor.GetComponent<LevelEditor>();

        Matrix matrix = levelEditor.GetMatrix();
        
        // Player.

        Vector2Int positionPlayerIJ = GetPositionIJ();

        positionPlayerIJ.x = Mathf.Clamp(positionPlayerIJ.x, 0, LevelInformation.matrixSize.x - 1);
        positionPlayerIJ.y = Mathf.Clamp(positionPlayerIJ.y, 0, LevelInformation.matrixSize.y - 1);

        int value = (int)Graph.CaseID.CASE_VOID;

        matrix.Set1x1(positionPlayerIJ.x, positionPlayerIJ.y, value);

        // SP.

        float SP_1 = 0.0f;
        float SP_2 = 0.0f;

        if (levelEditor.m_lever)
        {
            SP_1 = Graph.GetSP(
                matrix.m_matrix,
                LevelInformation.matrixSize.x,
                LevelInformation.matrixSize.y,
                positionPlayerIJ.x,
                positionPlayerIJ.y,
                (int)levelEditor.m_lever.transform.localPosition.x,
                (int)levelEditor.m_lever.transform.localPosition.y,
                true,
                10.0f
            );

            Debug.Assert(SP_1 != -1.0f);

            SP_2 = Graph.GetSP(
                matrix.m_matrix,
                LevelInformation.matrixSize.x,
                LevelInformation.matrixSize.y,
                (int)levelEditor.m_lever.transform.localPosition.x,
                (int)levelEditor.m_lever.transform.localPosition.y,
                (int)levelEditor.m_exit.transform.localPosition.x,
                (int)levelEditor.m_exit.transform.localPosition.y,
                true,
                10.0f
            );

            Debug.Assert(SP_2 != -1.0f);
        }
        else
        {
            SP_2 = Graph.GetSP(
                matrix.m_matrix,
                LevelInformation.matrixSize.x,
                LevelInformation.matrixSize.y,
                positionPlayerIJ.x,
                positionPlayerIJ.y,
                (int)levelEditor.m_exit.transform.localPosition.x,
                (int)levelEditor.m_exit.transform.localPosition.y,
                true,
                10.0f
            );

            Debug.Assert(SP_2 != -1.0f);
        }

        m_SP = (int)SP_1 + (int)SP_2;
    }

    public void Update_nCases()
    {
        Vector2Int positionIJ = GetPositionIJ();
        m_matrix_nCases.TrySet1x1(positionIJ.x, positionIJ.y, 1);
    }

    public void Init_nCases()
    {
        m_nCases = m_matrix_nCases.Sum();
        m_matrix_nCases.Fill(0);
    }

    public void Update_nSpades()
    {
        LevelEditor levelEditor = m_objectLevelEditor.GetComponent<LevelEditor>();

        Vector2 positionPlayer = GetPosition();

        foreach (Transform _ in levelEditor.m_dangers.transform)
        {
            if (_.name.Substring(0, 3) != "Spa") continue;

            Vector2 positionSpade = (Vector2)_.localPosition;

            if (Utils.Vector2_Distance(positionSpade, positionPlayer) < distanceSpadesAndMonsters)
            {
                int i = (int)positionSpade.x;
                int j = (int)positionSpade.y;

                m_matrix_nSpades.Set1x1(i, j, 1);
            }
        }
    }

    public void Init_nSpades()
    {
        m_nSpades = m_matrix_nSpades.Sum();
        m_matrix_nSpades.Fill(0);
    }

    public void Update_nMonsters()
    {
        LevelEditor levelEditor = m_objectLevelEditor.GetComponent<LevelEditor>();

        Vector2 positionPlayer = GetPosition();

        foreach (Transform _ in levelEditor.m_dangers.transform)
        {
            if (_.name.Substring(0, 3) != "Dan") continue;
            
            Vector2 positionMonster = (Vector2)_.localPosition;

            if (Utils.Vector2_Distance(positionMonster, positionPlayer) < distanceSpadesAndMonsters)
            {
                int i = (int)positionMonster.x;
                int j = (int)positionMonster.y;

                m_matrix_nMonsters.Set1x1(i, j, 1);
            }
        }
    }

    public void Init_nMonsters()
    {
        m_nMonsters = m_matrix_nMonsters.Sum();
        m_matrix_nMonsters.Fill(0);
    }

    // Inputs.

    public void UpdateInputs(int p_index0, int p_index1)
    {
        ResetInputs();

        switch (p_index0)
        {
            case 0:
                break;

            case 1:
                m_left = true;
                break;

            case 2:
                m_right = true;
                break;

            default:
                Debug.Assert(false);
                break;
        }

        switch (p_index1)
        {
            case 0:
                break;

            case 1:
                m_up = true;
                break;

            default:
                Debug.Assert(false);
                break;
        }

        if (m_left)
        {
            if (m_faceRight)
            {
                m_faceRight = false;

                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
        }

        if (m_right)
        {
            if (!m_faceRight)
            {
                m_faceRight = true;

                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
        }
    }
    
    // ML-Agents.

    public override void OnEpisodeBegin()
    {
        ResetLevel();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Objects.

        LevelEditor levelEditor = m_objectLevelEditor.GetComponent<LevelEditor>();

        Matrix matrix = levelEditor.GetMatrix();

        // Player.

        {
            Vector2Int positionPlayerIJ = GetPositionIJ();

            int value = (int)LevelEditor.CaseID.CASE_PLAYER;

            matrix.TrySet1x1(positionPlayerIJ.x, positionPlayerIJ.y, value);
        }

        // VectorSensor.

        int w = matrix.m_w;
        int h = matrix.m_h;

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                int value = matrix.Get1x1(i, j);

                sensor.AddObservation(value);
            }
        }

        sensor.AddObservation(GetVelocity());
        sensor.AddObservation(GetPosition());
        sensor.AddObservation(m_onGround);
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
            actions[1] = 1;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.01f);

        m_step++;

        int index0 = actions.DiscreteActions[0];
        int index1 = actions.DiscreteActions[1];

        UpdateInputs(index0, index1);
        UpdateVelocity();
    }
}
 