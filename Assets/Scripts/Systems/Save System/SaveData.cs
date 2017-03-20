using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData {

    private Level _currentLevel;
    private Dictionary<AbilityController.Ability, bool> _abilityArray;

    public enum Level
    {
        Hub,
        South1,
        South2,
        South3,
        South4,
        South5,
        NoSave
    }

    public SaveData(Level level)
    {
        _currentLevel = level;

        _abilityArray = new Dictionary<AbilityController.Ability, bool>();

        _abilityArray.Add(AbilityController.Ability.Blink, false);
        _abilityArray.Add(AbilityController.Ability.Vortex, false);
        _abilityArray.Add(AbilityController.Ability.Confusion, false);
        _abilityArray.Add(AbilityController.Ability.Lightning, false);

        Debug.Log("Array created as empty");

    }

    public void SetCurrentLevel(Level level)
    {
        _currentLevel = level;
    }

    public Level GetCurrentLevel()
    {
        return _currentLevel;
    }

    public Dictionary<AbilityController.Ability, bool> GetAbilityArray()
    {

        if (_abilityArray == null)
        {
            _abilityArray = new Dictionary<AbilityController.Ability, bool>();

            _abilityArray.Add(AbilityController.Ability.Blink, false);
            _abilityArray.Add(AbilityController.Ability.Vortex, false);
            _abilityArray.Add(AbilityController.Ability.Confusion, false);
            _abilityArray.Add(AbilityController.Ability.Lightning, false);
        }

        return _abilityArray;
    }

    public void ResetAbilityData()
    {
        _abilityArray[AbilityController.Ability.Blink] = false;
        _abilityArray[AbilityController.Ability.Vortex] = false;
        _abilityArray[AbilityController.Ability.Confusion] = false;
        _abilityArray[AbilityController.Ability.Lightning] = false;
    }

    public void SetAbilityArray(Dictionary<AbilityController.Ability, bool> array)
    {
        _abilityArray = array;
    }
}
