using Runner.Target;
using UnityEngine;

namespace Runner.Deck.Effects
{
	[CreateAssetMenu(menuName = "Runner/CardEffects/Corrupt")]
	public class Corrupt : AbstractRunnerCardEffect
	{
		protected override void OnActivate(RunnerTargetData target, RunnerDeckManager manager)
		{
			if (target.Strength > 0)
			{
				return;
			}
			if (target is not NodeData nodeData)
			{
				return;
			}
			nodeData.Corrupt();
		}

		public override bool IsValidTarget(RunnerTargetData target)
		{
			if (target.Strength > 0)
			{
				return false;
			}
			if (target is not NodeData nodeData)
			{
				return false;
			}
			return nodeData.CanBeCorrupted;
		}
	}
}
