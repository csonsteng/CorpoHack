using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.Common
{
    public abstract class UniqueScriptableObject : ScriptableObject
    {
        [SerializeField] private string _uniqueID;
        public string UniqueID => _uniqueID;


    }
}