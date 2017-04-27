using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class SaveSystem : MonoBehaviour
{

    public static SaveSystem Instance;

    private const string SaveFileName = "Save.dat";
    public SaveData SaveData;
    private Player _player;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.name = "SaveSystem";
            DontDestroyOnLoad(Instance);            
        }
        else
        {
            Destroy(gameObject);
        }

        if (SaveData == null)
        {
            Debug.Log("SaveData = null, creating new");
            if (SaveFileExists())
                LoadSaveData();
            else
            {
                SaveData = new SaveData(SaveData.Level.NoSave);
            }
        }
        else
        {
            LoadSaveData();
        }
        _player = FindObjectOfType<Player>();

        if (_player != null)
        {
            _player.init();
            _player.HubCrystals = SaveData.GetHubCrystals();
            _player.CrystalWithPlayer = SaveData.GetCrystalWithPlayer();
        }        
    }    

    
    // Path to the save file
    public static string SaveFilePath { get { return Path.Combine(Application.persistentDataPath, SaveFileName); } }

    /// <summary>
    /// Saves the current game data
    /// </summary>
    /// <param name="saveData"></param>
    public void SaveToFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        bf.Serialize(stream, SaveData);
        File.WriteAllBytes(SaveFilePath, stream.GetBuffer());
        //Debug.Log("Saved Data, CurrentLevel =" + SaveData.GetCurrentLevel());
    }

    public void SaveAll()
    {
        SaveCurrentLevel();
        SaveToFile();
    }

    internal void ResetSave()
    {
        Dictionary<SaveData.Crystal, bool> tmp = new Dictionary<SaveData.Crystal, bool>();
        tmp.Add(SaveData.Crystal.Blue, false);
        tmp.Add(SaveData.Crystal.Red, false);
        tmp.Add(SaveData.Crystal.Yellow, false);
        tmp.Add(SaveData.Crystal.Black, false);
        SaveData.SetCrystals(SaveData.Crystal.none, tmp);
        SaveData.SetCurrentLevel(SaveData.Level.NoSave);
        SaveData.SetHubState(SaveData.HubState.NothingActivated);

        SaveToFile();
    }

    public void SaveOnlyLevel()
    {
        SaveCurrentLevel();
        SaveToFile();
    }
    


    /// <summary>
    /// Sets the current level in use to SaveData and saves it to file
    /// </summary>
    public void SaveCurrentLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (levelName.Equals("South1"))
            SaveData.SetCurrentLevel(SaveData.Level.South1);
        else if (levelName.Equals("South2"))
            SaveData.SetCurrentLevel(SaveData.Level.South2);
        else if (levelName.Equals("South3"))
            SaveData.SetCurrentLevel(SaveData.Level.South3);
        else if (levelName.Equals("South4"))
            SaveData.SetCurrentLevel(SaveData.Level.South4);
        else if (levelName.Equals("South5"))
            SaveData.SetCurrentLevel(SaveData.Level.South5);
        else if (levelName.Equals("West1"))
            SaveData.SetCurrentLevel(SaveData.Level.West1);
        else if (levelName.Equals("West2"))
            SaveData.SetCurrentLevel(SaveData.Level.West2);
        else if (levelName.Equals("West3"))
            SaveData.SetCurrentLevel(SaveData.Level.West3);
        else if (levelName.Equals("West4"))
            SaveData.SetCurrentLevel(SaveData.Level.West4);
        else if (levelName.Equals("West5"))
            SaveData.SetCurrentLevel(SaveData.Level.West5);
        else if (levelName.Equals("East1"))
            SaveData.SetCurrentLevel(SaveData.Level.East1);
        else if (levelName.Equals("East2"))
            SaveData.SetCurrentLevel(SaveData.Level.East2);
        else if (levelName.Equals("East3"))
            SaveData.SetCurrentLevel(SaveData.Level.East3);
        else if (levelName.Equals("East4"))
            SaveData.SetCurrentLevel(SaveData.Level.East4);
        else if (levelName.Equals("East5"))
            SaveData.SetCurrentLevel(SaveData.Level.East5);
        else if (levelName.Equals("North1"))
            SaveData.SetCurrentLevel(SaveData.Level.North1);
        else if (levelName.Equals("North2"))
            SaveData.SetCurrentLevel(SaveData.Level.North2);
        else if (levelName.Equals("North3"))
            SaveData.SetCurrentLevel(SaveData.Level.North3);
        else if (levelName.Equals("North4"))
            SaveData.SetCurrentLevel(SaveData.Level.North4);
        else if (levelName.Equals("North5"))
            SaveData.SetCurrentLevel(SaveData.Level.North5);
        else if (levelName.Equals("Hub"))
            SaveData.SetCurrentLevel(SaveData.Level.Hub);
    }

    /// <summary>
    /// Checks if the save file exists
    /// </summary>
    /// <returns></returns>
    public bool SaveFileExists()
    {
        if (File.Exists(SaveFilePath))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Loads saved data
    /// </summary>
    public void LoadSaveData()
    {
        if (File.Exists(SaveFilePath))
        {
            byte[] data = File.ReadAllBytes(SaveFilePath);
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);
            SaveData = (SaveData)bf.Deserialize(ms);
            //Debug.Log("LOADED, Level = " + SaveData.GetCurrentLevel());
        }
    }
}
