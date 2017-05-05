

using UnityEngine;
using System.Collections;
using System;


public class line : MonoBehaviour
{
	public GameObject others_disp1, others_disp2;   //表示の時に一時的に値が代入される
	public GameObject others_touch1, others_touch2; //タッチされたときに一時的に値が代入される
	Ray ray;                                        //タッチのためのやつ
	RaycastHit hit;                                 //光線が当たったときの情報が入ります
	public GameObject wire;                         //導線のオブジェクトを入れる
	public GameObject[] line1;                      //インスタンス化したオブジェクトが入る
	const int WIRE_NUM=100;
    float[,] combined_resistance;
	int i = 0, j = 0;                               //i:導線の数カウント用　
    public GUISkin mySkin;
    public bool touch_flag = false;                 //GameObject選択時にtrue
    public bool touch_flag2 = false;                 //GameObject選択時にtrue
	string[] obj_name1, obj_name2;                  //タッチされたGmaeObjectの名前を保持する
	float x_disp1, x_disp2, y_disp1, y_disp2, z_disp1, z_disp2;         //表示の時に一時的に値が代入される
	float x_touch1, x_touch2, y_touch1, y_touch2, z_touch1, z_touch2;   //タッチされたときに一時的に値が代入される

    GUIStyle st = new GUIStyle();

	void Start()
	{

        st.fontSize = 30;
		line1 = new GameObject[WIRE_NUM];
		obj_name1 = new string[WIRE_NUM];
        obj_name2 = new string[WIRE_NUM];
        combined_resistance = new float[WIRE_NUM,WIRE_NUM*2];
	}
	
	void Update()
	{
        //for (j = 0; j < i; j++)
        //{
        //    if(gameObject.transform.Find(line1[i].GetComponent<wire>().name1).tag == "battery")j++;
        //    if(gameObject.transform.Find(line1[i].GetComponent<wire>().name2).tag == "battery")j++;

        //}
        CheckClick();
//		CheckTouch();                                                   //タッチのチェック、タッチ時の挙動を定義
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

                line1[j].transform.localScale = new Vector3(10f, Vector3.Distance(others_disp1.transform.position, others_disp2.transform.position) / 2 + 5.0f, 10f);
            }
		}
	}

    void OnGUI()
    {
        GUI.skin = mySkin;
        st = GUI.skin.button;
        st.fontSize = Screen.width / 80;
        //if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 100), "どうせんを\nけす", st))
        if (GUI.Button(new Rect(Screen.width * 13 / 15, 10, Screen.width / 15, Screen.width / 20), "どうせん\nをけす", st))
        {
            for (j = 0; j < i; j++)
            {
                line1[j].transform.GetComponent<wire>().eliminate = true;
                line1[j].transform.localPosition = new Vector3(10000, 0, 0);
                line1[j].transform.localScale = new Vector3(0, 0, 0);
            }
            i = 0;
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
        if (Input.GetMouseButtonDown(0) == false && Input.GetMouseButtonUp(0) == false)
        {
            return;
        }
//        Touch touch = Input.GetTouch(0);

            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                for(j=0;j<i;j++){
                    if (hit.collider.gameObject.GetInstanceID() == line1[j].gameObject.GetInstanceID() && !line1[j].transform.GetComponent<wire>().touch_flag)
                    {
                        line1[j].transform.GetComponent<wire>().touch_flag = true;
                        line1[j].gameObject.renderer.material.color = new Color(200.0f / 255.0f, 255.0f / 255.0f, 50.0f / 255.0f, 180.0f / 255.0f);
                    }
                    else if (hit.collider.gameObject.GetInstanceID() == line1[j].gameObject.GetInstanceID() && line1[j].transform.GetComponent<wire>().touch_flag && touch_flag2 == false)
                    {
                        line1[j].transform.GetComponent<wire>().eliminate = true;
                        hit.collider.gameObject.transform.localPosition = new Vector3(10000, 0, 0);
                        hit.collider.gameObject.transform.localScale = new Vector3(0, 0, 0);
                    }

                    else if (line1[j].transform.GetComponent<wire>().touch_flag && touch_flag2 == false)
                    {
                        line1[j].transform.GetComponent<wire>().touch_flag = false;
                        line1[j].gameObject.renderer.material.color = new Color(1.0f,1.0f,1.0f,1.0f);
                    }
                }
                if (hit.collider.gameObject.name == "minus" || hit.collider.gameObject.name == "minus1" || hit.collider.gameObject.name == "minus2" || hit.collider.gameObject.name == "minus3" || hit.collider.gameObject.name == "plus")
                {
                    //物体に光線が衝突した時の挙動
                    if (touch_flag == false && touch_flag2 == false)  //一度目のタッチ  
                    {
                        touch_flag = true;
                        others_touch1 = hit.collider.gameObject;

                        others_touch1.gameObject.renderer.material.color = new Color(20.0f / 255.0f, 25.0f / 255.0f, 50.0f / 255.0f, 180.0f/255.0f);
                        //gameObject.transform.Find(others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name).gameObject.renderer.enabled = true;
                    }
                    else if(touch_flag2 == false || hit.collider.gameObject.GetInstanceID() != others_touch1.gameObject.GetInstanceID())//もし一度タッチされていたら
                    {
                        if (others_touch1.gameObject.transform.parent.tag == "battery" || others_touch1.gameObject.transform.parent.tag == "ammeter" || others_touch1.gameObject.transform.parent.tag == "vmeter")
                        {
                            if (others_touch1.gameObject.name == "plus")
                            {
                                others_touch1.gameObject.renderer.material.color = new Color(255.0f / 255.0f, 71.0f / 255.0f, 71.0f / 255.0f, 150.0f / 255.0f);
                            }
                            else
                            {
                                others_touch1.gameObject.renderer.material.color = new Color(64.0f / 255.0f, 148.0f / 255.0f, 255.0f / 255.0f, 150.0f / 255.0f);
                            }
                        }
                        else
                        {
                            others_touch1.gameObject.renderer.material.color = new Color(34.0f / 255.0f, 248.0f / 255.0f, 34.0f / 255.0f, 150.0f / 255.0f);
                        }
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
                            line1[i].GetComponent<wire>().full_name[0] = others_touch1.transform.parent.gameObject.name + "/" + others_touch1.name;
                            line1[i].GetComponent<wire>().full_name[1] = others_touch2.transform.parent.gameObject.name + "/" + others_touch2.name;
                            //GameObject.Destroy(line1[i]); 
                            i++;
                        }
                    }

                }
                else if(touch_flag)
                {
                    touch_flag = false;
                    if (others_touch1.gameObject.name == "plus")
                    {
                        others_touch1.gameObject.renderer.material.color = new Color(255.0f / 255.0f, 71.0f / 255.0f, 71.0f / 255.0f, 150.0f / 255.0f);
                    }
                    else
                    {
                        others_touch1.gameObject.renderer.material.color = new Color(64.0f / 255.0f, 148.0f / 255.0f, 255.0f / 255.0f, 150.0f / 255.0f);
                    }
                }
            }
            else if (touch_flag)
            {
                touch_flag = false;
                if (others_touch1.gameObject.name == "plus")
                {
                    others_touch1.gameObject.renderer.material.color = new Color(255.0f / 255.0f, 71.0f / 255.0f, 71.0f / 255.0f, 150.0f / 255.0f);
                }
                else
                {
                    others_touch1.gameObject.renderer.material.color = new Color(64.0f / 255.0f, 148.0f / 255.0f, 255.0f / 255.0f, 150.0f / 255.0f);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                touch_flag2 = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touch_flag2 = false;
            }
    }
}
