using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreen : MonoBehaviour
{
    [SerializeField] private GameObject _container;
	[SerializeField] private GameObject _moreInfoScreen;
	[SerializeField] private bool _disable;

	private void Awake()
	{
		_container.SetActive(!_disable);
		_moreInfoScreen.SetActive(false);
	}
	public void Close()
    {
        _container.SetActive(false);
    }

	public void Back()
	{
		_moreInfoScreen.SetActive(false);
	}
	public void MoreInfo()
	{
		_moreInfoScreen.SetActive(true);
	}
}
