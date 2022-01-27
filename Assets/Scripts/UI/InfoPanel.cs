using Assets.Scripts.Interfaces;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InfoPanel : MonoBehaviour
    {
        public SelectedWorldItem selectedWorldItem;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Data;
        public GameObject Panel;

        private IMonitorable _monitorable;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //create a ray cast and set it to the mouses cursor position in game
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.DrawLine(ray.origin, hit.point);

                    if (_monitorable != null)
                    {
                        _monitorable.DeSelect();
                    }

                    _monitorable = hit.transform.gameObject.GetComponent<IMonitorable>();

                    if (_monitorable != null)
                    {
                        selectedWorldItem.Follow(hit.transform.gameObject);
                        Title.SetText(hit.transform.gameObject.name);
                        Panel.SetActive(true);
                    }
                    else
                    {
                        selectedWorldItem.Clear();
                        Panel.SetActive(false);
                    }
                }
            }

            if (_monitorable != null)
            {
                Data.SetText(_monitorable.GetData());
            }
        }
    }
}
