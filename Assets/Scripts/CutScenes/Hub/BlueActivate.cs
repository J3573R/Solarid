using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueActivate : MonoBehaviour {

    [SerializeField]
    private GameObject _redBarrierFirst;
    [SerializeField]
    private GameObject _redBarrierSecond;

    private ParticleSystem _redParticleFirst;
    private ParticleSystem _redParticleSecond;


    // Use this for initialization
    void Start () {
        _redParticleFirst = _redBarrierFirst.GetComponent<ParticleSystem>();
        _redParticleSecond = _redBarrierSecond.GetComponent<ParticleSystem>();

        _redParticleFirst.Stop();
        _redParticleSecond.Stop();
        StartCoroutine(StopDelay());

        SaveSystem.Instance.SaveData.SetHubState(SaveData.HubState.BlueActivated);
        SaveSystem.Instance.SaveData.SetCrystals(SaveData.Crystal.none, SaveSystem.Instance.SaveData.GetHubCrystals());
        SaveSystem.Instance.SaveToFile();        
	}
	
    private IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(2.5f);
        _redBarrierFirst.SetActive(false);
    }
}
