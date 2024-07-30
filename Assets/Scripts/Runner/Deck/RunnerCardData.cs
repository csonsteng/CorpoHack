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
    public class RunnerCardData: UniqueScriptableObject, IShopItem
	{
		public string Name;
		public string Description;
		public Sprite Sprite;

		public RunnerCardRarity Rarity;

		public List<AbstractRunnerCardEffect> Effects;


		public string ItemName() => Name;

		public void Play(RunnerTargetData target, RunnerDeckManager manager)
		{
			target.Damage();
			DetectionTracker.Instance.IncreaseDetection(1);
			foreach (var effect in Effects)
			{
				effect.Activate(target, manager);
			}
		}

		public bool CanPlay(RunnerTargetData target)
		{
			if (target.Strength > 0)
			{
				return true;	// at the very least can damage
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