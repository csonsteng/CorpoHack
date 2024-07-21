using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
	[CreateAssetMenu(menuName = "Runner/Node/HoneyPot")]
	public class HoneyPotNode : AbstractNodeType
	{
		public override void WhenBroken()
		{
			Debug.Log("Fire Honey Pot");
		}
	}
}
