using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{

    public EnemyBase TargetEnemy;
    public LightningBoltSpark Spark;

    private float Speed = 20;

    void Update () {
	    if (TargetEnemy != null)
	    {
	        transform.LookAt(TargetEnemy.transform);
	        transform.position = Vector3.MoveTowards(transform.position, TargetEnemy.gameObject.transform.position,
	            Time.deltaTime*Speed);
	    }
	    else
	    {
	        gameObject.SetActive(false);
	    }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Collider[] enemyColliders = Physics.OverlapSphere(other.gameObject.transform.position, 25);
            foreach (Collider enemy in enemyColliders)
            {
                EnemyBase tmp = enemy.gameObject.GetComponent<EnemyBase>();
                if (tmp != null && other.gameObject != tmp.gameObject && tmp.tag == "Enemy")
                {
                    LightningBoltSpark spark = Instantiate(Spark, other.transform.position, Quaternion.identity);
                    spark.IgnoreEnemy = other.gameObject;
                    spark.TargetEnemy = tmp;
                }
            }

            gameObject.SetActive(false);
        }
    }
}
