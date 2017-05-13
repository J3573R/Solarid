using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateCredits : GameStateBase
{

    private Image _blackScreen;
    private CreditScroll[] _texts;
    private bool _ending;

    void Awake()
    {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.enabled = true;
        _texts = FindObjectsOfType<CreditScroll>();
        _blackScreen.CrossFadeAlpha(0, 2, false);
        _ending = false;
    }

    public void End()
    {
        if (_ending) return;
        _ending = true;
        _blackScreen.CrossFadeAlpha(1, 2, false);
        foreach (var text in _texts)
        {
            text.End();
        }
        StartCoroutine(ChangeLevel());
    }

    IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(2);
        SaveSystem.Instance.ResetSave();
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.MainMenu, "MainMenu");
    }
}
