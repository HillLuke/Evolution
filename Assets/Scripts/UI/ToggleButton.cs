using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleButton : MonoBehaviour
    {
        private Toggle _toggle;
        private Image _image;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _image = GetComponent<Image>();

            _toggle.onValueChanged.AddListener((x) => _image.color = x ? _toggle.colors.pressedColor : _toggle.colors.normalColor);
        }
    }
}