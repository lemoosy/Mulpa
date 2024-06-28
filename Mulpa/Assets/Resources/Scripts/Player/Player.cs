using UnityEngine;

public class Player : MonoBehaviour
{
    private bool onGround = false;

    private Level level = null;
    private IPlayerInput input = null;
    private PlayerSprite sprite = null;
    private PlayerMovement movement = null;
    private PlayerInventory inventory = null;
    private IPlayerState state = null;

    public void Start()
    {
        Debug.Assert(level != null);
        input = new PlayerInputKeyboard();
        sprite = new PlayerSprite();
        movement = new PlayerMovement();
        inventory = new PlayerInventory();
        SetState(new PlayerStateStart());
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        IColliderManager entity = collider.gameObject.GetComponent<IColliderManager>();
        entity.CollisionWithPlayer(this);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        IColliderManager entity = collision.gameObject.GetComponent<IColliderManager>();
        entity.CollisionWithPlayer(this);
    }

    public void Update()
    {
        state.Update(this);
    }

    public void SetLevel(Level level)
    {
        this.level = level;
    }

    public Level GetLevel()
    {
        return level;
    }

    public IPlayerInput GetInput()
    {
        return input;
    }

    public void SetInput(IPlayerInput input)
    {
        this.input = input;
    }

    public PlayerSprite GetSprite()
    {
        return sprite;
    }

    public PlayerMovement GetMovement()
    {
        return movement;
    }

    public PlayerInventory GetInventory()
    {
        return inventory;
    }

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

    //public void UpdateOnGround()
    //{
    //    onGround = false;

    //    Collider2D collider = gameObject.GetComponent<Collider2D>();

    //    collider.enabled = false;
        
    //    float size = 0.4f;
    //    float dst = 0.05f;

    //    // LEFT

    //    Vector2 positionL = gameObject.transform.position;
    //    positionL.x -= size / 2.0f;
    //    positionL.y -= 0.5f;

    //    RaycastHit2D hitL = Physics2D.Raycast(positionL, Vector2.down, dst);

    //    Debug.DrawRay(positionL, Vector2.down * dst, Color.red);

    //    // RIGHT

    //    Vector2 positionR = gameObject.transform.position;
    //    positionR.x += size / 2.0f;
    //    positionR.y -= 0.5f;
        
    //    RaycastHit2D hitR = Physics2D.Raycast(positionR, Vector2.down, dst);

    //    Debug.DrawRay(positionR, Vector2.down * dst, Color.red);

    //    // OnGround

    //    onGround |= (hitL && (hitL.collider.tag == "tag_block"));
    //    onGround |= (hitR && (hitR.collider.tag == "tag_block"));

    //    collider.enabled = true;
    //}
}
