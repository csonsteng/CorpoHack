using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Trap")]
	public class TrapIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// puts a random virus in the players hand
			Debug.Log($"trigger {Name}");
		}
	}
}
