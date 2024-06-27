using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float m_angle = 0.0f;
    float m_height = 0.1f;
    float m_delta = 0.07f;
    Vector2 m_positionStart = Vector2.zero;

    void Start()
    {
        m_positionStart = transform.localPosition;
        m_angle = (int)m_positionStart.x;
    }

    void Update()
    {
        if (Time.timeScale == 0.0f) return;
        Vector2 position = transform.position;
        position.y = m_positionStart.y + Mathf.Sin(m_angle) * m_height;
        m_angle += m_delta;
        transform.localPosition = position;
    }
}
