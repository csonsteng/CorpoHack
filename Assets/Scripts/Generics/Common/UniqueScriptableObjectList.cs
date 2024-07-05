using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.Common
{
    public abstract class UniqueScriptableObjectList<TObjectType>: ScriptableObject, IUniqueScriptableObjectList where TObjectType : UniqueScriptableObject
    {
        [SerializeField] private List<TObjectType> _managedObjects = new ();

        public Type GetTargetType() => typeof(TObjectType);

        private Dictionary<string, TObjectType> _dictionary;

        public List<TObjectType> GetAll()
        {
            var allObjects = new List<TObjectType>();
            allObjects.AddRange(_managedObjects);
            return allObjects;
        }

        public void BuildDictionary()
		{
            _dictionary = new Dictionary<string, TObjectType>();
            foreach (var managedObject in _managedObjects)
			{
                _dictionary.Add(managedObject.UniqueID, managedObject);
			}
		}

        public bool TryGetUniqueObject<T>(string id, out T foundObject) where T: UniqueScriptableObject
        {
            if (!_dictionary.TryGetValue(id, out var typedFoundObject))
			{
                foundObject = null;
                return false;
			}

            foundObject = typedFoundObject as T;
            return true;
        }

    }
}