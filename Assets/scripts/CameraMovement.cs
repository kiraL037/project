using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraOn;
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void LateUpdate()
    {
        Vector3 diff = Vector3.zero;

        float diffX = cameraOn.position.x - transform.position.x;
        if (diffX > boundX || diffX < -boundX)
        {
            if (transform.position.x < cameraOn.position.x)
            {
                diff.x = diffX - boundX;
            }
            else
            {
                diff.x = diffX + boundX;
            }
        }

        float diffY = cameraOn.position.y - transform.position.y;
        if (diffY > boundY || diffY < -boundY)
        {
            if (transform.position.y < cameraOn.position.y)
            {
                diff.y = diffY - boundY;
            }
            else
            {
                diff.y = diffY + boundY;
            }
        }

        transform.position += new Vector3(diff.x, diff.y, 0);

    }
}
