using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class connectBD : MonoBehaviour
{
    public Text login_input;
    public Text password_input;
    public Text output;

    public void performLogin()
    {

        StartCoroutine(makeLogin());
    }

    private IEnumerator makeLogin()
    {

        WWWForm form = new WWWForm();
        form.AddField("FPSName", login_input.text);
        form.AddField("CPUName", password_input.text);

        string url = "http://pique993.prv.pl/index2.php";

        WWW www = new WWW(url, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else
        {
            string json = www.text;
            string[] jsonSplit = json.Split('<');
            Debug.Log(jsonSplit[0]);
            ResponseClass response = JsonUtility.FromJson<ResponseClass>(jsonSplit[0]);

            output.text = response.msg;
        }
    }

}

[Serializable]
public class ResponseClass
{
    public int status;
    public string msg;
}

