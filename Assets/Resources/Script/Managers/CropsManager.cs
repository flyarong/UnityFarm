﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class CropsManager : MonoBehaviour
{ 
    List<CropInfo> CropsInfo = new List<CropInfo>();

    public UIGrid Grid_Select_Crops;
    public GameObject Select_Crop_UI_Prefab;

    private static CropsManager instance = null;

    public static CropsManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(CropsManager)) as CropsManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("CropsManager ");
            instance = obj.AddComponent(typeof(CropsManager)) as CropsManager;

            Debug.Log("Fail to get CropsManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
        Get_DB_CropInfo();
    }

    public CropInfo Get_CropInfo(int find_id)
    {
        for (int i = 0; i < CropsInfo.Count; i++)
        {
            if (CropsInfo[i].ID == find_id)
            {
                return CropsInfo[i];
            }
        }

        return null;
    }

    void Set_SelectCropUI(CropInfo info)
    {
        if(info.Is_Farmming == false) { return; }

        GameObject obj = Instantiate(Select_Crop_UI_Prefab, Grid_Select_Crops.transform) as GameObject;
        obj.transform.localScale = Vector3.one;
        obj.name = info.Name;
        obj.GetComponent<Select_CropsButton_Action>().Set_Crop_info(info);

        Grid_Select_Crops.repositionNow = true;
    }

    public void Get_DB_CropInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_CropInfo");

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_DB_CropInfo));
    }
    public void Reply_DB_CropInfo(string json)
    {
        // JsonReader.Deserialize() : 원하는 자료형의 json을 만들 수 있다
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            CropInfo data = JsonReader.Deserialize<CropInfo>(JsonWriter.Serialize(info.Value));
            CropsInfo.Add(data);
            Set_SelectCropUI(data);
        }
    }
}
public class CropInfo
{
    public int ID;
    public bool Is_Farmming;
    public string Name;
    public string Sprite_Name;
    public int Grow_Time;
    public int Selling_Price;
    public int Price;
}