using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
	[CreateAssetMenu(menuName = "Runner/Node/ServerIPsNode")]
	public class ServerIPsNode : AbstractNodeType
	{
		public override bool CanBeDownloaded => true;
		public override void Download()
		{
			Logger.Log("Download Server IPs");
		}
	}
}
