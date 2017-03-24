using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dev script. used to enable abilities in runtime
/// </summary>
public class AbilitySetter : MonoBehaviour {

    public bool Blink;
    public bool Vortex;
    public bool Confusion;
    public bool Lightning;
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
            if (Confusion)
                _controller.EnableOrDisableAbility(AbilityController.Ability.Confusion, true);
            if (Lightning)
                _controller.EnableOrDisableAbility(AbilityController.Ability.Lightning, true);

            SetAbilities = false;
        }
	}
}
