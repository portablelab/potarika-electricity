using UnityEngine;
using System.Collections;

public class output_point : MonoBehaviour
{
    public float wire_voltage, parts_voltage;
    public float wire_amperage, parts_amperage;
    public float voltage, amperage,amperage_def;
    //Camera GameCamera;
    wire connect;
    parts mainparts;
    battery mainbattery;
    public bool flag = false;
    public bool wire_attached_flag = false;
    int wire_attached_count = 0;
    // Use this for initializ\ation
    void Start()
    {
//        GameCamera = GameObject.FindObjectOfType<Camera>();
        GetComponent<Collider>().isTrigger = true;
        amperage = 0.00f;
        amperage_def = 0;
    }

    //Update is called once per frame
    void Update()
    {

        if (wire_attached_count < 12)
        {
            wire_attached_count++;
        }
        else if (wire_attached_flag)
        {
                wire_attached_flag = false;
        }

        if (mainparts = GetComponentInParent<parts>())
        {
            parts_voltage = mainparts.output_voltage;
            amperage = mainparts.amperage;
            flag = true;
        }
        if (mainbattery = GetComponentInParent<battery>())
        {
            parts_voltage = mainbattery.output_voltage;
            amperage = mainbattery.amperage;

            if (mainbattery.battery_exist_flag)
            {
                flag = true;
            }
            else
            {
                voltage = mainbattery.output_voltage;
            }
        }
    }
    private void OnTriggerStay(Collider other_object)
    {
        if (other_object.GetComponent<wire>())
        {
            connect = other_object.GetComponent<wire>();
            wire_voltage = connect.voltage;
            wire_amperage = connect.amperage;
            amperage_def = connect.amperage_def;
            if (flag)
            {
                voltage = wire_voltage;
            }
            wire_attached_flag = true;
 //           print("out!");
            wire_attached_count = 0;
        }
    }

/*    void OnGUI()
    {
        GUI.TextField(new Rect(GameCamera.WorldToScreenPoint(this.transform.position).x, GameCamera.WorldToScreenPoint(this.transform.position).y + 30, 110, 40), "outputvoltage:" + voltage + "\namperge:" + amperage);
    }*/
}