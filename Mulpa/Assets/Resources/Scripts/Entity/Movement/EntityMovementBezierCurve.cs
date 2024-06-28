using UnityEngine;

public class EntityMovementBezierCurve
{
    float m_angle = 0.0f;
    float m_height = 0.1f;
    float m_delta = 0.07f;

    private Vector2 positionStart;

    //public CoinMovementIdle(Coin coin)
    //{
    //    this.positionStart = positionStart;


    //    m_positionStart = transform.localPosition;
    //    m_angle = (int)m_positionStart.x;
    //}

    //public void Update(Coin coin)
    //{
    //    if (Time.timeScale == 0.0f) return;
    //    Vector2 position = transform.position;
    //    position.y = m_positionStart.y + Mathf.Sin(m_angle) * m_height;
    //    m_angle += m_delta;
    //    transform.localPosition = position;
    //}

    //public void UpdatePosition(EntityAbstract entity)
    //{
    //    throw new System.NotImplementedException();
    //}
}