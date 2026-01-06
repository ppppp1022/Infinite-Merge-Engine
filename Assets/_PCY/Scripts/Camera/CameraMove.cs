using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera cam;
    public float moveSpeed;
    public Vector3 boundary_RT;
    public Vector3 boundary_LB;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos =new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            targetPos = cam.transform.position + Vector3.up * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            targetPos = cam.transform.position + Vector3.left * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            targetPos = cam.transform.position + Vector3.down * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetPos = cam.transform.position + Vector3.right * moveSpeed;
        }
        cam.transform.position = new Vector3(Mathf.Clamp(targetPos.x, boundary_LB.x, boundary_RT.x), Mathf.Clamp(targetPos.y, boundary_LB.y, boundary_RT.y), cam.transform.position.z);
        print(cam.transform.position);
    }
}
