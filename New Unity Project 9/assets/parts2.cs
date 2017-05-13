using UnityEngine;
using System.Collections;

public class parts2 : MonoBehaviour
{
    public float wire_input_voltage = 0,wire_output_voltage=0,wire_amperage=0;
    public float input_voltage = 0, output_voltage = 0, voltage = 0;
    public float amperage = 0,resistance;
    public float input_amperage = 0, output_amperage = 0;

    int i = 0,j=0;
    output_point2[] output_pointval;
    input_point2 input_pointval;

    void Start()
    {
        output_pointval = new output_point2[3];
    }
    void Update()
    {
        output_pointval = GetComponentsInChildren<output_point2>();
        input_pointval = GetComponentInChildren<input_point2>();
                
        for (i = 0; output_pointval.Length > i; i++)
        {
            if (output_pointval[i].wire_attached_flag && input_pointval.wire_attached_flag)
            {
                wire_input_voltage = input_pointval.wire_voltage;
                wire_output_voltage = output_pointval[0].wire_voltage;
                wire_amperage = (input_pointval.wire_amperage + output_amperage) / 2.0f;

                amperage = amperage * 0.2f + ((output_voltage - input_voltage) / resistance) * 0.8f;
                voltage = amperage * resistance;
                input_voltage = input_voltage / 2.0f + wire_input_voltage / 2.0f;
                output_voltage = input_voltage + voltage;
                break;

            }
            else if (output_pointval[i].wire_attached_flag)
            {
                output_voltage = output_pointval[i].wire_voltage;
                input_voltage = output_voltage;
                amperage = 0.0f;
                break;
            }
            else if (input_pointval.wire_attached_flag && i==2)
            {
                input_voltage = input_pointval.wire_voltage;
                output_voltage = input_voltage;
                amperage = 0.0f;
            }
            else
            {
                input_voltage = 0;
                output_voltage = 0;
                amperage = 0.0f;
            }

        }
    }
}