using LogicPuddle.Common;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public abstract class AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : UniqueScriptableObject
        where TRarity : System.Enum
        where TEffect: AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
        where TTargetData: AbstractTargetData<TTargetConfiguration, TTarget>
        where TTargetConfiguration: AbstractTargetConfiguration<TTarget>
        where TTarget : System.Enum
    {
        public string Name;
        public string Description;
        public int Cost;
        public Sprite Sprite;

        public TRarity Rarity;

        public List<TEffect> Effects;

        public virtual bool CanPlay(TTargetData target)
		{
            foreach (var effect in Effects)
			{
                if (effect.IsValidTarget(target))
				{
                    return true;
				}
			}
            return false;
		}

        public virtual void Play(TTargetData target, ICardManager manager)
		{
			Debug.Log("card was played");
            
			foreach (var effect in Effects)
			{
                effect.Activate(target, manager);
			}
		}
    }
}
