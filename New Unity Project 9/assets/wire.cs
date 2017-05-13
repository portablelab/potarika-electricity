using UnityEngine;
using System.Collections;
using System;
public class wire : MonoBehaviour{
    public float voltage=0;         
    public float[] input_voltage_buf;
    public float[] output_voltage_buf;     
    public float voltage_sum = 0;    
    public float amperage=0;        
    public float amperage_def = 0,amperage_def_before=0;
    public float input_amparege;
    public float output_amperage;
    input_point input_connect;
    output_point output_connect;
    public int i = 0, j = 0;
    public int k = 0, l = 0;
    public int amp_i = 0, amp_j = 0;
    Camera GameCamera;
    public bool in_flag, out_flag;
    public float in_voltage, out_voltage;
    public float voltage_sum_buf = 0;
    public bool disuse_flag = false;
    public bool solved = false;
    public bool touch_flag = false;
    public bool eliminate = false;
    public float weight = 0.00000008f;
    public string[] parent_name,full_name;
    // Use this for initialization
    void Start () {
        GameCamera = GameObject.FindObjectOfType<Camera>();
        GetComponent<Collider>().isTrigger = true;
        input_voltage_buf = new float[100];
        output_voltage_buf = new float[100];
        input_amparege = 0;
        output_amperage = 0;
        parent_name = new string[2];
        full_name = new string[2];
    }

