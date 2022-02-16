using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Utility
{
    [RequireComponent(typeof(Collider))]
    public class ColliderEventRaiser : MonoBehaviour
    {
        public UnityEvent<Collider> TriggerEnter = new UnityEvent<Collider>();
        public UnityEvent<Collider> TriggerExit = new UnityEvent<Collider>();

        [ShowInInspector, ReadOnly]
        private Collider _collider;

        [ShowInInspector, ReadOnly]
        private List<GameObject> _triggers;

        public void SetSize(float size)
        {
            if (_collider.GetType() == typeof(SphereCollider))
            {
                ((SphereCollider)_collider).radius = size;
            }
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _triggers = new List<GameObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggers.Contains(other.gameObject))
            {
                _triggers.Add(other.gameObject);
            }
            if (TriggerEnter != null)
            {
                TriggerEnter.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggers.Contains(other.gameObject))
            {
                _triggers.Remove(other.gameObject);
            }
            if (TriggerExit != null)
            {
                TriggerExit.Invoke(other);
            }
        }
    }
}