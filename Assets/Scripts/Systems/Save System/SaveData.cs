using System.Collections.Generic;
using System;

[Serializable]
public class SaveData {
    // Last level 
    private Level _currentLevel;
    // Dictionary where Key is the ability and Value is wether the ability should be in use or not
    private Dictionary<AbilityController.Ability, bool> _abilityArray;

    //TODO: Refactor this class, unnecessary methods which can be combined

    // Current level to save
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

    /// <summary>
    /// Constructor, creates Dictionary with all abilities disabled
    /// </summary>
    /// <param name="level">level to save</param>
    public SaveData(Level level)
    {
        _currentLevel = level;
        _abilityArray = new Dictionary<AbilityController.Ability, bool>();

        _abilityArray.Add(AbilityController.Ability.Blink, false);
        _abilityArray.Add(AbilityController.Ability.Vortex, false);
        _abilityArray.Add(AbilityController.Ability.Clone, false);
    }

    /// <summary>
    /// Set level to savedata
    /// </summary>
    /// <param name="level"></param>
    public void SetCurrentLevel(Level level)
    {
        _currentLevel = level;
    }

    /// <summary>
    /// Returns current level in savedata
    /// </summary>
    /// <returns></returns>
    public Level GetCurrentLevel()
    {
        return _currentLevel;
    }

    /// <summary>
    /// Returns Dictionary with ability info, creates new if null
    /// </summary>
    /// <returns>the dictionary...</returns>
    public Dictionary<AbilityController.Ability, bool> GetAbilityArray()
    {
        if (_abilityArray == null)
        {
            _abilityArray = new Dictionary<AbilityController.Ability, bool>();

            _abilityArray.Add(AbilityController.Ability.Blink, false);
            _abilityArray.Add(AbilityController.Ability.Vortex, false);
            _abilityArray.Add(AbilityController.Ability.Clone, false);
        }

        return _abilityArray;
    }

    /// <summary>
    /// Sets all Abilities to disabled in dictionary. Used in reseting save data
    /// </summary>
    public void ResetAbilityData()
    {
        _abilityArray[AbilityController.Ability.Blink] = false;
        _abilityArray[AbilityController.Ability.Vortex] = false;
        _abilityArray[AbilityController.Ability.Clone] = false;
    }

    /// <summary>
    /// Replace Savedata dictionary with another one
    /// </summary>
    /// <param name="array"></param>
    public void SetAbilityArray(Dictionary<AbilityController.Ability, bool> array)
    {
        _abilityArray = array;
    }
}
