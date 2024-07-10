using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Stealth")]
	public class StealthIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// fully cloaks a node
			Debug.Log($"trigger {Name}");
		}
	}
}
