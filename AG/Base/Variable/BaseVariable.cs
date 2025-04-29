using System;
using UnityEngine;

namespace AG.Base.Variable
{
    public class BaseVariable<T> : ScriptableObject
    {
        [SerializeField] private T _value = default;
        private event Action<T> _onValueChanged;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    _onValueChanged?.Invoke(_value);
                }
            }
        }

        public void AddListenerToValueChange(Action<T> onValueChanged)
        {
            _onValueChanged += onValueChanged;
        }

        public void RemoveListenerFromValueChange(Action<T> onValueChanged)
        {
            _onValueChanged -= onValueChanged;
        }
    }
}