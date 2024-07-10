using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Eject")]
	public class EjectIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// re-enables firewall
			Debug.Log($"trigger {Name}");
			RunnerTargetManager.Instance.ReEnableFirewall();
		}
	}
}
