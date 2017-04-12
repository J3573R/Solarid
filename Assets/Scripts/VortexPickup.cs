using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexPickup : MonoBehaviour {

    private AbilityController _abilitycontroller;

    // Use this for initialization
    void Start()
    {
        _abilitycontroller = GameStateManager.Instance.GameLoop.Player.gameObject.GetComponent<AbilityController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _abilitycontroller.EnableOrDisableAbility(AbilityController.Ability.Vortex, true);
            Destroy(gameObject);
        }
    }
}
