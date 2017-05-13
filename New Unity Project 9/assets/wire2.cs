using UnityEngine;
using System.Collections;

public class wire2 : MonoBehaviour
{
    public float voltage = 0;
    float[] input_voltage_buf;
    float[] output_voltage_buf;
    public float voltage_sum = 0;
    public float amperage = 0;
    public float amperage_def;
    float input_amparege;
    float output_amperage;
    input_point input_connect;
    output_point output_connect;
    int i = 0, j = 0;
    int amp_i = 0, amp_j = 0;
    Camera GameCamera;
    bool in_flag, out_flag;
    float in_voltage, out_voltage;
    float voltage_sum_buf = 0;
    int l = 0,k=0;
    public bool touch_flag = false;
    public bool eliminate = false;
    // Use this for initialization
    void Start()
    {
        GameCamera = GameObject.FindObjectOfType<Camera>();
        GetComponent<Collider>().isTrigger = true;
        input_voltage_buf = new float[30];
        output_voltage_buf = new float[30];
    }

    // Update is called once per frame
    void Update()
    {
        amperage_def = (input_amparege - output_amperage) / (float)(amp_i + amp_j);

        for (int k = 0; k < i; k++)
        {
            voltage_sum += input_voltage_buf[k];
        }
        for (int k = 0; k < j; k++)
        {
            voltage_sum += output_voltage_buf[k];
        }
        if (in_flag == true) voltage = in_voltage;
        else if (out_flag == true) voltage = out_voltage;
        else if (i != 0 || j != 0)
        {

        }
        if (i != 0 || j != 0)
        {
            amperage = (output_amperage + input_amparege) / (float)(i + j);
        }
        amp_i = 0;
        amp_j = 0;
        i = 0; j = 0;
        voltage_sum_buf = voltage_sum;
        voltage_sum = 0;
        input_amparege = 0;
        output_amperage = 0;
        in_flag = false;
        out_flag = false;
    }
    //void OnGUI()
    //{
    //    GUI.TextField(new Rect(GameCamera.WorldToScreenPoint(this.transform.position).x, GameCamera.WorldToScreenPoint(this.transform.position).y + 40, 140, 60), "wire voltage:" + voltage + "\nvoltage sum:" + voltage_sum_buf + "\namperge:" + amperage);
    //}

    private void OnTriggerStay(Collider other_object)
    {

        if (other_object.GetComponent<input_point>())
        {
            input_connect = other_object.GetComponent<input_point>();
            input_voltage_buf[i] = input_connect.parts_voltage;
            if (input_connect.amperage >= 0)
            {
                input_amparege += input_connect.amperage;
                amp_j++;
            }
            else
            {
                output_amperage -= input_connect.amperage;
                amp_i++;
            }
            i++;
            if (input_connect.flag == false)
            {
                in_flag = true;
                in_voltage = input_connect.voltage;
            }
        }
        if (other_object.GetComponent<output_point>())
        {
            output_connect = other_object.GetComponent<output_point>();
            output_voltage_buf[j] = output_connect.parts_voltage;
            if (output_connect.amperage > 0)
            {
                output_amperage += output_connect.amperage;
                amp_i++;
            }
            else
            {
                input_amparege -= output_connect.amperage;
                amp_j++;
            }
            j++;
            if (output_connect.flag == false)
            {
                out_flag = true;
                out_voltage = output_connect.voltage;
            }
        }

        //if (other_object.GetComponent<wire>())
        //{
        //    input_voltage_buf[i] = other_object.GetComponent<wire>().voltage;
        //    if (input_connect.amperage >= 0)
        //    {
        //        amperage_def = other_object.GetComponent<wire>().amperage_def;
        //    }
        //    i++;
        //}
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
                if (hit.collider.ToString() == this.ToString() && !touch_flag)
                {
                    touch_flag = true;
                }
                else if (hit.collider.GetInstanceID() == this.GetInstanceID() && touch_flag)
                {
                    Destroy(hit.collider.gameObject);
                }

                else if (touch_flag)
                {
                    touch_flag = false;
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

        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.GetInstanceID() + " " + this.gameObject.GetInstanceID());
            if (hit.collider.gameObject.GetInstanceID() == this.gameObject.GetInstanceID() && !touch_flag)
            {
                touch_flag = true;
            }
            else if (hit.collider.gameObject.GetInstanceID() == this.gameObject.GetInstanceID() && touch_flag)
            {
                hit.collider.gameObject.transform.localPosition = new Vector3(10000, 0, 0);
                hit.collider.gameObject.transform.localScale = new Vector3(0, 0, 0);
            }

            else if (touch_flag)
            {
                touch_flag = false;
            }
            //物体に光線が衝突した時の挙動
        }
    }

}
