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
            
            if(newPos.x > 87)
            {
                newPos.x = 87;
            }
            if (newPos.x < 12)
            {
                newPos.x = 12;
            }
            if (newPos.z < -102)
            {
                newPos.z = -102;
            }
            if (newPos.z > -20)
            {
                newPos.z = -20;
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
