using Runner.Target;
using LogicPuddle.CardManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck.Effects
{
	[CreateAssetMenu(menuName = "Runner/CardEffects/DrawCard")]
	public class DrawCard : AbstractRunnerCardEffect
	{
		public override void Activate(RunnerTargetData target, RunnerDeckManager manager)
		{
			manager.DrawCard();
		}

		public override bool IsValidTarget(RunnerTargetData target) => false;
	}
}
