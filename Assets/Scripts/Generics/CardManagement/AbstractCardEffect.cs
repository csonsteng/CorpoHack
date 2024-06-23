using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
	public abstract class AbstractCardEffect<TTarget> : ScriptableObject where TTarget : System.Enum
	{
		protected abstract List<TTarget> _validTargets { get; set; }

		public bool IsValidTarget(TTarget targetType) => _validTargets != null && _validTargets.Contains(targetType);

		public abstract void Activate<T>(AbstractTargetData<T, TTarget> target) where T: AbstractTargetConfiguration<TTarget>;

	}
}