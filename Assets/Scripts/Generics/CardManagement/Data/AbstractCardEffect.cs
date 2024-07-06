using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
	public abstract class AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget> : ScriptableObject
		where TTargetData: AbstractTargetData<TTargetConfiguration, TTarget> 
		where TTargetConfiguration: AbstractTargetConfiguration<TTarget>
		where TTarget : System.Enum
	{
		public abstract bool IsValidTarget(TTargetData target);

		public abstract void Activate(TTargetData target, ICardManager manager);

	}
}