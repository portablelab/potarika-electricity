using UnityEngine;
using System.Collections;
using System;

public class battery : MonoBehaviour
{
    public float input_voltage, output_voltage;
    public const float voltage = 1.5f;
    public float amperage;
    public float resistance=0.01f;
    float amperage_def=0;
    output_point output_pointval;
    input_point input_pointval;
    Camera GameCamera;
    public bool battery_exist_flag;
    // Use this for initialization
    void Start()
    {
        GameCamera = GameObject.FindObjectOfType<Camera>();
        amperage = 0.0f;//最初に超微量の電流を流す
    }   
    // Update is called once per frame
    void Update()
    {
        output_pointval = GetComponentInChildren<output_point>();
        input_pointval = GetComponentInChildren<input_point>();
        amperage_def = output_pointval.amperage_def;
 //       print(" amperage_def" + amperage_def);
        if (battery_exist_flag == false)
        {
            if (output_pointval.wire_attached_flag && input_pointval.wire_attached_flag)
            {
                if (Mathf.Abs(amperage_def) < 0.04f)
                {
                    amperage = amperage + amperage_def / 2.0f;
                }
                else
                {
                    amperage = amperage + 0.02f * Math.Sign(amperage_def);
                }
            }
            else if (output_pointval.wire_attached_flag)
            {
                amperage = 0;
            }
            else if (input_pointval.wire_attached_flag)
            {
                amperage = 0;
            }
            input_voltage = 0;
            output_voltage = input_voltage + voltage;
        }
        else
        {
            if (output_pointval.wire_attached_flag && input_pointval.wire_attached_flag)
            {
                if (Math.Abs(amperage_def) < 0.009f)
                {
                    amperage = amperage + amperage_def / 2.0f;
                }
                else
                {
                    amperage = amperage + 0.0045f * Math.Sign(amperage_def);
                }
                input_voltage = (output_pointval.wire_voltage + input_pointval.wire_voltage - voltage) / 2.0f;
                output_voltage = input_voltage + voltage;
                //output_voltage = output_pointval.wire_voltage;
                //input_voltage = output_voltage - voltage;

            }
            else if (output_pointval.wire_attached_flag)
            {
                output_voltage = output_pointval.wire_voltage;
                input_voltage = output_voltage-voltage;
                amperage = 0;
            }
            else if (input_pointval.wire_attached_flag)
            {
                input_voltage = input_pointval.wire_voltage;
                output_voltage = input_voltage+voltage;
                amperage = 0;
            }
        }
    }
    
    //void OnGUI()
    //{
    //    GUI.TextField(new Rect(GameCamera.WorldToScreenPoint(this.transform.position).x, GameCamera.WorldToScreenPoint(this.transform.position).y, 150, 40), "battery voltage:" + input_voltage + "\namperge:" + amperage);
    //}
}