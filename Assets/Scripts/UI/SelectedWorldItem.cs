using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SelectedWorldItem : MonoBehaviour
    {
        private GameObject _follow;

        public void Follow(GameObject gameObject)
        {
            _follow = gameObject;
        }

        public void Clear()
        {
            _follow = null;
        }

        private void Update()
        {

            if (_follow != null)
            {
                var pos = Camera.main.WorldToScreenPoint(_follow.transform.position);
                pos.y += 5;
                gameObject.transform.position = pos;
                gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
            }

        }
    }
}