using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck
{
    [CreateAssetMenu(menuName = "Runner/Card Data")]
    public class RunnerCardData : AbstractCardData<RunnerCardRarity, AbstractRunnerCardEffect, RunnerTargetData, RunnerTargetConfiguration, RunnerTargetType>
    {
        public RunnerTargetColor Color;
        public int CardStrength;

		public override void Play(RunnerTargetData target, ICardManager manager)
		{
			target.Damage(CardStrength);
			base.Play(target, manager);
		}

		public override bool CanPlay(RunnerTargetData target)
		{
			if (target.Strength > 0)
			{
				if(Color == RunnerTargetColor.None || Color == target.Color)
				{
					return true;	// at the very least can damage it.
				}
			}
			foreach (var effect in Effects)
			{
				if (effect.IsValidTarget(target))
				{
					return true;
				}
			}
			return false;
		}
	}
}