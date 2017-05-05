using UnityEngine;
using System.Collections;
using System;

public class parts : MonoBehaviour
{
    Camera GameCamera;
    public float input_voltage, output_voltage;
    public float voltage, amperage;
    public float resistance;
    output_point[] output_pointval;
    input_point input_pointval;
    //Camera GameCamera;
    float[] amperage_average;
    float amperage_sum = 0;
    float[] voltage_average;
    float voltage_sum = 0;
    GameObject[] lightbox;
    public GameObject spotlight;
    bool switch_on_flag = false;
    int i;
    float time = 0, dt;
    public GUIStyle st;
    public GUIStyleState stst;
    float wire_before = 0;
    // Use this for initialization
    void Start()
    {
        GameCamera = GameObject.FindObjectOfType<Camera>();
        st = new GUIStyle();

        stst = new GUIStyleState();
        st.fontSize = 30;
        stst.textColor = Color.red;
        output_pointval = new output_point[3];
        if (this.gameObject.tag == "ammeter" || this.gameObject.tag == "lightbulb")
        {
            amperage_sum = 0;
            amperage_average = new float[10];
            for (int j = 0; j < 10; j++)
            {
                amperage_average[j] = 0;
            }
        }
        if (this.gameObject.tag == "vmeter")
        {
            voltage_sum = 0;
            voltage_average = new float[10];
            for (int j = 0; j < 10; j++)
            {
                voltage_average[j] = 0;
            }
        }
            input_voltage = 0;
        //GameCamera = GameObject.FindObjectOfType<Camera>();
        amperage = 0.0f;//最初に超微量の電流を流す
        voltage = resistance * amperage;

        if (this.gameObject.tag == "switch")
        {
            resistance = 100000000f;
        }
        else if (this.gameObject.tag == "lightbulb")
        {
            resistance = 5;
            lightbox = new GameObject[9];
            GetComponentsInChildren<Light>()[0].intensity = 0.0f;
            GetComponentsInChildren<Light>()[1].intensity = 0.0f;
            GetComponentsInChildren<Light>()[2].intensity = 0.0f;
            GetComponentsInChildren<Light>()[3].intensity = 0.0f;
            GetComponentsInChildren<Light>()[4].intensity = 0.0f;
            GetComponentsInChildren<Light>()[0].range = 300.0f;
            GetComponentsInChildren<Light>()[1].range = 300.0f;
            GetComponentsInChildren<Light>()[2].range = 300.0f;
            GetComponentsInChildren<Light>()[3].range = 300.0f;
            GetComponentsInChildren<Light>()[4].range = 300.0f;
                        
            for (i = 0; i < 9; i++)
            {
                lightbox[i] = (GameObject)GameObject.Instantiate(spotlight);
                lightbox[i].light.intensity = 0;
                if (i == 5)
                    lightbox[i].transform.position = this.transform.position + new Vector3((i / 3 - 1) , 6, (i % 3 - 1) );
                else
                    lightbox[i].transform.position = this.transform.position + new Vector3((i / 3 - 1) , 4, (i % 3 - 1) );
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
   
        output_pointval = GetComponentsInChildren<output_point>();
        input_pointval = GetComponentInChildren<input_point>();
  //      print("pointnum"+output_pointval.Length);
        for (i = 0; output_pointval.Length>i;i++ )
        {
                if (output_pointval[i].wire_attached_flag && input_pointval.wire_attached_flag)
                {
                    //                    print("deta!");
                    amperage = (amperage) / 4.0f + (input_pointval.voltage - output_pointval[i].voltage) * 3.0f / (resistance * 4.0f);
                    if (Math.Abs(amperage) > Math.Abs(input_pointval.wire_amperage)  && input_pointval.wire_amperage !=0.0f && wire_before != 0)
                    {
                        amperage = Math.Sign(amperage) * input_pointval.wire_amperage ;
                    }

                    //   amperage = (input_voltage - output_voltage)/ (resistance);
//                    print("in:" + input_pointval.voltage + "\nout:" + output_pointval[i].voltage);
                    voltage = amperage * resistance;
//                    input_voltage = (input_voltage + input_pointval.voltage) / 2.0f;
                    input_voltage = (output_pointval[i].wire_voltage + input_pointval.wire_voltage + voltage) / 2.0f;
                    output_voltage = input_voltage - voltage;
 
                    break;

                }
                else if (output_pointval[i].wire_attached_flag)
                {
 //                   print("in:2" + input_pointval.voltage + "\nout:" + output_pointval[i].voltage);

                    output_voltage = output_pointval[i].voltage;
                    input_voltage = output_voltage;
                    voltage = 0;
                    amperage = 0.0f;
                    break;
                }
                else if (input_pointval.wire_attached_flag && i==output_pointval.Length-1)
                {
 //                   print("in:3" + input_pointval.voltage + "\nout:" + output_pointval[i].voltage);

                    input_voltage = input_pointval.voltage;
                    output_voltage = input_voltage;
                    voltage = 0;
                    amperage = 0.0f;
                }
                else if (i == output_pointval.Length-1)
                {
//                    print("in:4" + input_pointval.voltage + "\nout:" + output_pointval[i].voltage);
                    
                    voltage = 0;
                    input_voltage = 0;
                    output_voltage = 0;
                    amperage = 0.0f;
                }

        }
        wire_before = input_pointval.wire_amperage;
        dt = Time.deltaTime;
        time += dt;
            if (this.gameObject.tag == "switch")
            {
                //CheckTouch();
                CheckClick();
                /* if (time > 6.0f)
                 {
                     time = 0;
                     switch_on_flag = true;
                 }
                 */
                if (switch_on_flag)
                {
                    switch_on_flag = false;
                    if (resistance > 9999.0f)
                    {
                        resistance = 0.001f;
                        gameObject.transform.FindChild("switch_off").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_off/Circle_001").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_off/Sphere").gameObject.renderer.enabled = false;

                        gameObject.transform.FindChild("switch_on").gameObject.renderer.enabled = true;
                        gameObject.transform.FindChild("switch_on/Circle_001").gameObject.renderer.enabled = true;
                        gameObject.transform.FindChild("switch_on/Sphere").gameObject.renderer.enabled = true;
                    }
                    else
                    {
                        resistance = 10000000.0f;
                        if(amperage != 0){
                            amperage = Math.Sign(amperage)*0.000001f;
                        }
                        gameObject.transform.FindChild("switch_on").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_on/Circle_001").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_on/Sphere").gameObject.renderer.enabled = false;

                        gameObject.transform.FindChild("switch_off").gameObject.renderer.enabled = true;
                        gameObject.transform.FindChild("switch_off/Circle_001").gameObject.renderer.enabled = true;
                        gameObject.transform.FindChild("switch_off/Sphere").gameObject.renderer.enabled = true;
                    }

                }
                else
                {
                    if (resistance > 9999.0f)
                    {
                        resistance = 10000000.0f;
                        gameObject.transform.FindChild("switch_on").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_on/Circle_001").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_on/Sphere").gameObject.renderer.enabled = false;
                    }
                    else
                    {
                        resistance = 0.001f;
                        gameObject.transform.FindChild("switch_off").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_off/Circle_001").gameObject.renderer.enabled = false;
                        gameObject.transform.FindChild("switch_off/Sphere").gameObject.renderer.enabled = false;
                    }
                }
                
            }
            else if (this.gameObject.tag == "lightbulb")
            {
                if (time > 0.5f)
                {
                    time = 0.0f;
                    amperage_sum = 0;
                    for (i = 0; i < 9; i++)
                    {
                        amperage_sum += amperage_average[i + 1];
                        amperage_average[i + 1] = amperage_average[i];
                    }

                    amperage_average[0] = amperage;
                    amperage_sum += amperage_average[0];

                    if (Math.Abs(amperage - amperage_average[9]) < 0.01f && Math.Abs(amperage - (amperage_sum / 10.0f)) < 0.01f
                    && Math.Sign(amperage) == Math.Sign(amperage_average[9]) && Math.Sign(amperage) == Math.Sign(amperage_sum / 10.0f))
                    {

                        GetComponentsInChildren<Light>()[0].intensity = Math.Abs(amperage * 2.0f);
                        GetComponentsInChildren<Light>()[1].intensity = Math.Abs(amperage * 2.0f);
                        GetComponentsInChildren<Light>()[2].intensity = Math.Abs(amperage * 2.0f);
                        GetComponentsInChildren<Light>()[3].intensity = Math.Abs(amperage * 2.0f);
                        GetComponentsInChildren<Light>()[4].intensity = Math.Abs(amperage * 2.0f);
                        
                        for (i = 0; i < 9; i++)
                        {
                            //lightbox[i].light.intensity = amperage * amperage * 2.0f;
                            //if (i == 4)
                            //    lightbox[i].transform.position = this.gameObject.transform.FindChild("BezierCurve").transform.position + new Vector3((i / 3 - 1) * 15f, 100f, (i % 3 - 1) * 15f);
                            //else
                            //    lightbox[i].transform.position = this.gameObject.transform.FindChild("BezierCurve").transform.position + new Vector3((i / 3 - 1) * 15f, 50f, (i % 3 - 1) * 15f);
                            //lightbox[i].light.intensity = Math.Abs(amperage*2);
                            
                                if (i == 4)
                                    lightbox[i].transform.position = this.gameObject.transform.FindChild("BezierCurve").transform.position + new Vector3(((i / 3) - 1) * 15.0f, 100.0f, ((i % 3) - 1) * 15.0f);
                                else
                                    lightbox[i].transform.position = this.gameObject.transform.FindChild("BezierCurve").transform.position + new Vector3(((i / 3) - 1) * 15.0f, 50.0f, ((i % 3) - 1) * 15.0f);
                        }
                    }
                    else
                    {
                        for (i = 0; i < 9;i++)
                        {
                            lightbox[i].light.intensity = 0.0f;
                        }
                    }
                }
            }
        }
    /*
    void OnGUI()
    {
        if (this.tag == "ammeter")
        {
            if (time > 0.5f)
            {
                time = 0;

                amperage_sum = 0;
                for (i = 0; i < 9; i++)
                {
                    amperage_sum += amperage_average[i + 1];
                    amperage_average[i + 1] = amperage_average[i];
                }

                amperage_average[0] = amperage;
                amperage_sum += amperage_average[0];
            }
//                print("amperage" + amperage_sum / 100.0f);
            if (Math.Abs(amperage - amperage_average[9]) < 0.01f && Math.Abs(amperage - (amperage_sum / 10.0f)) < 0.01f
              && Math.Sign(amperage) == Math.Sign(amperage_average[9]) && Math.Sign(amperage) == Math.Sign(amperage_sum / 10.0f))
            {

                if (this.gameObject.transform.FindChild("minus1").GetComponent<output_point>().wire_attached_flag)
                {
                    if (amperage_sum / 10f > 99.9f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + 99.9f + " A", st);
                    }
                    else if (amperage_sum > 0f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + (float)((int)(amperage_sum)) / 10.0f + " A", st);
                    }
                    else if (amperage_sum < 0f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + 0 + "    A", st);
                    }

                }
                else if (this.gameObject.transform.FindChild("minus2").GetComponent<output_point>().wire_attached_flag)
                {
                    if (amperage_sum / 10f > 9.99f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + 9.99f + " A", st);
                    }
                    else if (amperage_sum > 0.0f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + (float)((int)(amperage_sum * 100.0f)) / 1000.0f + " A", st);
                    }
                    else if (amperage_sum < 0f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + 0 + "    A", st);
                    }
                }
                else if (this.gameObject.transform.FindChild("minus3").GetComponent<output_point>().wire_attached_flag)
                {
                    if (amperage_sum / 10f > 0.999f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + 0.999f + " A", st);
                    }
                    else if (amperage_sum > 0f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + (float)((int)(amperage_sum * 1000.0f)) / 10000.0f + " A", st);
                    }
                    else if (amperage_sum < 0f)
                    {
                        GUI.TextField(new Rect(0, 0, 100, 40), "" + 0 + "    A", st);
                    }
                }
            }
            else
            {
                GUI.TextField(new Rect(0, 0, 100, 40), "" + 0 + "    A", st);
            }
            
   //        GUI.TextField(new Rect(0, 0, 100, 40),  " " + amperage_sum/20.0f);
        }
        else if (this.tag == "vmeter")
        {
            if (time > 0.5f)
            {
                time = 0;

                voltage_sum = 0;
                for (i = 0; i < 9; i++)
                {
                    voltage_sum += voltage_average[i + 1];
                    voltage_average[i + 1] = voltage_average[i];
                }
                voltage_average[0] = voltage;
                voltage_sum += voltage_average[0];
            }
           if (this.gameObject.transform.FindChild("minus1").GetComponent<output_point>().wire_attached_flag)
            {
                if (voltage_sum / 10f > 99.9f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + 99.9f + " V", st);
                }
                else if (voltage_sum > 0f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + (float)((int)(voltage_sum)) / 10.0f + " V", st);
                }
                else if (voltage_sum < 0f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + 0 + "    V", st);
                }

            }
            else if (this.gameObject.transform.FindChild("minus2").GetComponent<output_point>().wire_attached_flag)
            {
                if (voltage_sum / 10f > 9.99f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + 9.99f + " V", st);
                }
                else if (voltage_sum > 0.0f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + (float)((int)(voltage_sum * 1.0f)) / 1000.0f + " V", st);
                }
                else if (voltage_sum < 0f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + 0 + "    V", st);
                }
            }
            else if (this.gameObject.transform.FindChild("minus3").GetComponent<output_point>().wire_attached_flag)
            {
                if (voltage_sum / 10f > 0.999f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + 0.999f + " V", st);
                }
                else if (voltage_sum > 0f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + (float)((int)(voltage_sum * 10.0f)) / 10000.0f + " V", st);
                }
                else if (voltage_sum < 0f)
                {
                    GUI.TextField(new Rect(100, 0, 100, 40), "" + 0 + "    V", st);
                }
            }

            //        GUI.TextField(new Rect(0, 0, 100, 40),  " " + voltage_sum/20.0f);
        }
        //else if (this.tag == "switch")
        //{
        //   GUI.TextField(new Rect(100, 0, 100, 80), " " + voltage + "\n" + output_voltage + "out\n" + input_voltage + "in\n" + amperage, st);
        //}
        //else
        //{
        //    GUI.TextField(new Rect(GameCamera.WorldToScreenPoint(this.transform.position).x, GameCamera.WorldToScreenPoint(this.transform.position).y + 40, 140, 60), "voltage:" + voltage + "\namperge:" + amperage + "\nin:" + input_voltage + "\nout:" + output_voltage);
        //}

    }
    */
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
                if (hit.collider.gameObject.name == this.name)
                {
                    switch_on_flag = true;
                }
                //物体に光線が衝突した時の挙動
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
                if (hit.collider.gameObject.name == this.name)
                {
                    switch_on_flag = true;
                }
                //物体に光線が衝突した時の挙動
        }
    }

}