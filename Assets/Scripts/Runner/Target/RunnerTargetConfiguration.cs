using LogicPuddle.CardManagement;
using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
    [CreateAssetMenu(menuName = "Runner/Target Configuration")]
    public class RunnerTargetConfiguration : UniqueScriptableObject
	{
		public string Name;
		public RunnerTargetType TargetType;
		public GameObject Prefab;
    }
}