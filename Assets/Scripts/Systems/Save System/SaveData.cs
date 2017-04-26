using System.Collections.Generic;
using System;

[Serializable]
public class SaveData {
    // Last level 
    private Level _currentLevel;
    private Crystal _crystalWithPlayer;
    private Dictionary<Crystal, bool> _hubCrystals;

    //TODO: Refactor this class, unnecessary methods which can be combined

    public enum Crystal
    {
        Blue,
        Red,
        Yellow,
        Black,
        none
    }

    // Current level to save
    public enum Level
    {
        Hub,
        South1,
        South2,
        South3,
        South4,
        South5,
        West1,
        West2,
        West3,
        West4,
        West5,
        East1,
        East2,
        East3,
        East4,
        East5,
        North1,
        North2,
        North3,
        North4,
        North5,
        NoSave
    }

    /// <summary>
    /// Constructor, creates Dictionary with all abilities disabled
    /// </summary>
    /// <param name="level">level to save</param>
    public SaveData(Level level)
    {
        _currentLevel = level;
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
    /// Set Crystal data to save
    /// </summary>
    /// <param name="crystalWithPlayer"></param>
    /// <param name="hubCrystals"></param>
    public void SetCrystals(Crystal crystalWithPlayer, Dictionary<Crystal, bool> hubCrystals)
    {
        _crystalWithPlayer = crystalWithPlayer;
        _hubCrystals = hubCrystals;
    }

    /// <summary>
    /// Get Crystal Player has currently
    /// </summary>
    /// <returns></returns>
    public Crystal GetCrystalWithPlayer()
    {
        return _crystalWithPlayer;
    }

    /// <summary>
    /// Get hub crystal data
    /// </summary>
    /// <returns></returns>
    public Dictionary<Crystal, bool> GetHubCrystals()
    {
        return _hubCrystals;
    }

    /// <summary>
    /// Returns current level in savedata
    /// </summary>
    /// <returns></returns>
    public Level GetCurrentLevel()
    {
        return _currentLevel;
    }
}
