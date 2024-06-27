using UnityEngine;

public class Button : MonoBehaviour
{
    public Sprite m_spriteA = null;
    
    public Sprite m_spriteB = null;

    [HideInInspector] public bool m_pressed = false;

    public void Start()
    {
        SetSprite(m_spriteA);
    }

    public void OnMouseEnter()
    {
        SetSprite(m_spriteB);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_pressed = true;
        }
    }

    public void OnMouseExit()
    {
        SetSprite(m_spriteA);
    }

    private void SetSprite(Sprite p_sprite)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = p_sprite;
    }
}
