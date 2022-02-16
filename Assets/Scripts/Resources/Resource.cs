using Assets.Scripts.Interfaces;
using Assets.Scripts.ScriptableObjects.Identity;
using Assets.Scripts.ScriptableObjects.Resources;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    [Serializable]
    public class Resource : MonoBehaviour, IResource, ISpawnable, ISpawnAnimationEvents, IMonitorable, IIdentity
    {
        [ShowInInspector]
        public EResourceType ResourceType
        { get { return _resourceProperties.ResourceType; } }

        [ShowInInspector]
        public float Value
        { get { return _value; } }

        private Animator _animator;

        private bool _isSelected;

        [Required, SerializeReference]
        private ResourceSO _resourceProperties;

        private float _value;

        public void DeSelect()
        {
            _isSelected = false;
        }

        public string GetData()
        {
            return $"Type : {_resourceProperties.ResourceType}\nValue: {_value.ToString("#.##")}";
        }

        public IdentitySO GetIdentity()
        {
            return _resourceProperties.Identity;
        }

        public virtual string GetName()
        {
            return _resourceProperties.name;
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void Spawn()
        {
            if (_animator)
            {
                _animator.SetTrigger("Spawn");
            }
        }

        public void SpawnEnd()
        {
            if (_animator)
            {
                gameObject.transform.localScale = Vector3.one;
            }
        }

        public void SpawnStart()
        {
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

        private void DestroyIfValueIsZero()
        {
            if (_value <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Invoke("Spawn", 1);
        }

        private void Update()
        {
            DestroyIfValueIsZero();
        }
    }
}