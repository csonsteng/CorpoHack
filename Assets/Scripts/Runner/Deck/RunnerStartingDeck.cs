using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;

namespace Runner.Deck { 
	[CreateAssetMenu(menuName = "Runner/Starting Deck")]
	public class RunnerStartingDeck : AbstractBaseDeck<RunnerCardData, RunnerCardRarity, AbstractRunnerCardEffect, RunnerTargetData, RunnerTargetConfiguration, RunnerTargetType>
	{
		public Sprite BackingImage;
		public override Sprite Backing => BackingImage;
	}
}