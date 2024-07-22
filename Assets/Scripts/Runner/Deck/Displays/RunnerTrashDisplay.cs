using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Runner
{
    public class RunnerTrashDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _cardHolder;
        [SerializeField] private TextMeshProUGUI _cardCountDisplay;
        private List<RunnerCardDisplay> _cards = new List<RunnerCardDisplay>();
		private Vector3 _deckLocation;

		private void Awake()
		{
			_cardCountDisplay.text = "";
		}

		public void Setup(RunnerTrash trash, Vector3 deckLocation)
		{
			_deckLocation = deckLocation;
			trash.RegisterListeners(OnClear);
		}

		private void UpdateCardCount()
		{
			_cardCountDisplay.text = _cards.Count > 0 ? _cards.Count.ToString() : "";
		}
		public void AddCard(RunnerCardDisplay card)
        {
            card.transform.parent = _cardHolder;
			card.Scale(1f, 0.1f);
			card.MovePositionLocal(new Vector3(0f, _cards.Count * card.CardThickness, 0f), 0.1f);
			card.RotateInPlane(0f, 0.1f);
			_cards.Add(card);
			UpdateCardCount();
		}

		public RunnerCardDisplay RemoveCard(RunnerCardData data)
		{
			var removedCard = _cards.First(c => c.Data == data);
			if (removedCard != null)
			{
				_cards.Remove(removedCard);
			}
			UpdateCardCount();
			return removedCard;
		}

		public void OnClear()
		{
			StartCoroutine(ReturnToDeck());
		}

		private IEnumerator ReturnToDeck()
		{
			foreach (var card in _cards.ToArray())
			{
				card.MovePosition(_deckLocation, 0.1f);
				card.TurnFaceDown(0.1f);
				yield return new WaitForSeconds(0.05f);
			}
			foreach (var card in _cards.ToArray())
			{
				yield return new WaitForSeconds(0.05f);
				_cards.Remove(card);
				Destroy(card.gameObject);
			}
			UpdateCardCount();
		}
    }
}
