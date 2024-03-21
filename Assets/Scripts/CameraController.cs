using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    float rotationX = 0f;
    float rotationY = 0f;
    public float sensitivity = 5f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;    // The distance between the camera and the player

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        
    }
}
