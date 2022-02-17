using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWheel : MonoBehaviour
{
    public float wheelSpeed = 10.0f;
    public Transform target;

    private Camera thisCam;
    private Vector3 worldDefaultForward;

    void Start()
    {
        thisCam = GetComponent<Camera>();
        worldDefaultForward = this.transform.forward;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * (-wheelSpeed);

        if(thisCam.fieldOfView <= 55.0f && scroll < 0)
        {
            // 최대 줌 인
            thisCam.fieldOfView = 55.0f;
        }
        else if(thisCam.fieldOfView >= 85.0f && scroll > 0)
        {
            // 최대 줌 아웃
            thisCam.fieldOfView = 85.0f;
        }
        else
        {
            // 줌 인
            thisCam.fieldOfView += scroll;
        }

        //일정 구간 줌으로 들어가면 캐릭터를 바라보도록 한다.
        if(target && thisCam.fieldOfView <= 30.0f)
        {
            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                Quaternion.LookRotation(target.position - transform.position),
                0.15f);
        }
        else
        {
            //일정 구간 밖에서는 원래의 카메라 방향으로 되돌아가기   
            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                Quaternion.LookRotation(worldDefaultForward),
                0.15f);
        }
    }
}
