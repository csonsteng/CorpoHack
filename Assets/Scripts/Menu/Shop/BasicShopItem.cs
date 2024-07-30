using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _image;

    private Action<IShopItem> _onClickCallback;
    private IShopItem _target;

    public void Setup(IShopItem target, Action<IShopItem> onClickCallback)
	{
        _target = target;
        _onClickCallback = onClickCallback;
        _name.text = target.ItemName();
	}

    public void OnClick()
	{
        _onClickCallback?.Invoke(_target);
	}

}
