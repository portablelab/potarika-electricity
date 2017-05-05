using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Indication : MonoBehaviour {

	public GUISkin mySkin;

	float num1 = 0.0f;
    float num2 = 0.0f;
    GameObject ammeter, vmeter;
    float time=0,dt;
    float[] amperage;
    float amperage_sum;
    float[] voltage;
    float voltage_sum;
    int i;
    int num = -2; // 課題実験用変数

    string str;
    private string pathtxt = "";
    public Texture2D chara;
    public Texture2D[] kairo;
    public string file_path = "/textData.txt";
    public string textdata;
    public string[] splittextdata;
    int touch_count = 0;
    char[] separate;
    string aaa = "☆";
    int kadai_dai = 1;
    int kadai_syou = 1;
    TextAsset txtxt;
    int texture_num=0;
	// Use this for initialization
	void Start () {
        splittextdata = new string[100];
        separate = new char[2];
        touch_count = 0;
        amperage = new float[10];
        voltage = new float[10];
        kairo = new Texture2D[3];
        num = Eclipse.CallFuncB();
        //num = 221;

        if (num == 0)
        {
            txtxt = Resources.Load("kadai_text/tutorial") as TextAsset;
            kairo[0] = Resources.Load("gazou/tutorial") as Texture2D;
            ReadTextData();
            separate = aaa.ToCharArray();
            splittextdata = textdata.Split(separate, 100);
        }
        else if (num > 0)
        {
            kadai_dai = num / 100;              //章番号
            kadai_syou = (num - kadai_dai*100) / 10;  //節番号
            file_path = "kadai_text/kadai_0" + kadai_dai + "_0" + kadai_syou ;//ファイルパス指定
            print(file_path);
            txtxt = Resources.Load(file_path) as TextAsset;
            file_path = "gazou/kadai_0" + kadai_dai + "_0" + kadai_syou;//ファイルパス指定
            for (i = 0; i < 3; i++)
            {
                kairo[i] = Resources.Load(file_path + "_" + i) as Texture2D;
            }
            ReadTextData();                     //テキストデータ読み込み（textdataにテキストデータを代入）
            separate = aaa.ToCharArray();       //区切り文字の設定
            splittextdata = textdata.Split(separate, 100);//区切り文字ごとに区切る
        }

        for (i = 0; i < 10; i++)
        {
            amperage[i] = 0;
            voltage[i] = 0;
        }


	}
	
	// Update is called once per frame
	void Update () {

        CheckClick();
        dt = Time.deltaTime;
        time += dt;
        if (time > 0.5)
        {
            time = 0;
            ammeter = GameObject.FindGameObjectWithTag("ammeter");
            vmeter = GameObject.FindGameObjectWithTag("vmeter");
            if (ammeter)
            {
                amperage_sum = 0;
                for (i = 0; i < 9; i++)
                {
                    amperage_sum += amperage[i];
                    amperage[i] = amperage[i + 1];
                }
                amperage[9] = ammeter.GetComponent<parts>().amperage;
                amperage_sum += amperage[0];
                if (Math.Abs(amperage[9] - amperage_sum / 10.0f) < 0.1 && Math.Abs(amperage[9] - amperage[0]) < 0.1)
                {
                    num2 = ((float)(int)(amperage_sum * 100.0f)) / 1000.0f;
                }
                else
                {
                    num2 = 0;
                }
            }
            if (vmeter)
            {
                voltage_sum = 0;
                for (i = 0; i < 9; i++)
                {
                    voltage_sum += voltage[i];
                    voltage[i] = voltage[i + 1];
                }
                voltage[9] = vmeter.GetComponent<parts>().voltage;
                voltage_sum += voltage[0];
                if (Math.Abs(voltage[9] - voltage_sum / 10.0f) < 0.1 && Math.Abs(voltage[9] - voltage[0]) < 0.1)
                {
                    num1 = ((float)(int)(voltage_sum * 100.0f)) / 1000.0f;
                }
                else
                {
                    num1 = 0;
                }
            }
        }
    }

	void OnGUI()
    {
        //GUI.TextArea(new Rect(5, 5, Screen.width, 50), pathtxt);

        // 現在使用中のスキンにスキンを割り当てます。
        GUI.skin = mySkin;
        GUI.skin.box.fontSize = Screen.height / 20;

		// ボタンを作成します。 これにより、デフォルトの''ボタン''スタイルが mySkin に割り当てたスキンから作成されます。
        if (Math.Abs(amperage[9] - amperage_sum / 10.0f) < 0.1 && Math.Abs(amperage[9] - amperage[0]) < 0.1
            && Math.Abs(voltage[9] - voltage_sum / 10.0f) < 0.1 && Math.Abs(voltage[9] - voltage[0]) < 0.1)
        {
            if (Math.Abs(num2) >= 1.0)
            {
                GUI.Label(new Rect(10, 10, Screen.width / 5, Screen.height / 5), "電圧計:" + num1 + " V \n電流計:" + num2  + " A");
            }
            else
            {
                GUI.Label(new Rect(10, 10, Screen.width / 5, Screen.height / 5), "電圧計:" + num1 + " V \n電流計:" + (int)(num2 * 1000.0 )+ " mA");
            }
        }
        else{
            GUI.Label(new Rect(10, 10, Screen.width / 5, Screen.height / 5), "計算中...");
        }
        /* === 課題実験（ここから） === */
        if (num >= 0 && splittextdata.Length > touch_count)
        {
            // ぽーたくん表示
            GUI.DrawTexture(new Rect(Screen.width / 4 * 3, Screen.height - Screen.height / 3 - 10, Screen.width / 4, Screen.height / 3), chara, ScaleMode.ScaleToFit);

            if (num == 0)
            {// tutorial.txt
                
                //    GUI.Label(new Rect(10, 50, Screen.width / 5, Screen.height / 5), "" + num + "");
                GUI.DrawTexture(new Rect(0, Screen.height / 5 + 30, Screen.width / 4, Screen.height / 4), kairo[0], ScaleMode.ScaleToFit);
                GUI.Box(new Rect(10, Screen.height - Screen.height / 5 - 10, Screen.width / 4 * 3, Screen.height / 5), new GUIContent(splittextdata[touch_count]));//touch_count + " " + textdata));//"これはテキストです"));
            }
            else if(num > 0){
                GUI.Box(new Rect(10, Screen.height - Screen.height / 5 - 10, Screen.width / 4 * 3, Screen.height / 5), new GUIContent(splittextdata[touch_count]));//touch_count + " " + textdata));//"これはテキストです"));

                GUI.DrawTexture(new Rect(0, Screen.height / 5, (float)Screen.height  / (3.0f * 0.6f), Screen.height / 3), kairo[texture_num], ScaleMode.StretchToFill);
            }
            /*else if (num == 131)
            {// kadai_01_03.txt

            }
            else if (num == 132)
            {

            }
            else if (num == 211)
            {// kadai_02_01.txt

            }
            else if (num == 212)
            {

            }
            else if (num == 213)
            {

            }
            else if (num == 221)
            {// kadai_02_02.txt

            }
            else if (num == 222)
            {

            }
            else if (num == 223)
            {

            }
            else if (num == 224)
            {

            }
            else if (num == 225)
            {

            }
            else if (num == 226)
            {

            }
            else if (num == 227)
            {

            }
            else if (num == 231)
            {// kadai_02_03.txt

            }
            else if (num == 232)
            {

            }
            else if (num == 233)
            {

            }
            else if (num == 234)
            {

            }
            else if (num == 235)
            {

            }
            else if (num == 236)
            {

            }
            else if (num == 311)
            {// kadai_03_01.txt

            }
            else if (num == 312)
            {

            }
            else if (num == 321)
            {// kadai_03_02.txt

            }
            else if (num == 322)
            {

            }*/
        }
        else if(num >= 0 && splittextdata.Length == touch_count)
        {
            Application.Quit();
        }
        /* === 課題実験（ここまで） === */
    }

    public void onCallBack(string msg)
    {
        Debug.Log("Call From Native. (" + msg + ")");
    }
    void ReadTextData()
    {
//        System.IO.StreamReader reader = new System.IO.StreamReader(Application.dataPath + file_path, System.Text.Encoding.GetEncoding("UTF-8"));
//        System.IO.StreamReader reader = new System.IO.StreamReader(Application.dataPath + file_path, System.Text.Encoding.GetEncoding("Shift_JIS"));
        //System.IO.StreamReader reader = new System.IO.StreamReader("jar:file://"+Application.dataPath + "!/assets/"+file_path, System.Text.Encoding.GetEncoding("Shift_JIS"));
         //       StringReader reader = new StringReader(txtxt.text);
        textdata = txtxt.text;//reader.ReadToEnd();
        //reader.Close();
//        textdata = System.Text.RegularExpressions.Regex.Replace(stageData,@" ","");
    }
    void CheckClick()
    {
        if (Input.GetMouseButtonUp(0) && Input.mousePosition.y < Screen.height / 5)
        {
            if (Input.mousePosition.x > Screen.width *3 / 4)
            {
                if (touch_count > 0) touch_count--;
                if ((num == 131 && touch_count == 3) ||
                    (num == 211 && touch_count == 2) ||
                    (num == 221 && touch_count == 4) ||
                    (num == 311 && touch_count == 1) ||
                    (num == 321 && touch_count == 4))
                {
                    texture_num--;
                }
            }
            else
            {
                if (touch_count < 3000) touch_count++;
                if ((num == 131 && touch_count == 4)||
                    (num == 211 && touch_count == 3)||
                    (num == 221 && touch_count == 5)||
                    (num == 311 && touch_count == 2)||
                    (num == 321 && touch_count == 5))
                {
                    texture_num++;
                }
            }
        }

    }
}


