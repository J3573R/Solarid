using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dev script. used to enable abilities in runtime
/// </summary>
public class AbilitySetter : MonoBehaviour {

    public bool Blink;
    public bool Vortex;
    public bool Clone;
    public bool SetAbilities;

    private AbilityController _controller;

	// Use this for initialization
	void Start () {
        _controller = FindObjectOfType<AbilityController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (SetAbilities)
        {
            //Debug.Log("SETTING ABILITIES");
            if (Blink)
                _controller.EnableOrDisableAbility(AbilityController.Ability.Blink, true);
            if (Vortex)
                _controller.EnableOrDisableAbility(AbilityController.Ability.Vortex, true);
            if (Clone)
                _controller.EnableOrDisableAbility(AbilityController.Ability.Clone, true);

            SetAbilities = false;
        }
	}
}
