using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace dps.ui
{
    public class AbilityButton : MonoBehaviour
    {
        public string KeybindHint = "1";
        private Material _bar = null;
        [SerializeField] private Text _text = null;
        [SerializeField] private Text _keybindHint = null;
        private Image _barImage = null;
        [SerializeField] private GameObject _buttonGO;


        void Awake()
        {
            _barImage = _buttonGO.GetComponent<Image>();
            _bar = Instantiate(_barImage.material);
            _barImage.material = _bar;
        }

        public void SetCdProgress(float __progress, float timeLeft)
        {
            if (timeLeft <= 0f)
            {
                _text.text = "";
                _keybindHint.text = KeybindHint;
            }
            else
            {
                _text.text = string.Format("{0:0.0}s", timeLeft);
                _keybindHint.text = "";
            }
            _bar.SetFloat("_Progress", __progress);
        }
    }
}
