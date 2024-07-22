using Runner.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{

    public class BaseRunnerCardDisplay : MonoBehaviour
    {
		[SerializeField] protected Image _cardImage;

		[SerializeField] protected Image _cardFrame;
		[SerializeField] protected Image _cardBacking;
		[SerializeField] protected TextMeshProUGUI _name;
		[SerializeField] protected TextMeshProUGUI _description;

		protected RunnerCardData _data;
		public virtual void Setup(RunnerCardData card)
		{
			_data = card;
			_cardImage.sprite = card.Sprite;
			_name.text = card.Name;
			_description.text = card.Description;

			//_cardBacking.sprite = deckConfiguration.Backing;

			// todo : card frame based on rarity?

		}
	}
}
