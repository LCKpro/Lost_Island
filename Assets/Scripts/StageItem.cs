using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageItem : Spawn
{
    protected Transform player;
    public int itemNum;
    public float rotSpeed = 10.0f;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        transform.Rotate(0, 1, 0);
    }
}
