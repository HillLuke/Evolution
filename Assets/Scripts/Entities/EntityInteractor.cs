using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityInteractor : MonoBehaviour
    {
        public System.Action ActoinInRange;

        [ShowInInspector, ReadOnly]
        public bool isInInteractRange;

        [ShowInInspector, ReadOnly]
        private EntityProperties _entityProperties;

        [ShowInInspector, ReadOnly]
        private GameObject _interactGoal;

        [ShowInInspector, ReadOnly]
        private SphereCollider _sphereCollider;

        [ShowInInspector, ReadOnly]
        private List<GameObject> _triggers;

        public void Init(EntityProperties entityProperties)
        {
            _entityProperties = entityProperties;
            _sphereCollider.radius = _entityProperties.InteractRange;
        }

        public void SetInteractTarget(GameObject interactGoal)
        {
            _interactGoal = interactGoal;
        }

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            isInInteractRange = false;
            _triggers = new List<GameObject>();
            if (_sphereCollider == null)
            {
                throw new ArgumentNullException(nameof(SphereCollider));
            }
        }

        private void FixedUpdate()
        {
            if (_interactGoal != null)
            {
                foreach (var trigger in _triggers)
                {
                    if (GameObject.ReferenceEquals(_interactGoal, trigger))
                    {
                        Debug.Log("Is in range");

                        if (ActoinInRange != null)
                        {
                            Debug.Log("Is in range raised");
                            ActoinInRange.Invoke();
                            _interactGoal = null;
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggers.Contains(other.gameObject))
            {
                _triggers.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggers.Contains(other.gameObject))
            {
                _triggers.Remove(other.gameObject);
            }
        }
    }
}