using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runner.Target
{
	public enum IceTrigger
	{
		SelfPinged,
		OtherPinged,
		NodePinged,
		SelfBroken,
		OtherBroken,
		NodeBroken,
		DetectionAt15
	}

	public abstract class AbstractIceType : UniqueScriptableObject
	{
		[FormerlySerializedAs("_name")][SerializeField] public string Name;
		[SerializeField] public string Description;
		[SerializeField] private List<IceTrigger> _invalidTriggers = new List<IceTrigger>();
        public abstract void OnTrigger();

		public IceTrigger GetRandomValidTrigger()
		{
			var validTriggers = new List<IceTrigger>();
			foreach (IceTrigger trigger in System.Enum.GetValues(typeof(IceTrigger)))
			{
				if (_invalidTriggers.Contains(trigger))
				{
					continue;
				}
				validTriggers.Add(trigger);
			}
			validTriggers.Shuffle();
			return validTriggers[0];
		}

    }
}