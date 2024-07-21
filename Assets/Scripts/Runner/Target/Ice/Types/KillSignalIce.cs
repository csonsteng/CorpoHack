using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{

	[CreateAssetMenu(menuName = "Runner/Ice/KillSignal")]
	public class KillSignalIce : AbstractIceType
	{
		public override void OnTrigger()
		{
			// force discards player hand
			Debug.Log($"trigger {Name}");
			var currentHand = RunnerDeckManager.Instance.Hand.GetAll();
			foreach ( var card in currentHand.ToArray())
			{
				RunnerDeckManager.Instance.Discard(card, false);
			}
			RunnerDeckManager.Instance.AutofillHand();
		}
	}
}
