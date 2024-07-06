using LogicPuddle.CardManagement;
using LogicPuddle.Common;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck
{
    [CreateAssetMenu(menuName = "Runner/Card Data")]
    public class RunnerCardData: UniqueScriptableObject
	{
		public string Name;
		public string Description;
		public int Cost;
		public Sprite Sprite;

		public RunnerCardRarity Rarity;

		public List<AbstractRunnerCardEffect> Effects;
		public RunnerTargetColor Color;
        public int CardStrength;

		public void Play(RunnerTargetData target, ICardManager manager)
		{
			target.Damage(CardStrength); 
			foreach (var effect in Effects)
			{
				effect.Activate(target, manager);
			}
		}

		public bool CanPlay(RunnerTargetData target)
		{
			if (target.Strength > 0)
			{
				if(Color == RunnerTargetColor.None || Color == target.Color)
				{
					return true;	// at the very least can damang
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