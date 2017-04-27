using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedActivate : MonoBehaviour {

    [SerializeField]
    private GameObject _yellowBarrierFirst;
    [SerializeField]
    private GameObject _yellowBarrierSecond;

    private ParticleSystem _yellowParticleFirst;
    private ParticleSystem _yellowParticleSecond;


    // Use this for initialization
    void Start()
    {
        _yellowParticleFirst = _yellowBarrierFirst.GetComponent<ParticleSystem>();
        _yellowParticleSecond = _yellowBarrierSecond.GetComponent<ParticleSystem>();

        _yellowParticleFirst.Stop();
        _yellowParticleSecond.Stop();
        StartCoroutine(StopDelay());

        SaveSystem.Instance.SaveData.SetHubState(SaveData.HubState.BlueRedActivated);
        SaveSystem.Instance.SaveData.SetCrystals(SaveData.Crystal.none, SaveSystem.Instance.SaveData.GetHubCrystals());
        SaveSystem.Instance.SaveToFile();
    }

    private IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(2.5f);
        _yellowBarrierFirst.SetActive(false);
    }
}
