using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/TakeOver")]
	public class TakeOverIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// auto-plays 3 cards
			Debug.Log($"trigger {Name}");
		}
	}
}
