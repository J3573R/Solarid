using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowActivate : MonoBehaviour {

    [SerializeField]
    private GameObject _blackBarrierFirst;


    private ParticleSystem _blackParticleFirst;

    // Use this for initialization
    void Start()
    {
        _blackParticleFirst = _blackBarrierFirst.GetComponent<ParticleSystem>();

        _blackParticleFirst.Stop();
        StartCoroutine(StopDelay());

        SaveSystem.Instance.SaveData.SetHubState(SaveData.HubState.BlueRedYellowActivated);
        SaveSystem.Instance.SaveData.SetCrystals(SaveData.Crystal.none, SaveSystem.Instance.SaveData.GetHubCrystals());
        SaveSystem.Instance.SaveToFile();
    }

    private IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(2.5f);
        _blackBarrierFirst.SetActive(false);
    }
}
