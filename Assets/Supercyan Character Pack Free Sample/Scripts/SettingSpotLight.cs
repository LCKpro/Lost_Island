using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSpotLight : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(0, 10, 0);
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
}
