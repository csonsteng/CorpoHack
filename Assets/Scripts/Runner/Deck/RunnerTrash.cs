using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using LogicPuddle.Common;

namespace Runner.Deck
{
    public class RunnerTrash : ISerializable
	{
		private readonly RunnerCardContainer _cards = new();


		public List<RunnerCardData> GetAll() => _cards;
		public void Add(RunnerCardData card) => _cards.AddToTop(card);   // adds to top since "face up"
		public void Remove(RunnerCardData card) => _cards.RemoveCard(card);
		public void Clear() => _cards.Clear();


		public void Deserialize(Dictionary<string, object> data)
		{
			_cards.Deserialize((Dictionary<string, object>)data["cards"]);
		}

		public Dictionary<string, object> Serialize()
		{
			return new Dictionary<string, object>
			{
				{ "cards" , _cards.Serialize() },
			};
		}
	}
}