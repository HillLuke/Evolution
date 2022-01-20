using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInteractor : MonoBehaviour
{
    private SphereCollider _sphereCollider;
    private EntityProperties _entityProperties;
    private GameObject _interactGoal;
    private List<GameObject> _triggers;

    public bool isInInteractRange;
    public Action ActoinInRange;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        isInInteractRange = false;
        _triggers = new List<GameObject>();
    }

    public void Init(EntityProperties entityProperties)
    {
        _entityProperties = entityProperties;
        _sphereCollider.radius = _entityProperties.InteractRange;
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

    public void SetInteractTarget(GameObject interactGoal)
    {
        _interactGoal = interactGoal;
    }
}
