using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    Vector3 dragOrigin;
    public LayerMask layerFloor;
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = GetWorldPoint();

        }
        if (Input.GetMouseButton(0))
        {


            Vector3 direction = dragOrigin - GetWorldPoint();
            Vector3 newPos = cam.transform.position + direction;
            
            if(newPos.x > 90)
            {
                newPos.x = 90;
            }
            if (newPos.x < 10)
            {
                newPos.x = 10;
            }
            if (newPos.z < -105)
            {
                newPos.z = -105;
            }
            if (newPos.z > -16)
            {
                newPos.z = -16;
            }
            cam.transform.position = newPos;
        }
    }

    private Vector3 GetWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100f, layerFloor);
        return hit.point;
    }
}
