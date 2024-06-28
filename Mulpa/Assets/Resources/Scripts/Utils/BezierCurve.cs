using System.Collections.Generic;
using UnityEngine;
 
public class BezierCurve
{
    public List<Transform> controlPoints; // Liste des points de contr�le
    public float speed = 1.0f; // Vitesse de d�placement le long de la courbe
    private float t = 0.0f; // Param�tre de temps



    void Update()
    {
        if (controlPoints.Count > 1)
        {
            t += Time.deltaTime * speed;

            // Boucle le param�tre t
            if (t > 1.0f)
            {
                t = 0.0f;
            }

            // Calcule la position sur la courbe de B�zier
            Vector3 position = CalculateBezierPoint(t, controlPoints);
            //transform.position = position;
        }
    }

    Vector3 CalculateBezierPoint(float t, List<Transform> points)
    {
        if (points.Count == 1)
        {
            return points[0].position;
        }

        List<Vector3> newPoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[i].position;
            Vector3 p1 = points[i + 1].position;
            Vector3 newPoint = Vector3.Lerp(p0, p1, t);
            newPoints.Add(newPoint);
        }
        return new Vector3();
        //return CalculateBezierPoint(t, newPoints);
    }
}