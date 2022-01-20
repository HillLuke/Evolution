using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour, ISpawnable, ISpawnAnimationEvents
{
    public float Value { get { return _value; } }
    public EResourceType ResourceType { get { return _resourceProperties.ResourceType; } }

    [SerializeField]
    private float _value;

    [SerializeField]
    private ResourceSO _resourceProperties;

    [SerializeField]
    private Animator _animator;

    void Start()
    {
        Invoke("Spawn", 1);
    }

    private void Awake()
    {
        _value = _resourceProperties?.MaxValue ?? throw new NullReferenceException(nameof(Resource));
        _animator = GetComponent<Animator>() ?? null;

        if (_animator)
        {
            gameObject.transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        DestroyIfValueIsZero();
    }

    public float Use(float amount)
    {
        if (Value > 0)
        {
            float use = _value - amount;
            Mathf.Clamp(use, 0, _resourceProperties.MaxValue);

            return use;
        }

        return 0;
    }

    private void DestroyIfValueIsZero()
    {
        if (_value <= 0)
        {
            Destroy(gameObject);
        }
    }

    #region ISpawnable

    public void Spawn()
    {
        if (_animator)
        {
            _animator.SetTrigger("Spawn");
        }
    }

    #endregion

    #region ISpawnAnimationEvents

    public void SpawnStart()
    {

    }

    public void SpawnEnd()
    {
        if (_animator)
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }

    #endregion
}
