using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudBarController : MonoBehaviour {

    private Material _bar = null;
    private Material _secondaryBar = null;
    private Image _barImage = null;
    private Image _secondaryBarImage = null;
    [SerializeField]    
    private GameObject _barGO;
    [SerializeField]
    private GameObject _secondarBarGO;

    public float Progress
    {
        get { return _progress; }

        set
        {
            if (value < 0f || value > 1f)
            {
                _progress = 0f;
                return;
            }

            _progress = value;
            SetBarProgress(_progress, _secondary);
        }
    }

    public float SecondaryProgress
    {
        get { return _secondary; }

        set
        {
            if (value < 0f || value > 1f)
            {
                _secondary = 0;
                return;
            }

            _secondary = value;
            SetBarProgress(_progress, _secondary);
        }
    }

    private float _secondary = 0f;
    private float _progress = 1f;

    private void Update()
    {
        if (_secondary > 0)
        {
            SecondaryProgress = SecondaryProgress - (Time.deltaTime * 0.4f);
            
        }
    }

    void Awake()
    {
        _barImage = _barGO.GetComponent<Image>();
        _secondaryBarImage = _secondarBarGO.GetComponent<Image>();
        _secondaryBar = Instantiate(_secondaryBarImage.material);
        _bar = Instantiate(_barImage.material);
        _barImage.material = _bar;
        _secondaryBarImage.material = _secondaryBar;
        SetBarProgress(Progress);
    }


    private void SetBarProgress(float __progress, float __segmentTwoProgress = 0f)
    {
        _bar.SetFloat("_Progress", __progress);
        //_bar.SetFloat("_SegmentTwo", __segmentTwoProgress);

        _secondaryBar.SetFloat("_Progress", __progress);
        _secondaryBar.SetFloat("_SegmentTwo", __segmentTwoProgress);
    }
}
