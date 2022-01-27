﻿using Assets.Scripts.Interfaces;
using Assets.Scripts.ScriptableObjects.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Resource
{
    public class Resource : MonoBehaviour, ISpawnable, ISpawnAnimationEvents, IMonitorable
    {
        public float Value { get { return _value; } }
        public EResourceType ResourceType { get { return _resourceProperties.ResourceType; } }

        [SerializeField]
        private float _value;

        [SerializeField]
        private ResourceSO _resourceProperties;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private bool _isSelected;

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
            _isSelected = false;
        }

        private void Update()
        {
            DestroyIfValueIsZero();
        }

        public float Use(float amount)
        {
            if (_value >= amount)
            {
                _value -= amount;
                return amount;
            }
            else
            {
                var actual = _value % amount;
                _value -= actual;
                return actual;
            }

            //if (Value > 0)
            //{
            //    float use = _value - amount;
            //    _value -= use;
            //    Mathf.Clamp(use, 0, _resourceProperties.MaxValue);

            //    return use;
            //}

            //return 0;
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


        #region IMonitorable

        public void Select()
        {
            _isSelected = true;
        }

        public void DeSelect()
        {
            _isSelected = false;
        }

        public string GetData()
        {
            return $"Name : {_resourceProperties.name}\nType : {_resourceProperties.ResourceType}\nValue: {_value.ToString("#.##")}";
        }

        #endregion
    }
}