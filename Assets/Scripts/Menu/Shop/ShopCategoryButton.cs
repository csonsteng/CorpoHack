using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCategoryButton : MonoBehaviour
{

    [SerializeField] private ShopCategory _category;
	[SerializeField] private BasicShop _shop;

    public void OnClick()
	{
		_shop.OnCategorySelected(_category);
	}
}
