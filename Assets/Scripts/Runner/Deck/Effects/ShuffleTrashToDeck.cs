using Runner.Target;
using LogicPuddle.CardManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck.Effects
{
	[CreateAssetMenu(menuName = "Runner/CardEffects/ShuffleTrashToDeck")]
	public class ShuffleTrashToDeck : AbstractRunnerCardEffect
	{
		public override void Activate(RunnerTargetData target, ICardManager manager)
		{
			Debug.Log("shuffle trash to deck");
			manager.ShuffleTrashIntoDeck();
		}
	}
}
