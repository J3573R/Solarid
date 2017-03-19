using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData {

    private Level _currentLevel;

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
    }

	public void SetCurrentLevel(Level level)
    {
        _currentLevel = level;
    }

    public Level GetCurrentLevel()
    {
        return _currentLevel;
    }
}
