using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{

    public Text FPSText;
    float FPSNumber;
    float summaryFPS;
    int samplesNumber;
    bool countingFPS = false;

    IEnumerator Start()
    {
        while (true)
        {
            FPSNumber = (1 / Time.deltaTime);
            FPSText.text = "FPS :" + (Mathf.Round(FPSNumber));

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void startCountFPS()
    {
        summaryFPS = 0;
        samplesNumber = 0;
        countingFPS = true;
        StartCoroutine(countFPSCor());
    }

    public int stopCountFPS()
    {
        countingFPS = false;
        int avgFPS = (int)(summaryFPS / samplesNumber);
        return avgFPS;
    }

    IEnumerator countFPSCor()
    {
        while (countingFPS)
        {
            

            FPSNumber = (1 / Time.deltaTime);
            FPSText.text = "FPS :" + (Mathf.Round(FPSNumber));

            summaryFPS += FPSNumber;
            samplesNumber++;

            yield return new WaitForSeconds(0.5f);
        }
    }
}