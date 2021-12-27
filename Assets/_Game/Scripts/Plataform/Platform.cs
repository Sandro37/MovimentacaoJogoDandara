using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;

    public Vector2 Point(Vector2 hitPoint)
    {
        Vector2 newPoint = hitPoint;

        if(hitPoint.x > maxPoint.position.x)
        {
            newPoint.x = maxPoint.position.x;
        }

        if (hitPoint.y > maxPoint.position.y)
        {
            newPoint.y = maxPoint.position.y;
        }

        if (hitPoint.x < minPoint.position.x)
        {
            newPoint.x = minPoint.position.x;
        }

        if(hitPoint.y < minPoint.position.y)
        {
            newPoint.y = minPoint.position.y;
        }
        
        return newPoint;
    }
}
