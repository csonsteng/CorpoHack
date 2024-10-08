using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BasicShop : MonoBehaviour
{
    [SerializeField] private BasicShopItem _itemTemplate;
    [SerializeField] private BascisShopList _inventory;


	[SerializeField] private GameObject _detailsDisplay;
	[SerializeField] private TextMeshProUGUI _selectedTitle;
	[SerializeField] private TextMeshProUGUI _selectedType;
	[SerializeField] private TextMeshProUGUI _selectedCost;
	[SerializeField] private TextMeshProUGUI _selectedDescription;
	[SerializeField] private Button _buyButton;

	private Dictionary<ShopCategory, List<IShopItem>> _categorizedItems = new Dictionary<ShopCategory, List<IShopItem>>();

	private List<GameObject> _spawnedItems = new List<GameObject>();

	private IShopItem _selected;

	private void Awake()
	{
		_itemTemplate.gameObject.SetActive(false);
		_detailsDisplay.SetActive(false);
		RunnerDeckManager.Instance.CryptoChanged += OnCryptoChanged;
	}

	private void Start()
	{
		foreach (var component in _inventory.Components())
		{
			if (component is RunnerRigRAM)
			{
				AddToCategory(ShopCategory.RAM, component);
				continue;
			}
			if (component is AbstractRunnerRigDrive)
			{
				AddToCategory(ShopCategory.Drives, component);
				continue;
			}
			if (component is RunnerRigProcessor)
			{
				AddToCategory(ShopCategory.CPUs, component);
				continue;
			}
			if (component is RunnerRigMotherboard)
			{
				AddToCategory(ShopCategory.Boards, component);
				continue;
			}
			if (component is RunnerRigCooling)
			{
				AddToCategory(ShopCategory.Cooling, component);
				continue;
			}
			if (component is AbstractRunnerRigInterfaceComponent)
			{
				AddToCategory(ShopCategory.Interfaces, component);
				continue;
			}
			AddToCategory(ShopCategory.Misc, component);
		}

		foreach (var program in _inventory.Programs())
		{
			AddToCategory(ShopCategory.Programs, program);
		}
	}

    private void AddToCategory(ShopCategory category, IShopItem item)
	{
        if (!_categorizedItems.TryGetValue(category, out var list))
		{
			list = new List<IShopItem>();
			_categorizedItems[category] = list;
		}
		list.Add(item);
	}
	public void OnCategorySelected(ShopCategory category)
	{
		foreach (var item in _spawnedItems)
		{
			Destroy(item);
		}

		if (!_categorizedItems.ContainsKey(category))
		{
			Debug.LogError($"no items in {category}");
			return;
		}

		foreach (var item in _categorizedItems[category])
		{
			var itemDisplay = Instantiate(_itemTemplate, _itemTemplate.transform.parent);
			itemDisplay.gameObject.SetActive(true);
			_spawnedItems.Add(itemDisplay.gameObject);
			itemDisplay.Setup(item, i =>
			{
				Select(i);
			});
		}
	}

	private void Select(IShopItem item)
	{
		_selected = item;
		_selectedTitle.text = item.ItemName();
		_selectedCost.text = item.GetCost().ToString();
		_selectedDescription.text = item.Effect();
		_selectedType.text = item.Type();


		_buyButton.interactable = RunnerDeckManager.Instance.Crypto > item.GetCost();
		_detailsDisplay.SetActive(true);
	}

	private void OnCryptoChanged(int amount)
	{
		if (_selected == null)
		{
			return;
		}
		_buyButton.interactable = RunnerDeckManager.Instance.Crypto > _selected.GetCost();
	}

	private void OnDestroy()
	{
		RunnerDeckManager.Instance.CryptoChanged -= OnCryptoChanged;
	}

	public void Buy()
	{
		if (RunnerDeckManager.Instance.SpendCrypto(_selected.GetCost()))
		{
			_selected.Buy();
			Debug.Log($"Buying {_selected.ItemName()}");
		}
	}
}

public enum ShopCategory
{
    RAM,
    Drives,
    CPUs,
    Boards,
    Cooling,
    Interfaces,
    Programs,
    Misc
}