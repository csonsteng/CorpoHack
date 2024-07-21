using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck.Effects
{
	[CreateAssetMenu(menuName = "Runner/CardEffects/PingData")]
	public class PingData : AbstractRunnerCardEffect
	{

		protected override void OnActivate(RunnerTargetData target, RunnerDeckManager manager)
		{
			RunnerTargetManager.Instance.PingTarget(target);
		}

		public override bool IsValidTarget(RunnerTargetData target)
		{
			if (target.TargetType == RunnerTargetType.None || target.TargetType == RunnerTargetType.Firewall)
			{
				return false;
			}
			if (target.Strength <= 0)
			{
				return false;
			}
			if (target.Pinged)
			{
				return false;
			}
			return true;
		}
	}
}
