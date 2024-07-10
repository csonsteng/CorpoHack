using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Scramble")]
	public class ScrambleIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// shuffles all securities
			Debug.Log($"trigger {Name}");
		}
	}
}
