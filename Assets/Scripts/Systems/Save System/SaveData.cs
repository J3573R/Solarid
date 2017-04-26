using System.Collections.Generic;
using System;

[Serializable]
public class SaveData {
    // Last level 
    private Level _currentLevel;

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
    /// Returns current level in savedata
    /// </summary>
    /// <returns></returns>
    public Level GetCurrentLevel()
    {
        return _currentLevel;
    }
}
