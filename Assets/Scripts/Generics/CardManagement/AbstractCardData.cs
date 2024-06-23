using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public abstract class AbstractCardData<TRarity, TTarget> : ScriptableObject where TRarity : System.Enum where TTarget : System.Enum
    {
        public string Name;
        public int Cost;
        public Sprite Sprite;

        public TRarity Rarity;

        public List<AbstractCardEffect<TTarget>> Effects;

        public bool CanPlay(TTarget target)
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

        public void Play<T>(AbstractTargetData<T, TTarget> target) where T : AbstractTargetConfiguration<TTarget>
		{
            foreach (var effect in Effects)
			{
                effect.Activate(target);
			}
		}
    }
}
