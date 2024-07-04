using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
	public class RunnerCardDisplayManager : AbstractCardDisplayManager<
		RunnerHand, 
		RunnerCardData, 
		RunnerCardDisplay, 
		RunnerHandDisplay, 
		RunnerCardRarity,
		AbstractRunnerCardEffect, 
		RunnerTargetManager, 
		RunnerTargetDisplay,
		RunnerTargetData, 
		RunnerTargetConfiguration, 
		RunnerTargetType>
	{
	}
}
