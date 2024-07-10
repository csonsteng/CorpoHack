using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/Alarm")]
	public class AlarmIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// raise detection level
			Debug.Log($"trigger {Name}");
			DetectionTracker.Instance.IncreaseDetection(5);
		}
	}
}
