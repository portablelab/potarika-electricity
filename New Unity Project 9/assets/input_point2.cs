using UnityEngine;
using System.Collections;

public class input_point2 : MonoBehaviour
{

    public float wire_voltage = 0, wire_amperage;
    public float parts_input_voltage = 0, parts_output_voltage = 0;
    public float parts_voltage, parts_amperage, parts_resistance;

    wire_input_point connect_in;
    wire_output_point connect_out;
    parts2 mainparts;
    battery mainbattery;
    public bool parts_flag = false;
    public bool wire_attached_flag = false;
    int wire_attached_count = 0;

    // Use this for initialization
    void Start()
    {
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainparts = GetComponentInParent<parts2>())
        {
            parts_input_voltage = mainparts.input_voltage;
            parts_output_voltage = mainparts.output_voltage;
            parts_resistance = mainparts.resistance;
            parts_amperage = mainparts.amperage;
            parts_voltage = mainparts.voltage;
            parts_flag = true;
        }
        if (mainbattery = GetComponentInParent<battery>())
        {
            parts_input_voltage = mainbattery.input_voltage;
            parts_output_voltage = mainparts.output_voltage;
            parts_amperage = mainbattery.amperage;
            parts_voltage = mainbattery.input_voltage;
        }
        if (wire_attached_count < 12)
        {
            wire_attached_count++;
        }
        else if (wire_attached_flag)
        {
            wire_attached_flag = false;
        }

    }
    private void OnTriggerStay(Collider other_object)
    {
        if (other_object.GetComponent<wire_input_point>())
        {
            connect_in = other_object.GetComponent<wire_input_point>();
            wire_voltage = connect_in.wire_voltage;
            wire_amperage = connect_in.wire_amperage;

            wire_attached_flag = true;
            wire_attached_count = 0;
        } 
        if (other_object.GetComponent<wire_output_point>())
        {
            connect_out = other_object.GetComponent<wire_output_point>();
            wire_voltage = connect_in.wire_voltage;
            wire_amperage = connect_in.wire_amperage;

            wire_attached_flag = true;
            wire_attached_count = 0;
        }
    }
}
