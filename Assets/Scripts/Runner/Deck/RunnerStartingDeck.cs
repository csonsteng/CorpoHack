using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using LogicPuddle.Common;

namespace Runner.Deck { 
	[CreateAssetMenu(menuName = "Runner/Starting Deck")]
	public class RunnerStartingDeck : UniqueScriptableObject, IDeckConfiguration
	{
		public Sprite BackingImage;
		public Sprite Backing => BackingImage;
		public string Name;
		public List<RunnerCardData> Cards;

	}
}