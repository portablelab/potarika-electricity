

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class line2 : MonoBehaviour
{
    public GameObject others_disp1, others_disp2;   //表示の時に一時的に値が代入される
    public GameObject others_touch1, others_touch2; //タッチされたときに一時的に値が代入される
    Ray ray;                                        //タッチのためのやつ
    RaycastHit hit;                                 //光線が当たったときの情報が入ります
    public GameObject wire;                         //導線のオブジェクトを入れる
    public GameObject[] line1;                      //インスタンス化したオブジェクトが入る
    const int WIRE_NUM = 100;
    float[,] combined_resistance;
    int i = 0, j = 0,k=0;                               //i:導線の数カウント用　
 
    public bool touch_flag = false;                 //GameObject選択時にtrue
    string[] obj_name1, obj_name2;                  //タッチされたGmaeObjectの名前を保持する
    float x_disp1, x_disp2, y_disp1, y_disp2, z_disp1, z_disp2;         //表示の時に一時的に値が代入される
    float x_touch1, x_touch2, y_touch1, y_touch2, z_touch1, z_touch2;   //タッチされたときに一時的に値が代入される
    List<float> ResistanceArray;
    List<int> BatteryArray;
    List<List<string>> ConnectedPointArray;
    List<string> ConnectedPoint;
    List<string> TempArray;

    void Start()
    {
        line1 = new GameObject[WIRE_NUM];
        obj_name1 = new string[WIRE_NUM];
        obj_name2 = new string[WIRE_NUM];
        combined_resistance = new float[WIRE_NUM, WIRE_NUM * 2];
        ResistanceArray = new List<float>();
        BatteryArray = new List<int>();
        ConnectedPoint = new List<string>();
        ConnectedPointArray = new List<List<string>>();
    }

    void Update()
    {
        for (j = 0; j < i; j++)
        {
            if (line1[j].GetComponent<wire>().full_name[0] != "done")
            {
                ConnectedPoint.Add(line1[j].GetComponent<wire>().full_name[0]);
                ConnectedPoint.Add(line1[j].GetComponent<wire>().full_name[1]);

                for (k = j + 1; k < i; k++)
                {
                    foreach (string temp_name in ConnectedPoint)
                    {
                        if (line1[k].GetComponent<wire>().full_name[0] == temp_name)
                        {
                            ConnectedPoint.Add(line1[k].GetComponent<wire>().full_name[1]);
                            /*ここで追加済みのところを除去*/
                            line1[k].GetComponent<wire>().full_name[0] = "done";
                            line1[k].GetComponent<wire>().full_name[1] = "done";
                        }
                        else if (line1[k].GetComponent<wire>().full_name[1] == temp_name)
                        {
                            ConnectedPoint.Add(line1[k].GetComponent<wire>().full_name[0]);
                            /*ここで追加済みのところを除去*/
                            line1[k].GetComponent<wire>().full_name[0] = "done";
                            line1[k].GetComponent<wire>().full_name[1] = "done";
                        }
                    }
                }
                ConnectedPointArray.Add(ConnectedPoint);
            }
        }
            
        for (j = 0; j < ConnectedPointArray.Count; j++)
        {
            TempArray = ConnectedPointArray[j];
            if (ConnectedPointArray[j].Count <2)
            {
                /*不要なので消す*/
            }
            else if (ConnectedPointArray[j].Count == 2 && GameObject.Find(TempArray[0]).tag != "battery" && GameObject.Find(TempArray[1]).tag != "battery")
            {
                /*抵抗値を足す　→　足す前の抵抗値と置き換える*/
                /*不要になるので消す*/
            }
        }


        CheckClick();

        for (j = 0; j < i; j++)
        {
            if (!line1[j].transform.GetComponent<wire>().eliminate)
            {

                others_disp1 = GameObject.Find(obj_name1[j]).gameObject;	//未完成　これで参照している　タッチ時に取ってきた名前からゲームオブジェクトを参照する
                others_disp2 = GameObject.Find(obj_name2[j]).gameObject;	//あとで名前を参照できるように変える予定

                x_disp1 = others_disp1.transform.position.x;	//見にくいので代入しておく
                y_disp1 = others_disp1.transform.position.y;
                z_disp1 = others_disp1.transform.position.z;
                x_disp2 = others_disp2.transform.position.x;
                y_disp2 = others_disp2.transform.position.y;
                z_disp2 = others_disp2.transform.position.z;

                /*導線の位置決め*/
                line1[j].transform.position = new Vector3((x_disp1 + x_disp2) / 2.0f, (y_disp1 + y_disp2) / 2.0f, (z_disp1 + z_disp2) / 2.0f);	//ベクトルがわからない……orz
                line1[j].transform.eulerAngles = new Vector3(0, 0, 90f);//xz平面上での変換
                line1[j].transform.Rotate(new Vector3(-(float)(Math.Atan2((z_disp2 - z_disp1), (x_disp2 - x_disp1)) * (180.0 / 3.14)), 0, 0));
                line1[j].transform.Rotate(new Vector3(0, 0, 90f - (float)(Math.Atan2(Vector3.Distance(new Vector3(x_disp1, 0, z_disp1), new Vector3(x_disp2, 0, z_disp2)), (y_disp2 - y_disp1)) * (180.0 / 3.14))));

                line1[j].transform.localScale = new Vector3(10f, Vector3.Distance(others_disp1.transform.position, others_disp2.transform.position) / 2 + 5f, 10f);
            }
        }
    }
    protected virtual void CheckTouch()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended)
        {
            Vector2 point = touch.position;
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(point);

            if (Camera.main == null)
            {
                ray = Camera.current.ScreenPointToRay(point);
            }
            if (Physics.Raycast(ray, out hit))
            {
                for (j = 0; j < i; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == line1[j].gameObject.GetInstanceID() && !line1[j].transform.GetComponent<wire>().touch_flag)
                    {
                        line1[j].transform.GetComponent<wire>().touch_flag = true;
                    }
                    else if (hit.collider.gameObject.GetInstanceID() == line1[j].gameObject.GetInstanceID() && line1[j].transform.GetComponent<wire>().touch_flag)
                    {
                        line1[j].transform.GetComponent<wire>().eliminate = true;
                        hit.collider.gameObject.transform.localPosition = new Vector3(10000, 0, 0);
                        hit.collider.gameObject.transform.localScale = new Vector3(0, 0, 0);
                    }

                    else if (line1[j].transform.GetComponent<wire>().touch_flag)
                    {
                        line1[j].transform.GetComponent<wire>().touch_flag = false;
                    }
                }
                if (hit.collider.gameObject.name == "minus" || hit.collider.gameObject.name == "minus1" || hit.collider.gameObject.name == "minus2" || hit.collider.gameObject.name == "minus3" || hit.collider.gameObject.name == "plus")
                {
                    //物体に光線が衝突した時の挙動
                    if (touch_flag == false)  //一度目のタッチ  
                    {
                        touch_flag = true;
                        others_touch1 = hit.collider.gameObject;
                        //gameObject.transform.Find(others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name).gameObject.renderer.enabled = true;
                    }
                    else//もし一度タッチされていたら
                    {
                        touch_flag = false;
                        others_touch2 = hit.collider.gameObject;
                        //gameObject.transform.Find(others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name).gameObject.renderer.enabled = false;
                        obj_name1[i] = others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name;//親オブジェクト名/タッチされたオブジェクト名
                        obj_name2[i] = others_touch2.transform.parent.gameObject.name + "/" + others_touch2.name;

                        if (obj_name1[i] != obj_name2[i])
                        {

                            others_touch1 = GameObject.Find(obj_name1[i]).gameObject;	//未完成　これで参照している
                            others_touch2 = GameObject.Find(obj_name2[i]).gameObject;	//あとで名前を参照できるように変える予定

                            x_touch1 = others_touch1.transform.position.x;	//見にくいので代入しておく
                            y_touch1 = others_touch1.transform.position.y;
                            z_touch1 = others_touch1.transform.position.z;
                            x_touch2 = others_touch2.transform.position.x;
                            y_touch2 = others_touch2.transform.position.y;
                            z_touch2 = others_touch2.transform.position.z;

                            line1[i] = (GameObject)Instantiate(wire, new Vector3(1f, 1f, 1f), Quaternion.identity);     //導線をインスタンス化
                            line1[i].transform.position = new Vector3((x_touch1 + x_touch2) / 2, 0, (z_touch1 + z_touch2) / 2);	//ベクトルがわからない……orz 乾追記マーカのy座標が0なので線のy座標も0でよい
                            //GameObject.Destroy(line1[i]); 
                            i++;
                        }
                    }

                }
            }
            else
            {
                touch_flag = false;

            }
        }
    }
    protected virtual void CheckClick()
    {
        if (Input.GetMouseButtonDown(0) == false)
        {
            return;
        }
        //        Touch touch = Input.GetTouch(0);

        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            for (j = 0; j < i; j++)
            {
                if (hit.collider.gameObject.GetInstanceID() == line1[j].gameObject.GetInstanceID() && !line1[j].transform.GetComponent<wire>().touch_flag)
                {
                    line1[j].transform.GetComponent<wire>().touch_flag = true;
                }
                else if (hit.collider.gameObject.GetInstanceID() == line1[j].gameObject.GetInstanceID() && line1[j].transform.GetComponent<wire>().touch_flag)
                {
                    line1[j].transform.GetComponent<wire>().eliminate = true;
                    hit.collider.gameObject.transform.localPosition = new Vector3(10000, 0, 0);
                    hit.collider.gameObject.transform.localScale = new Vector3(0, 0, 0);
                }

                else if (line1[j].transform.GetComponent<wire>().touch_flag)
                {
                    line1[j].transform.GetComponent<wire>().touch_flag = false;
                }
            }
            if (hit.collider.gameObject.name == "minus" || hit.collider.gameObject.name == "minus1" || hit.collider.gameObject.name == "minus2" || hit.collider.gameObject.name == "minus3" || hit.collider.gameObject.name == "plus")
            {
                //物体に光線が衝突した時の挙動
                if (touch_flag == false)  //一度目のタッチ  
                {
                    touch_flag = true;
                    others_touch1 = hit.collider.gameObject;
                    //gameObject.transform.Find(others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name).gameObject.renderer.enabled = true;
                }
                else//もし一度タッチされていたら
                {
                    touch_flag = false;
                    others_touch2 = hit.collider.gameObject;
                    //gameObject.transform.Find(others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name).gameObject.renderer.enabled = false;
                    obj_name1[i] = others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name;//親オブジェクト名/タッチされたオブジェクト名
                    obj_name2[i] = others_touch2.transform.parent.gameObject.name + "/" + others_touch2.name;

                    if (obj_name1[i] != obj_name2[i] && others_touch1.transform.parent.gameObject.name != others_touch2.transform.parent.gameObject.name)
                    {

                        others_touch1 = GameObject.Find(obj_name1[i]).gameObject;	//未完成　これで参照している
                        others_touch2 = GameObject.Find(obj_name2[i]).gameObject;	//あとで名前を参照できるように変える予定

                        x_touch1 = others_touch1.transform.position.x;	//見にくいので代入しておく
                        y_touch1 = others_touch1.transform.position.y;
                        z_touch1 = others_touch1.transform.position.z;
                        x_touch2 = others_touch2.transform.position.x;
                        y_touch2 = others_touch2.transform.position.y;
                        z_touch2 = others_touch2.transform.position.z;

                        line1[i] = (GameObject)Instantiate(wire, new Vector3(1f, 1f, 1f), Quaternion.identity);     //導線をインスタンス化
                        line1[i].transform.position = new Vector3((x_touch1 + x_touch2) / 2, 0, (z_touch1 + z_touch2) / 2);	//ベクトルがわからない……orz 乾追記マーカのy座標が0なので線のy座標も0でよい
                        line1[i].GetComponent<wire>().parent_name[0] = others_touch1.transform.parent.gameObject.name;
                        line1[i].GetComponent<wire>().parent_name[1] = others_touch2.transform.parent.gameObject.name;
                        line1[i].GetComponent<wire>().full_name[0] = others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name;
                        line1[i].GetComponent<wire>().full_name[1] = others_touch2.transform.parent.gameObject.name + "/" + others_touch2.name;
                        //GameObject.Destroy(line1[i]); 
                        i++;
                    }
                }

            }
        }
        else
        {
            touch_flag = false;

        }
    }
}
class ParallelCombinedResistance
{
    public string[] resistance_name;
    public  float resistance;

    ParallelCombinedResistance()
    {
        resistance_name = new string[2];
    }
}
class SeriesCombinedResistance
{
    public string[] resistance_name;
    public float resistance;

    SeriesCombinedResistance()
    {
        resistance_name = new string[2];
    }
}