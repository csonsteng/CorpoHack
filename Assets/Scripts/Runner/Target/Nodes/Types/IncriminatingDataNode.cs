using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
	[CreateAssetMenu(menuName = "Runner/Node/IncriminatingData")]
	public class IncriminatingDataNode : AbstractNodeType
	{
		public override bool CanBeDownloaded => true;
		public override void Download()
		{
			Logger.Log("Download Incriminating Data");
		}
	}
}
