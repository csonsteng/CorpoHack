using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.Common
{
    public interface IUniqueScriptableObjectList
    {
        public System.Type GetTargetType();

        public void BuildDictionary();

        public bool TryGetUniqueObject<T>(string id, out T foundObject) where T : UniqueScriptableObject;
    }
}