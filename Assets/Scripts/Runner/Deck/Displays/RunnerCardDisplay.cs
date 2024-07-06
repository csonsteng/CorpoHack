using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    public class RunnerCardDisplay : AbstractCardDisplay<RunnerCardData, RunnerCardRarity, AbstractRunnerCardEffect, RunnerTargetData, RunnerTargetConfiguration, RunnerTargetType>
    {
        [SerializeField] private TextMeshProUGUI _strength;
		[SerializeField] private TextMeshProUGUI _cost;
		[SerializeField] private Image _color;

		[SerializeField] private Color _blue;
		[SerializeField] private Color _green;
		[SerializeField] private Color _red;
		[SerializeField] private Color _colorless;

		public override void Setup(RunnerCardData card, Vector3 position)
		{
			base.Setup(card, position);
			_strength.text = card.CardStrength.ToString();
			_cost.text = card.Cost.ToString();
			_color.color = GetColor(card);

		}

		private Color GetColor(RunnerCardData card)
		{
			return card.Color switch
			{
				RunnerTargetColor.Blue => _blue,
				RunnerTargetColor.Red => _red,
				RunnerTargetColor.Green => _green,
				_ => _colorless

			};
		}
	}
}