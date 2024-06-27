using UnityEngine;

public class Spawn : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        spriteRenderer.enabled = false;
    }
}
