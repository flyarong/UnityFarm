﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour {

    private List<Item> Items = new List<Item>();

    public GameObject Store_Item_UI_Prefab;
    public UIGrid Grid_Items;

    private static StoreManager instance = null;

    public static StoreManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(StoreManager)) as StoreManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("StoreManager ");
            instance = obj.AddComponent(typeof(StoreManager)) as StoreManager;

            Debug.Log("Fail to get StoreManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;

        Item test = new Item();
        test.ID = 1;
        test.type = ITEM_TYPE.BUILDING;
        test.Name = "밭";
        test.Sprite_Name = "Farm";
        test.Model_Name = "Farm";
        test.Price = 100;
        test.Buff_Happy = 10;
        test.Buff_Speed = 0;

        Items.Add(test);

        Item test2 = new Item();
        test2.ID = 2;
        test2.type = ITEM_TYPE.BUILDING;
        test2.Name = "집";
        test2.Sprite_Name = "House_1";
        test2.Model_Name = "House_1";
        test2.Price = 100;
        test2.Buff_Happy = 0;
        test2.Buff_Speed = 0;

        Items.Add(test2);

        for (int i = 0; i < Items.Count; i++)
        {
            Create_Store_Item_UI(Items[i]);
        }
    }

    void Create_Store_Item_UI(Item item_info)
    {
        GameObject ItemUI = Instantiate(Store_Item_UI_Prefab, Grid_Items.transform) as GameObject;
        ItemUI.transform.localScale = Vector3.one;
        ItemUI.name = item_info.ID.ToString();

        ItemUI.GetComponent<Store_Item_Action>().Set_Item_Info(item_info);

        Grid_Items.repositionNow = true;
    }

    public void View_StoreUI()
    {
        GetComponent<UIPanel>().alpha = 1;
        GameManager.Get_Inctance().Set_ViewUI();
    }
    public void NotView_StroeUI()
    {
        GetComponent<UIPanel>().alpha = 0;
        GameManager.Get_Inctance().Set_NotViewUI();
    }
}
public enum ITEM_TYPE
{
    NONE = -1,
    BUILDING = 0,
    LANDSCAPING,
}
public class Item
{
    public int ID;
    public ITEM_TYPE type;
    public string Name;
    public string Sprite_Name;
    public string Model_Name;
    public int Price;
    public int Buff_Happy;
    public int Buff_Speed;
}

