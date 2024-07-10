using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/ScapeGoat")]
	public class ScapeGoatIce: AbstractIceType
	{
		public override void OnTrigger()
		{
			// transfers its security elsewhere
			Debug.Log($"trigger {Name}");
		}
	}
}
