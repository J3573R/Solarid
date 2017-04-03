using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePickup : MonoBehaviour {

    private AbilityController _abilitycontroller;

    // Use this for initialization
    void Start()
    {
        _abilitycontroller = Globals.Player.GetComponent<AbilityController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _abilitycontroller.EnableOrDisableAbility(AbilityController.Ability.Clone, true);
            Destroy(gameObject);
        }
    }
}
