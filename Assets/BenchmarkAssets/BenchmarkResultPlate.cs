using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BenchmarkResultPlate : MonoBehaviour
{
    public ConnectDataBase dataBase;
    public TextMeshPro avgFPSAll;
    public TextMeshPro avgCPUAll;

    public TextMeshPro avgFPSAllPercent;
    public TextMeshPro avgCPUAllPercent;

    public List<TextMeshPro> FPSList = new List<TextMeshPro>();
    public List<TextMeshPro> CPUList = new List<TextMeshPro>();

    public float avgFPSAllNumber;
    public float avgCPUAllNumber;


    public List<float> FPSListFromDB = new List<float>();
    public List<float> CPUListFromDB = new List<float>();

    public void createChartCPU()
    {
        StartCoroutine(createChartCPUCor());
    }

    IEnumerator createChartCPUCor()
    {
        yield return new WaitForFixedUpdate();

        List<float> numbersForChart = new List<float>();
        dataBase.downloadCPU();

        yield return new WaitForSeconds(3);
        CPUListFromDB = dataBase.CPUListFromDB;


        if (CPUListFromDB.Count <= 11)
        {
            numbersForChart = CPUListFromDB;
        }
        else
        {
            /*//Test
            avgCPUAllNumber = 21.0f;*/

            int currentResulIndex = CPUListFromDB.IndexOf(avgCPUAllNumber);

            float percent = (float)(currentResulIndex + 1) / (float)(CPUListFromDB.Count);
            percent = percent * 100;

            avgCPUAllPercent.text = "Worst than: " + percent.ToString("F2") + " %";

            // Jest mniej
            if (currentResulIndex - 5 >= 0)
            {
                // Jest więcej
                if (currentResulIndex + 5 <= FPSListFromDB.Count - 1)
                {
                    numbersForChart.Add(CPUListFromDB[0]);
                    numbersForChart.Add(CPUListFromDB[1]);

                    for (int i = currentResulIndex - 3; i <= currentResulIndex + 3; i++)
                    {
                        numbersForChart.Add(CPUListFromDB[i]);
                    }

                    numbersForChart.Add(CPUListFromDB[CPUListFromDB.Count - 1]);
                    numbersForChart.Add(CPUListFromDB[CPUListFromDB.Count - 2]);
                }
                else // Nie ma więcej
                {
                    for (int i = currentResulIndex - 3; i <= CPUListFromDB.Count - 1; i++)
                    {
                        numbersForChart.Add(CPUListFromDB[i]);
                    }

                    numbersForChart.Add(CPUListFromDB[0]);
                    numbersForChart.Add(CPUListFromDB[1]);
                }
            }
            else // Nie ma mniej
            {
                // Jest więcej
                if (currentResulIndex + 5 <= CPUListFromDB.Count - 1)
                {
                    for (int i = 0; i <= currentResulIndex + 3; i++)
                    {
                        numbersForChart.Add(CPUListFromDB[i]);
                    }

                    numbersForChart.Add(CPUListFromDB[CPUListFromDB.Count - 1]);
                    numbersForChart.Add(CPUListFromDB[CPUListFromDB.Count - 2]);
                }
                else // Nie ma więcej
                {
                    for (int i = 0; i <= CPUListFromDB.Count - 1; i++)
                    {
                        numbersForChart.Add(CPUListFromDB[i]);
                    }
                }
            }


        }

        GameObject startProjectObject = GameObject.Find("ScriptHandler");
        startProjectObject.GetComponent<createChart>().createChartOfColumn(numbersForChart, this.transform, "CPU", avgCPUAllNumber);
    }


    public void createChartFPS()
    {
        StartCoroutine(createChartFPSCor());

    }

    IEnumerator createChartFPSCor()
    {
        yield return new WaitForFixedUpdate();

        List<float> numbersForChart = new List<float>();
        dataBase.downloadFPS();

        yield return new WaitForSeconds(3);
        FPSListFromDB = dataBase.FPSListFromDB;


        if (FPSListFromDB.Count <= 11)
        {
            numbersForChart = FPSListFromDB;
        }
        else
        {
            /*//Test
            avgFPSAllNumber = 29.0f;*/

            int currentResulIndex = FPSListFromDB.IndexOf(avgFPSAllNumber);

            float percent = (float)(currentResulIndex + 1) / (float)(FPSListFromDB.Count);
            percent = (1 - percent) * 100;


            avgFPSAllPercent.text = "Worst than: " + percent.ToString("F2") + " %";

            // Jest mniej
            if (currentResulIndex - 5 >= 0)
            {
                // Jest więcej
                if (currentResulIndex + 5 <= FPSListFromDB.Count - 1)
                {
                    numbersForChart.Add(FPSListFromDB[0]);
                    numbersForChart.Add(FPSListFromDB[1]);

                    for (int i = currentResulIndex - 3; i <= currentResulIndex + 3; i++)
                    {
                        numbersForChart.Add(FPSListFromDB[i]);
                    }

                    numbersForChart.Add(FPSListFromDB[FPSListFromDB.Count - 1]);
                    numbersForChart.Add(FPSListFromDB[FPSListFromDB.Count - 2]);
                }
                else // Nie ma więcej
                {
                    for (int i = currentResulIndex - 3; i <= FPSListFromDB.Count - 1; i++)
                    {
                        numbersForChart.Add(FPSListFromDB[i]);
                    }

                    numbersForChart.Add(FPSListFromDB[0]);
                    numbersForChart.Add(FPSListFromDB[1]);
                }
            }
            else // Nie ma mniej
            {
                // Jest więcej
                if (currentResulIndex + 5 <= FPSListFromDB.Count - 1)
                {
                    for (int i = 0; i <= currentResulIndex + 3; i++)
                    {
                        numbersForChart.Add(FPSListFromDB[i]);
                    }

                    numbersForChart.Add(FPSListFromDB[FPSListFromDB.Count - 1]);
                    numbersForChart.Add(FPSListFromDB[FPSListFromDB.Count - 2]);
                }
                else // Nie ma więcej
                {
                    for (int i = 0; i <= FPSListFromDB.Count - 1; i++)
                    {
                        numbersForChart.Add(FPSListFromDB[i]);
                    }
                }
            }


        }

        GameObject startProjectObject = GameObject.Find("ScriptHandler");
        startProjectObject.GetComponent<createChart>().createChartOfColumn(numbersForChart, this.transform, "FPS", avgFPSAllNumber);
    }
}
