using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTextGenerator : MonoBehaviour {

    public List<string> text = new List<string>(); 

    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
        int rand = Random.Range(0, text.Count);
        _text.text = text[rand];
    }
}
