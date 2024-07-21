using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
	[CreateAssetMenu(menuName = "Runner/Node/CryptoWallet")]
	public class CryptoWalletNode : AbstractNodeType
	{
		public override bool CanBeSiphoned => true;
		public override void Siphon()
		{
			Logger.Log("Siphon");
		}
	}
}
