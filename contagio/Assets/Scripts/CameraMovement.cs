﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    Vector3 dragOrigin;
    public LayerMask layerFloor;

    void Update()
    {
        if (PauseMenu.gameIsPaused) return;

        //When the mouse is pressed define drag origin
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = GetWorldPoint();

        }
        if (Input.GetMouseButton(0))
        {

            Vector3 direction = dragOrigin - GetWorldPoint();
            Vector3 newPos = cam.transform.position + direction;
            
            //Define Camera limits
            if (newPos.x > 87)
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

    //Get point where the raycast hit the floor
    private Vector3 GetWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100f, layerFloor);
        return hit.point;
    }
}
