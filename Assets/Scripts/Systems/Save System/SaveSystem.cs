using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{

    public static SaveSystem Instance;

    private const string SaveFileName = "Save.dat";
    public SaveData SaveData;
    
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
            LoadSaveData();

    }    

    
    public static string SaveFilePath { get { return Path.Combine(Application.persistentDataPath, SaveFileName); } }

    /// <summary>
    /// Saves the game data
    /// </summary>
    /// <param name="saveData"></param>
    public void SaveStats()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        bf.Serialize(stream, SaveData);
        File.WriteAllBytes(SaveFilePath, stream.GetBuffer());
        //Debug.Log("Saved Data, CurrentLevel =" + SaveData.GetCurrentLevel());
    }

    public void SaveAbilities()
    {
        SaveData.SetAbilityArray(Globals.Player.GetComponent<AbilityController>().AbilityArray);
        SaveStats();
    }

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
        else if (levelName.Equals("Hub"))
                SaveData.SetCurrentLevel(SaveData.Level.Hub);

        SaveStats();
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
