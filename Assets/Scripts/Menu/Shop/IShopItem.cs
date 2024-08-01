using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopItem
{
    public string ItemName();
    public string Effect();
    public string Type();
    public int GetCost();

    public void Buy();
}
