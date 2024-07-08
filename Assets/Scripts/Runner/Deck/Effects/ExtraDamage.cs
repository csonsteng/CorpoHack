
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck.Effects
{
	[CreateAssetMenu(menuName = "Runner/CardEffects/ExtraDamage")]
	public class ExtraDamage : AbstractRunnerCardEffect
	{
		public RunnerTargetColor TargetColor;

		protected override void OnActivate(RunnerTargetData target, RunnerDeckManager manager)
		{
			target.Damage();
		}

		public override bool IsValidTarget(RunnerTargetData target)
		{
			if (target.TargetType == RunnerTargetType.None)
			{
				return false;
			}
			if (target.Strength <= 0)
			{
				return false;
			}
			if (TargetColor == RunnerTargetColor.None)
			{
				return true;
			}
			return TargetColor == target.Color;
		}
	}
}