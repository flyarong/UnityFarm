﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private int User_Index;

    public bool Is_ViewUI = false;
    private static GameManager instance = null;

    public static GameManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(GameManager)) as GameManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("GameManager ");
            instance = obj.AddComponent(typeof(GameManager)) as GameManager;

            Debug.Log("Fail to get GameManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void Start_Update()
    {
        StartCoroutine(C_Update());
    }
    IEnumerator C_Update()
    {
        while (true)
        {
            if (Is_ViewUI) { yield return null; continue; }

            if (Input.GetMouseButtonDown(0))
            {
                // 카메라에서 화면상의 마우스 좌표에 해당하는 공간으로 레이를 쏜다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Physics.Raycast(쏜 레이 정보, 충돌 정보, 거리)
                //  => 충돌이 되면 true를 리턴하면서 충돌 정보를 확인 할 수 있다.
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    GameObject obj = hit.collider.gameObject;

                    if (obj.CompareTag("EventOBJ"))
                    { 
                        obj.GetComponent<BulidingOBJ_Action>().Start_Action();
                    }
                }

            }

            yield return null;
        }
    }

    public void Plant_Drag_Farm()
    {
        if (Select_Crops_Manager.Get_Inctance().Select_Crop_ID == -1) { return; }

        StopCoroutine("C_Update");
        StartCoroutine(C_Plant_Drag_Farm());
    }
    IEnumerator C_Plant_Drag_Farm()
    {
        while (true)
        {
            if (Is_ViewUI) { yield return null; continue; }

            if (Input.GetMouseButton(0))
            {
                // 카메라에서 화면상의 마우스 좌표에 해당하는 공간으로 레이를 쏜다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Physics.Raycast(쏜 레이 정보, 충돌 정보, 거리)
                //  => 충돌이 되면 true를 리턴하면서 충돌 정보를 확인 할 수 있다.
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                    GameObject obj = hit.collider.gameObject;

                    if (obj.name.Contains("Farm"))
                    {
                        obj.GetComponent<Farm_Action>().Plant_Crop();
                    }
                }
            }

            yield return null;

        }
    }

    public void Install_Item(GameObject obj)
    {
        StopAllCoroutines();
        StartCoroutine(C_Install_Item(obj));
    }
    IEnumerator C_Install_Item(GameObject obj)
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 카메라에서 화면상의 마우스 좌표에 해당하는 공간으로 레이를 쏜다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Physics.Raycast(쏜 레이 정보, 충돌 정보, 거리)
                //  => 충돌이 되면 true를 리턴하면서 충돌 정보를 확인 할 수 있다.
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == obj)
                    {
                        Set_ViewUI();

                        while (Input.GetMouseButton(0))
                        {
                            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit1;

                            if (Physics.Raycast(ray1, out hit1, Mathf.Infinity))
                            {
                                Vector3 pos = hit1.collider.transform.position;
                                pos.y = 0.06f;
                                obj.transform.position = pos;
                            }

                            yield return null;
                        }
                    }
                    else
                    {
                        Set_NotViewUI();
                    }
                }
            }
            yield return null;
        }
    }

    public void Set_BasicSetting()
    {
        StopAllCoroutines();
        Is_ViewUI = false;
        StartCoroutine("C_Update");

        Camera_Action.Get_Inctance().Set_CameraMoving();
    }

    public string Set_Text_Time(int time)
    {
        string text_time = "";
        int second = (int)time % 60;
        int Minute = (int)time / 60;
        int Hour = Minute / 60;

        if (Hour != 0)
        {
            text_time += Hour.ToString() + "시 ";
        }
        if (Minute != 0)
        {
            text_time += Minute.ToString() + "분 ";
        }

            text_time += second.ToString() + "초";

        return text_time;
    }

    public void Set_ViewUI()
    {
        Is_ViewUI = true;
        Camera_Action.Get_Inctance().Set_NotCameraMoving();
    }
    public void Set_NotViewUI()
    {
        Is_ViewUI = false;
        Camera_Action.Get_Inctance().Set_CameraMoving();
    }

    public void Set_Message(string text)
    {

    }

    public void Set_UserIndex(int value)
    {
        User_Index = value;
    }
    public int Get_UserIndex()
    {
        return User_Index;
    }
}
