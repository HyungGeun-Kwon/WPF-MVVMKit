using System;
using System.Collections.Generic;

namespace MVVMKit.Dialogs
{
    public class DialogParameters
    {
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            _parameters[key] = value;
        }

        public object this[string key] => _parameters[key];

        public T GetValue<T>(string key)
        {
            if (!_parameters.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Parameter with key '{key}' was not found.");
            }
            if (_parameters[key] is T tValue)
            {
                return tValue;
            }

            throw new InvalidCastException($"Cannot convert parameter '{key}' to {typeof(T).Name}");
        }

        public object GetValue(string key)
        {
            if (!_parameters.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Parameter with key '{key}' was not found.");
            }
            return _parameters[key];
        }

        public bool ContainsKey(string key) => _parameters.ContainsKey(key);
    }
}
