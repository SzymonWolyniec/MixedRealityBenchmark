using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class ConnectDataBase : MonoBehaviour
{

    public List<float> FPSListFromDB = new List<float>();
    public List<float> CPUListFromDB = new List<float>();
    public void downloadCPU()
    {
        StartCoroutine(downloadCPUCor());
    }

    private IEnumerator downloadCPUCor()
    {
        WWWForm form = new WWWForm();
        form.AddField("downloadCPUData", "true");

        string url = "http://pique993.prv.pl/index2.php";

        WWW www = new WWW(url, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.text;
            string[] jsonSplit = json.Split('<');

            ResponseCls response = JsonUtility.FromJson<ResponseCls>(jsonSplit[0]);

            CPUFromJsonClass CPUClassObj = JsonConvert.DeserializeObject<CPUFromJsonClass>(jsonSplit[0]);

            foreach (var x in CPUClassObj.content)
            {
                var convertAvgCPU = x.avgCPU.Replace('.', ',');
                CPUListFromDB.Add(float.Parse(convertAvgCPU));
            }

        }
    }


    public void downloadFPS()
    {
        StartCoroutine(downloadFPSCor());
    }

    private IEnumerator downloadFPSCor()
    {
        WWWForm form = new WWWForm();
        form.AddField("downloadFPSData", "true");

        string url = "http://pique993.prv.pl/index2.php";

        WWW www = new WWW(url, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.text;
            string[] jsonSplit = json.Split('<');
            
            ResponseCls response = JsonUtility.FromJson<ResponseCls>(jsonSplit[0]);

            FPSFromJsonClass FPSClassObj = JsonConvert.DeserializeObject<FPSFromJsonClass>(jsonSplit[0]);

            foreach (var x in FPSClassObj.content)
            {
                var convertAvgFPS = x.avgFPS.Replace('.', ',');
                FPSListFromDB.Add(float.Parse(convertAvgFPS));
            }

        }
    }


    public void sendScoresToDB(string CPUValue, string FPSValue)
    {
        CPUValue = CPUValue.Replace(',', '.');
        FPSValue = FPSValue.Replace(',', '.');

        StartCoroutine(sendScores(CPUValue, FPSValue));
    }

    private IEnumerator sendScores(string CPUValue, string FPSValue)
    {

        WWWForm form = new WWWForm();
        form.AddField("CPUName", CPUValue);
        form.AddField("FPSName", FPSValue);


        string url = "http://pique993.prv.pl/index2.php";

        WWW www = new WWW(url, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            //print(www.error);
            Debug.Log(www.error);
        }
        else
        {
            string json = www.text;
            string[] jsonSplit = json.Split('<');

            Debug.Log(jsonSplit[0]);
            ResponseCls response = JsonUtility.FromJson<ResponseCls>(jsonSplit[0]);

            Debug.Log(response.msg);


        }
    }

}

[Serializable]
public class ResponseCls
{
    public int status;
    public string msg;
}

public class FPSContent
{
    public string avgFPS { get; set; }
}

public class FPSFromJsonClass
{
    public int status { get; set; }
    public string msg { get; set; }
    public List<FPSContent> content { get; set; }
}

public class CPUContent
{
    public string avgCPU { get; set; }
}

public class CPUFromJsonClass
{
    public int status { get; set; }
    public string msg { get; set; }
    public List<CPUContent> content { get; set; }
}



