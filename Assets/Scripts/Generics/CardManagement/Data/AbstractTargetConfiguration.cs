using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class AbstractTargetConfiguration<TTarget> : UniqueScriptableObject where TTarget : System.Enum
    {
        public string Name;
        public TTarget TargetType;
        public GameObject Prefab;
    }
}
