using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject m_objectLevelEditor = null;



    [HideInInspector] public bool m_onGround = false;
    [HideInInspector] public bool m_resetPosition = false;
    [HideInInspector] public bool m_atExit = false;
    [HideInInspector] public bool m_isDead = false;

    public Animator animator = null;




    private PlayerCollision collision = null;

    private PlayerInput input = null;

    private PlayerSprite sprite = null;

    private IPlayerState state = null;



    public void ResetLevel()
    {
        //Debug.Assert(m_objectLevelEditor);

        ResetVelocity();

        m_resetPosition = true;
        m_atExit = false;
        m_isDead = false;

        animator.SetBool("IsJumping", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("OnGround", false);

    }

    public void Start()
    {
        collision = new PlayerCollision();

        sprite = new PlayerSprite();

        input = new PlayerInput();

        SetState(new PlayerStateStart());
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        LevelEditor levelEditor = m_objectLevelEditor.GetComponent<LevelEditor>();

        if (collider.tag == "tag_danger")
        {
            m_collisionDanger = true;
        }

        if (collider.tag == "tag_coin")
        {
            Reward reward = collider.gameObject.GetComponent<Reward>();
            
            Debug.Assert(reward);

            if (levelEditor.m_lever)
            {
                if (reward.m_prevLever)
                {
                    m_collisionCoin = true;
                    Destroy(collider.gameObject);
                }
            }
            else
            {
                if (reward.m_nextLever)
                {
                    m_collisionCoin = true;
                    Destroy(collider.gameObject);
                }
            }
        }

        if (collider.tag == "tag_lever")
        {
            m_collisionLever = true;
            Destroy(collider.gameObject);
            Destroy(levelEditor.m_doors);
        }

        if (collider.tag == "tag_exit")
        {
            m_collisionExit = true;
        }

        if (collider.tag == "tag_bonus")
        {
            Reward reward = collider.gameObject.GetComponent<Reward>();

            Debug.Assert(reward);

            if (levelEditor.m_lever)
            {
                if (reward.m_prevLever)
                {
                    m_collisionBonus = true;
                    Destroy(collider.gameObject);
                }
            }
            else
            {
                if (reward.m_nextLever)
                {
                    m_collisionBonus = true;
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tag_danger")
        {
            m_collisionDanger = true;
        }
    }



    public void Update()
    {
        state.Update(this);



        UpdateStates();
        UpdateInputs();
        UpdateVelocity();

        if (m_isDead)
        {
            gameObject.SetActive(false);
        }
        
        if (m_atExit)
        {
            gameObject.SetActive(false);
        }
    }



    public void LateUpdate()
    {
        if (m_resetPosition)
        {
            m_resetPosition = false;
            ResetPosition();
        }

        m_collisionDanger = false;
        m_collisionCoin = false;
        m_collisionLever = false;
        m_collisionExit = false;
        m_collisionBonus = false;
    }
    





    // Input.

    public PlayerInput GetInput()
    {
        return input;
    }

    // Position.



    // State.

    public IPlayerState GetState()
    {
        return state;
    }

    public void SetState(IPlayerState state)
    {
        if (state != null)
        {
            state.Exit(this);
        }

        this.state = state;

        state.Enter(this);
    }









    // States.

    public void UpdateOnGround()
    {
        m_onGround = false;

        Collider2D collider = gameObject.GetComponent<Collider2D>();

        collider.enabled = false;
        
        float size = 0.4f;
        float dst = 0.05f;

        // LEFT

        Vector2 positionL = gameObject.transform.position;
        positionL.x -= size / 2.0f;
        positionL.y -= 0.5f;

        RaycastHit2D hitL = Physics2D.Raycast(positionL, Vector2.down, dst);

        Debug.DrawRay(positionL, Vector2.down * dst, Color.red);

        // RIGHT

        Vector2 positionR = gameObject.transform.position;
        positionR.x += size / 2.0f;
        positionR.y -= 0.5f;
        
        RaycastHit2D hitR = Physics2D.Raycast(positionR, Vector2.down, dst);

        Debug.DrawRay(positionR, Vector2.down * dst, Color.red);

        // OnGround

        m_onGround |= (hitL && (hitL.collider.tag == "tag_block"));
        m_onGround |= (hitR && (hitR.collider.tag == "tag_block"));

        animator.SetBool("OnGround", m_onGround);

        collider.enabled = true;
    }

    public void UpdateStates()
    {
        UpdateOnGround();

        if (m_collisionDanger)
        {
            m_isDead = true;
        }

        if (m_collisionExit)
        {
            m_atExit = true;
        }
    }



    // Inputs.

    public void ResetInputs()
    {
        m_left = false;
        m_right = false;
        m_up = false;
    }

    public void UpdateInputs()
    {
        //if (Time.timeScale == 0.0f)
        //{
        //    return;
        //}

        Debug.Log("Update Inputs = " + Input.GetKey(KeyCode.Space).ToString());

        ResetInputs();

        if (Input.GetKey(KeyCode.Space))
        {
        Debug.Log("LEFT");
            m_left = true;

            if (m_faceRight)
            {
                m_faceRight = false;

                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
        Debug.Log("RIGHT");
            m_right = true;

            if (!m_faceRight)
            {
                m_faceRight = true;

                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_up = true;
        }
    }




    // Velocity.

    public Vector2 GetVelocity()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        return body.velocity;
    }

    public void SetVelocity(Vector2 p_velocity)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.velocity = p_velocity;
    }

    public void ResetVelocity()
    {
        SetVelocity(Vector2.zero);
    }

    public void UpdateVelocity()
    {
        

        animator.SetBool("IsJumping", false);
        animator.SetBool("IsWalking", false);

        Vector2 velocity = GetVelocity();

        if (m_left)
        {
            Debug.Log("LEFT");
            if (m_impulse)
            {
                velocity.x = -m_speed.x;
            }
            else
            {
                velocity.x += -m_speed.x * 0.15f;
            }
        }

        if (m_right)
        {
            Debug.Log("RIGHT");
            if (m_impulse)
            {
                velocity.x = +m_speed.x;
            }
            else
            {
                velocity.x += +m_speed.x * 0.15f;
            }
        }

        if (m_up && m_onGround)
        {
            animator.SetBool("IsJumping", true);

            velocity.y = m_speed.y;
        }

        velocity.x *= 0.8f;

        velocity.x = Mathf.Clamp(velocity.x, -m_speed.x, +m_speed.x);
        velocity.y = Mathf.Clamp(velocity.y, -m_speed.y, +m_speed.y);

        SetVelocity(velocity);

        if (Mathf.Abs(velocity.x) > 1.0f)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    // Position.

    public Vector2 GetPosition()
    {
        return (Vector2)transform.localPosition;
    }

    public Vector2Int GetPositionIJ()
    {
        Vector2 position = GetPosition();

        Vector2Int positionIJ = new Vector2Int((int)position.x, (int)position.y);

        return positionIJ;
    }

    public void SetPosition(Vector2 p_position)
    {
        transform.localPosition = (Vector3)p_position;
    }

    public void ResetPosition()
    {
        LevelEditor levelEditor = m_objectLevelEditor.GetComponent<LevelEditor>();

        Vector3 positionSpawn = levelEditor.m_spawn.transform.localPosition;

        SetPosition((Vector2)positionSpawn);

        transform.localScale = levelEditor.m_spawn.transform.localScale;

        m_faceRight = (transform.localScale.x > 0.0f);
    }

    public bool OutOfDimension()
    {
        int w = LevelInformation.matrixSize.x;
        int h = LevelInformation.matrixSize.y;

        Vector2Int positionIJ = GetPositionIJ();

        if (positionIJ.x < 0)
        {
            return true;
        }

        if (positionIJ.y < 0)
        {
            return true;
        }

        if (positionIJ.x >= w)
        {
            return true;
        }

        if (positionIJ.y >= h)
        {
            return true;
        }

        return false;
    }
}
