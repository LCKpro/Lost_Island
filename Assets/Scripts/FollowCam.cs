using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject target;
    public float distanceY = 2.0f;
    public float distanceZ = 4.0f;
    public Transform turnPoint;
    private Vector3 offset;
    Vector3 reverseDistance;
    //Quaternion originRot;
    public float xmove = 0.0f;
    public float ymove = 0.0f;

    public float rotateSpeed = 10.0f;
    static public FollowCam instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //originRot = transform.rotation;
        offset = new Vector3(0, 3.2f, -7.5f);
        reverseDistance = new Vector3(0, 0, distanceZ);
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        /*if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X");

            ymove -= Input.GetAxis("Mouse Y");
            
            //Debug.Log($"X : {xmove} Y : {ymove}");
        }
        else
        {
            ymove = 0;
            xmove = 0;
            transform.rotation = originRot;
            //Debug.Log($"X : {xmove} Y : {ymove}");
        }*/

        //xmove = Mathf.Clamp(xmove, -179.0f, 179.0f);
        //ymove = Mathf.Clamp(ymove, -30.0f, 55.0f);

        transform.position = target.transform.position - turnPoint.rotation * reverseDistance;
        transform.rotation = Quaternion.Euler(ymove, xmove, 0);
        //transform.position = target.transform.position - transform.rotation * reverseDistance;
        transform.position += offset;
    }
    
}
