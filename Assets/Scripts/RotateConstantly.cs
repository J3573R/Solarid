using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour
{

    public Vector3 RotationSpeed = Vector3.zero;
    public Vector3 ChangeRotationSpeed = Vector3.zero;

    void Start()
    {
        ChangeRotationSpeed = RotationSpeed;
    }

    void Update()
    {        
        if(ChangeRotationSpeed == RotationSpeed)
        {
            transform.Rotate(RotationSpeed * Time.deltaTime);
        } else
        {
            RotationSpeed = Vector3.Lerp(RotationSpeed, ChangeRotationSpeed, Time.smoothDeltaTime);
            transform.Rotate(RotationSpeed * Time.deltaTime);
        }
        
    }

}
