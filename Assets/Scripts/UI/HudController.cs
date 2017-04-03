using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    [SerializeField]
    private GameObject _cooldownGO;

    private AbilityController _abilityController;
    private Image _cooldownImage;
    private Material _cooldownMaterial;

	// Use this for initialization
	void Start () {
        _cooldownImage = _cooldownGO.GetComponent<Image>();
        _cooldownMaterial = Instantiate(_cooldownImage.material);
        _cooldownImage.material = _cooldownMaterial;

        _abilityController = Globals.Player.GetComponent<AbilityController>();
    }
	
	// Update is called once per frame
	void Update () {
        float progress = _abilityController.GetCurrentCooldownProgress();

        progress = 1 - progress;

        _cooldownMaterial.SetFloat("_Progress", progress);
    }
}
