using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltSpark : MonoBehaviour {

    public EnemyBase TargetEnemy;
    public GameObject IgnoreEnemy;

    private float Speed = 20;

    void Update () {
        if (TargetEnemy != null)
        {
            transform.LookAt(TargetEnemy.transform);
            transform.position = Vector3.MoveTowards(transform.position, TargetEnemy.gameObject.transform.position,
                Time.deltaTime * Speed);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() != IgnoreEnemy.GetInstanceID() && other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