    // Update is called once per frame
    void Update () {
            //        print(""+this.ToString()+","+ this.gameObject.ToString()+","+ name);
            //CheckTouch();
            //CheckClick();
            //l++;
            //if (l > 1)
            //{
            //    l = 0;
        if (!eliminate)
        {
            if (amp_i != 0 || amp_j != 0)
            {
                //   print(output_amperage+"out\n " + input_amparege + " in\n" + amp_i + "\n " + j);
                amperage_def_before = amperage_def;
                amperage_def = (input_amparege - output_amperage) / (float)(amp_i + amp_j);
            }
            if (Math.Abs(amperage_def) > 0.001f || Math.Abs(amperage_def_before) == 0.0f)
            {
                if (amperage_def / amperage_def_before < 0 && Math.Abs(amperage_def) > Math.Abs(amperage_def_before))
                {
                    weight = weight * 0.05f;
                }
                else if (amperage_def / amperage_def_before < 0 && Math.Abs(amperage_def) > Math.Abs(amperage_def_before) / 2.0f)
                {
                    weight = weight * 0.1f;
                }
                else if (amperage_def / amperage_def_before > 0 && Math.Abs(amperage_def) > Math.Abs(amperage_def_before) * 3.0f / 4.0f && Math.Abs(amperage_def) > 0.0000001)
                {
                    weight = weight * 1.2f;
                }
                else if (amperage_def / amperage_def_before > 0 && Math.Abs(amperage_def) > Math.Abs(amperage_def_before) / 2.0f && Math.Abs(amperage_def) > 0.0000001)
                {
                    weight = weight * 1.05f;
                }
                //else
                //{
                //    weight = weight * 1.00f;
                //}

                if (weight < 0.0000001f)
                {
                    weight = 0.0000001f;
                }
                else if (weight > 0.8f)
                {
                    weight = 0.8f;
                }
                for (k = 0; k < i; k++)
                {
                    voltage_sum += input_voltage_buf[k];
                }
                for (k = 0; k < j; k++)
                {
                    voltage_sum += output_voltage_buf[k];
                }
                if (in_flag == true)
                {
                    voltage = in_voltage;
                }
                else if (out_flag == true)
                {
                    voltage = out_voltage;
                    //                print("gets!");
                }
                else if (i != 0 || j != 0)
                {
                    //    voltage = ((voltage * 3.0f) / 4.0f + (voltage_sum / (float)(i + j)) / 4.0f) - amperage_def * weight;
                    //print("voltage_sum" + voltage_sum + "\namperage_def" + amperage_def + "\nweight" + weight + "\nvoltage" + voltage);
                    if (Math.Abs(amperage_def * weight) < 1.0f)
                    {
                        voltage = ((voltage) / 4.0f + (voltage_sum / (float)(i + j)) * 3.0f / 4.0f) - amperage_def * weight;
                    }
                    else
                    {
                        voltage = ((voltage) / 4.0f + (voltage_sum / (float)(i + j)) * 3.0f / 4.0f) - Math.Abs(amperage_def) / (amperage_def );
                    }
                    if (Math.Abs(voltage) > 6.0f) { voltage = (Math.Abs(voltage) / voltage) * 6.0f; }
                }
            }
            if (amp_i != 0 || amp_j != 0)
            {
                amperage = (output_amperage + input_amparege) / 2;// (float)(amp_i + amp_j);
            }
            //else amperage = 0;

            //      print(output_amperage+"\n " + input_amparege + " \n" + amp_i + "\n " + j);
            amp_i = 0;
            amp_j = 0;
            i = 0;
            j = 0;
            voltage_sum_buf = voltage_sum;
            voltage_sum = 0;
            input_amparege = 0.0f;
            output_amperage = 0.0f;
            in_flag = false;
            out_flag = false;
            solved = false;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
      //  }
    }
    //void OnGUI()
    //{
    //    GUI.TextField(new Rect(GameCamera.WorldToScreenPoint(this.transform.position).x, GameCamera.WorldToScreenPoint(this.transform.position).y + 40, 140, 60), "weight:" + weight + "\namperage_def:" + amperage_def + "\nvoltage:" + voltage);
    //}
    
    private void OnTriggerStay(Collider other_object)
    {
        if (!eliminate)
        {
            if (other_object.GetComponent<input_point>())
            {
                input_connect = other_object.GetComponent<input_point>();
                input_voltage_buf[i] = input_connect.parts_voltage;
                //          print("input:"+input_connect.amperage+"name"+input_connect.gameObject.transform.parent.name);
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
                if (!input_connect.flag)
                {
                    //                print("get");
                    in_flag = true;
                    in_voltage = input_connect.voltage;
                }
            }
            if (other_object.GetComponent<output_point>())
            {
                output_connect = other_object.GetComponent<output_point>();
                output_voltage_buf[j] = output_connect.parts_voltage;
                //        print("output:" + output_connect.amperage + "name" + output_connect.gameObject.name);
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
                if (!output_connect.flag)
                {
                    out_flag = true;
                    out_voltage = output_connect.voltage;
                }
            }

            if (other_object.GetComponent<wire>())
            {
                if (!other_object.GetComponent<wire>().solved)
                {
                    for (k = 0; k < other_object.GetComponent<wire>().i; k++)
                    {
                        input_voltage_buf[i + k] = other_object.GetComponent<wire>().input_voltage_buf[k];
                    }
                    i = i + k;
                    for (k = 0; k < other_object.GetComponent<wire>().j; k++)
                    {
                        output_voltage_buf[i + k] = other_object.GetComponent<wire>().output_voltage_buf[k];
                    }
                    j = j + k;
                    output_amperage += other_object.GetComponent<wire>().output_amperage;
                    amp_i += other_object.GetComponent<wire>().amp_i;
                    input_amparege += other_object.GetComponent<wire>().input_amparege;
                    amp_j += other_object.GetComponent<wire>().amp_j;
                    if (other_object.GetComponent<wire>().in_flag)
                    {
                        in_flag = true;
                        in_voltage = other_object.GetComponent<wire>().in_voltage;
                    }
                    if (other_object.GetComponent<wire>().out_flag)
                    {
                        out_flag = true;
                        out_voltage = other_object.GetComponent<wire>().out_voltage;
                    }
                }
                else
                {
                    //print("already solved");
                    for (k = 0; k < other_object.GetComponent<wire>().i; k++)
                    {
                        input_voltage_buf[k] = other_object.GetComponent<wire>().input_voltage_buf[k];
                    }
                    i = k;
                    for (k = 0; k < other_object.GetComponent<wire>().j; k++)
                    {
                        output_voltage_buf[k] = other_object.GetComponent<wire>().output_voltage_buf[k];
                    }
                    j = k;
                    output_amperage = other_object.GetComponent<wire>().output_amperage;
                    amp_i = other_object.GetComponent<wire>().amp_i;
                    input_amparege = other_object.GetComponent<wire>().input_amparege;
                    amp_j = other_object.GetComponent<wire>().amp_j;
                    if (other_object.GetComponent<wire>().in_flag)
                    {
                        in_flag = true;
                        in_voltage = other_object.GetComponent<wire>().in_voltage;
                    }
                    if (other_object.GetComponent<wire>().out_flag)
                    {
                        out_flag = true;
                        out_voltage = other_object.GetComponent<wire>().out_voltage;
                    }

                }
                solved = true;
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
//        Touch touch = Input.GetTouch(0);

            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
     //           print(hit.collider.gameObject.GetInstanceID() +" "+ this.gameObject.GetInstanceID());
                if (hit.collider.gameObject.GetInstanceID()==this.gameObject.GetInstanceID()&&!touch_flag)
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
