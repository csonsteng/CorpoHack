using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Castle")]
	public class CastleIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// raise strength of all securities
			Debug.Log($"trigger {Name}");
			foreach (var target in RunnerTargetManager.Instance.GetAllData())
			{
				if (target.Strength == 0 )
				{
					continue;
				}
				target.Damage(-1);
			}
		}
	}
}
