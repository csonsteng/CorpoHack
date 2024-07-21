using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
	[CreateAssetMenu(menuName = "Runner/Node/UserData")]
	public class UserDataNode : AbstractNodeType
	{
		public override bool CanBeDownloaded => true;
		public override void Download()
		{
			Logger.Log("Download User Data");
		}

		public override bool CanBeCorrupted => true;
		public override void Corrupt()
		{
			Logger.Log("Corrupt User Data");
		}
	}
}
