using UnityEngine;

public class Slime : MonoBehaviour
{
    [HideInInspector] public float m_direction = +1.0f;

    [HideInInspector] public bool m_directionSwap = false;

    public float m_speed = +2.0f;

    //public void Update()
    //{
    //    Collider2D collider2D = GetComponent<Collider2D>();

    //    collider2D.enabled = false;

    //    float size = 0.65f;
    //    float dst = 0.05f;

    //    Vector2 center = transform.position;

    //    // Left Middle.

    //    Vector2 positionLM = center;
    //    positionLM.x -= size / 2.0f;
        
    //    RaycastHit2D hitLM = Physics2D.Raycast(positionLM, Vector2.left, dst);

    //    bool LM = (hitLM && (hitLM.collider.tag == "tag_block" || hitLM.collider.tag == "tag_danger"));

    //    Debug.DrawRay(positionLM, Vector2.left * dst, Color.blue);

    //    // Right Middle.

    //    Vector2 positionRM = center;
    //    positionRM.x += size / 2.0f;

    //    RaycastHit2D hitRM = Physics2D.Raycast(positionRM, Vector2.right, dst);

    //    bool RM = (hitRM && (hitRM.collider.tag == "tag_block" || hitRM.collider.tag == "tag_danger"));

    //    Debug.DrawRay(positionRM, Vector2.right * dst, Color.blue);

    //    // Left Down.

    //    Vector2 positionLD = center;
    //    positionLD.x -= size / 2.0f;
    //    positionLD.y -= 1.0f / 2.0f;

    //    RaycastHit2D hitLD = Physics2D.Raycast(positionLD, Vector2.down, dst);

    //    bool LD = (hitLD && (hitLD.collider.tag == "tag_block" || hitLD.collider.tag == "tag_danger"));

    //    Debug.DrawRay(positionLD, Vector2.down * dst, Color.red);

    //    // Right Down.

    //    Vector2 positionRD = center;
    //    positionRD.x += size / 2.0f;
    //    positionRD.y -= 1.0f / 2.0f;

    //    RaycastHit2D hitRD = Physics2D.Raycast(positionRD, Vector2.down, dst);

    //    bool RD = (hitRD && (hitRD.collider.tag == "tag_block" || hitRD.collider.tag == "tag_danger"));

    //    Debug.DrawRay(positionRD, Vector2.down * dst, Color.red);

    //    // Swap.

    //    if (m_directionSwap)
    //    {
    //        if (!LM && !RM && LD && RD)
    //        {
    //            m_directionSwap = false;
    //        }
    //    }
    //    else
    //    {
    //        if (LD ^ RD)
    //        {
    //            m_directionSwap = true;
    //        }

    //        if (LM | RM)
    //        {
    //            m_directionSwap = true;
    //        }

    //        if (m_directionSwap)
    //        {
    //            m_direction *= -1.0f;

    //            m_directionSwap = true;
            
    //            Vector2 scale = transform.localScale;
    //            scale.x *= -1.0f;
    //            transform.localScale = scale;
    //        }
    //    }

    //    collider2D.enabled = true;

    //    // Update velocity.

    //    Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

    //    Vector2 velocity = rigidBody2D.velocity;

    //    velocity.x = m_speed * m_direction;

    //    rigidBody2D.velocity = velocity;
    //}
}
