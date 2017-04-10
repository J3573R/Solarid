using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    [SerializeField]
    private GameObject _cooldownGO;
    [SerializeField]
    private Sprite _blinkSprite;
    [SerializeField]
    private Sprite _vortexSprite;
    [SerializeField]
    private Sprite _cloneSprite;

    private GameObject _selectedSkill;
    private Gun _playerGun;
    private Image _smallBlink;
    private Image _smallVortex;
    private Image _smallClone;
    private AbilityController _abilityController;
    private Image _cooldownImage;
    private Image _blackScreen;
    private Material _cooldownMaterial;
    private bool _initDone = false;
    private Text _bulletText;

    // Use this for initialization
    void Start () {
        init();
    }

    internal void init()
    {
        if (!_initDone)
        {
            _cooldownImage = _cooldownGO.GetComponent<Image>();
            _cooldownMaterial = Instantiate(_cooldownImage.material);
            _cooldownImage.material = _cooldownMaterial;
            _smallBlink = GameObject.Find("BlinkSmall").GetComponent<Image>();
            _smallVortex = GameObject.Find("VortexSmall").GetComponent<Image>();
            _smallClone = GameObject.Find("CloneSmall").GetComponent<Image>();
            _bulletText = GameObject.Find("BulletsRemaining").GetComponent<Text>();
            _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
            _selectedSkill = GameObject.Find("SelectedSkill");
            _playerGun = Globals.Player.GetComponent<Player>().Gun;

            _smallBlink.enabled = false;
            _smallClone.enabled = false;
            _smallVortex.enabled = false;

            _abilityController = Globals.Player.GetComponent<AbilityController>();
            _initDone = true;
        }        
    }

    /// <summary>
    /// Fades the screen to black
    /// </summary>
    public void FadeScreenToBlack()
    {

    }

    /// <summary>
    /// Fades the screen to visible
    /// </summary>
    public void FadeScreenToVisible()
    {

    }

    // Update is called once per frame
    void Update () {

        float progress = _abilityController.GetCurrentCooldownProgress();
        progress = 1 - progress;

        _cooldownMaterial.SetFloat("_Progress", progress);

        UpdateBulletCount();
    }

    private void UpdateBulletCount()
    {
        if (!_playerGun.Reloading)
        {
            String tmp = (_playerGun.BulletsRemaining + " / " + _playerGun.ClipSize);
            _bulletText.text = tmp;
        } else
        {
            _bulletText.text = "Reloading";
        }
    }

    /// <summary>
    /// Changes the ability icon, called from AbilityController
    /// </summary>
    /// <param name="ability"></param>
    public void ChangeImage(AbilityController.Ability ability)
    {
        if (ability == AbilityController.Ability.Blink)
        {
            _cooldownImage.sprite = _blinkSprite;
            _selectedSkill.transform.position = _smallBlink.transform.position;
        }            
        else if (ability == AbilityController.Ability.Vortex)
        {
            _cooldownImage.sprite = _vortexSprite;
            _selectedSkill.transform.position = _smallVortex.transform.position;
        }            
        else if (ability == AbilityController.Ability.Clone)
        {
            _cooldownImage.sprite = _cloneSprite;
            _selectedSkill.transform.position = _smallClone.transform.position;
        }           
        
    }

    /// <summary>
    /// Enables small skill icons
    /// </summary>
    /// <param name="ability"></param>
    public void EnableSmallIcon(AbilityController.Ability ability)
    {
        if (ability == AbilityController.Ability.Blink)
        {            
            _smallBlink.enabled = true;
        }
        else if (ability == AbilityController.Ability.Vortex)
        {
            _smallVortex.enabled = true;
        }
        else if (ability == AbilityController.Ability.Clone)
        {
            _smallClone.enabled = true;
        }
        _selectedSkill.SetActive(true);
        _cooldownImage.enabled = true;
    }

    /// <summary>
    /// Disables bunch of stuff from UI if all abilites are disabled
    /// </summary>
    /// <param name="state"></param>
    public void AllAbilitesDisabled(bool state)
    {
        if (state)
        {
            _selectedSkill.SetActive(false);
            _smallBlink.enabled = false;
            _smallClone.enabled = false;
            _smallVortex.enabled = false;
            _cooldownImage.enabled = false;
        } 
    }
}
