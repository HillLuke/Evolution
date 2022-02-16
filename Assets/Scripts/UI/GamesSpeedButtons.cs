using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GamesSpeedButtons : MonoBehaviour
    {
        public Toggle DefaultOn;

        public void Start()
        {
            DefaultOn.Select();
        }
    }
}