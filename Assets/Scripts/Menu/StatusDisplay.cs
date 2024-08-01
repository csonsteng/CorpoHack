using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cryptoDisplay;

	private void Start()
	{
		RunnerDeckManager.Instance.CryptoChanged += OnCryptoChanged;
		OnCryptoChanged(RunnerDeckManager.Instance.Crypto);
	}
	private void OnDestroy()
	{
		RunnerDeckManager.Instance.CryptoChanged -= OnCryptoChanged;
	}

	private void OnCryptoChanged(int amount)
	{
		_cryptoDisplay.text = $"Crypto: {amount}";
	}
}
