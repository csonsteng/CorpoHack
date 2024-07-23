using LogicPuddle.Common;
using Runner.Deck;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class RunnerUnlockedCardsList : ISerializable
    {
		private readonly HashSet<RunnerCardData> _cards = new();

		public IEnumerable<RunnerCardData> Cards => _cards.OrderBy(c => c.Name);
		public void UnlockCards(IEnumerable<RunnerCardData> cards)
		{
			foreach (RunnerCardData card in cards)
			{
				_cards.Add(card);
			}
		}
		public void UnlockCard(RunnerCardData card)
		{
			_cards.Add(card);
		}
		public void Deserialize(Dictionary<string, object> data)
		{
			_cards.Clear();
			foreach (var id in (List<object>)data["cards"])
			{
				if (UniqueScriptableObjectLinker.TryGetUniqueObject<RunnerCardData>(System.Convert.ToString(id), out var card))
				{
					_cards.Add(card);
				}
			}
		}

		public Dictionary<string, object> Serialize()
		{
			var cardIDs = new List<string>();
			foreach (var card in _cards)
			{
				cardIDs.Add(card.UniqueID);
			}
			return new Dictionary<string, object>
			{
				{ "cards" , cardIDs },
			};
		}
		public void Clear() => _cards.Clear();
	}
}