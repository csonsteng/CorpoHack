using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Nullify")]
	public class NullifyIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// disables all copies of a card until it is hack
			Debug.Log($"trigger {Name}");
		}
	}
}
