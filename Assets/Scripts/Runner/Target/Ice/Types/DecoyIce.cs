using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Decoy")]
	public class DecoyIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// does nothing
			Debug.Log($"trigger {Name}");
		}
	}
}
