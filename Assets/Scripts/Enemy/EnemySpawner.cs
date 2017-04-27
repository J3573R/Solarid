using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawnable enemy prefabs
    public List<GameObject> SpawnableGameObjects;
    // Delay to wait before starting spawning
    public float DelayBeforeFirstSpawn;
    // Delay between spawns !! RECOMMENDED TO BE MORE THAN 1 !!
    public float SpawnInterval;
    // Default state when spawn
    public EnemyBase.State StateOnAwake;
    // For playing spawn sound while burrow sound is active
    public AudioSource SpawnAudioController;
    
    // Timer between spawns
    private float _timeBeforeSpawn = 0;

    void OnEnable()
    {
        _timeBeforeSpawn = DelayBeforeFirstSpawn;
    }

	void Update () {

        // If spawn pool is not empty
	    if (SpawnableGameObjects.Count > 0)
	    {
	        if (_timeBeforeSpawn <= 0)
	        {                
                if(SpawnableGameObjects[0] == null)
                {
                    SpawnableGameObjects.RemoveAt(0);
                    return;
                }
	            GameObject spawn = Instantiate(SpawnableGameObjects[0], transform.position, Quaternion.identity);
	            SpawnableGameObjects.RemoveAt(0);
	            _timeBeforeSpawn = SpawnInterval;
                SpawnAudioController.Play();
	            if (StateOnAwake != EnemyBase.State.None)
	            {
	                EnemyBase temp = spawn.GetComponent<EnemyBase>();
	                temp.SetState(StateOnAwake);
	            }
	        }
	        else
	        {
	            _timeBeforeSpawn -= Time.deltaTime;
	        }
	    }
	    else
	    {
	        gameObject.SetActive(false);
	    }
	}
}
