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
    }
}