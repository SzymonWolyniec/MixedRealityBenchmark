using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class createChart : MonoBehaviour
{
    public GameObject infoText;
    public GameObject singleElementOfChart;
    public GameObject singleElementOfChartText;
    public GameObject singleElementOfChartParent;
    public GameObject LineObject;
    public float breakBetweenBars = 1;
    public float maxHeightOfBars;
    public float barWidth;
    public float barDepth;
    public bool sort = false;


    List<float> numberForChart = new List<float>();
    double min, max, colorUnit, heightUnit;
    float normalization = 0;
    Transform background;

    public void createChartOfColumn(List<float> numberForChart, Transform background, string chartInfo, float currentNumber)
    {
        this.numberForChart = numberForChart;
        this.background = background;

        StartCreatingChart(chartInfo, currentNumber);
    }

    void StartCreatingChart(string chartInfo, float currentNumber)
    {
        if (sort)
            findMinMaxAndSort(numberForChart);
        else
            findMinMax(numberForChart);

        //colorUnit = 0.8f / (numberForChart.Count - 1);

        Color[] colorTab;
        if (chartInfo == "FPS")
        {
            colorTab = new Color[] { new Color(0.65f, 0, 0), new Color(0, 0.65f, 0), new Color(0, 0, 0.65f), new Color(0.35f, 0.35f, 0.35f) };
        }
        else
        {
            colorTab = new Color[] { new Color(0, 0.65f, 0), new Color(0.65f, 0, 0), new Color(0, 0, 0.65f), new Color(0.35f, 0.35f, 0.35f) };
        }


        float breakBetweenElements = singleElementOfChart.transform.localScale.x * (breakBetweenBars + 1);

        Vector3 spawnObjPosition = Camera.main.transform.position + Camera.main.transform.forward * 1f;
        Quaternion spawnObjRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);

        var parent = Instantiate(singleElementOfChartParent, background.transform.position, background.transform.rotation);

        // Rescale bars
        if (Math.Abs(min) < max)
        {
            heightUnit = maxHeightOfBars / max;
        }
        else
        {
            heightUnit = maxHeightOfBars / Math.Abs(min);
        }



        int i = 0;
        foreach (var values in numberForChart)
        {

            var singleElement = Instantiate(singleElementOfChart, parent.transform);
            var singleElementText = Instantiate(singleElementOfChartText, parent.transform);

            var textPro = singleElementText.GetComponent<TextMeshPro>();
            textPro.text = values.ToString();
            if (values < 0)
            {
                textPro.alignment = TextAlignmentOptions.MidlineRight;
                singleElementText.GetComponent<RectTransform>().localPosition += Vector3.up * 0.02f;
            }
            else
            {
                singleElementText.GetComponent<RectTransform>().localPosition -= Vector3.up * 0.02f;
            }



            normalization = (float)(values * heightUnit);

            if (max > 0 && min > 0)
            {
                normalization = (float)((values - min) / (max - min) * maxHeightOfBars);
            }
            else if (max < 0 && min < 0)
            {
                normalization = -((float)((values - max) / (max - min) * -maxHeightOfBars));
            }


            singleElementText.transform.position += singleElementText.transform.right * i * breakBetweenElements;
            singleElementText.transform.position += singleElementText.transform.forward * -barDepth;

            singleElementText.transform.rotation = Quaternion.Euler(singleElementText.transform.rotation.eulerAngles.x, singleElementText.transform.rotation.eulerAngles.y, singleElementText.transform.rotation.eulerAngles.z - 85);

            singleElement.transform.position += singleElement.transform.right * i * breakBetweenElements;
            // singleElement.transform.position += new Vector3(0, normalization / 2, 0);
            singleElement.transform.position += singleElement.transform.up * (normalization / 2);
            singleElement.transform.localScale = new Vector3(barWidth, normalization, barDepth);

            //singleElement.GetComponent<Renderer>().material.color = Color.HSVToRGB((float)colorUnit * i, 1, 1);

            int indexOfCurrentResult = numberForChart.IndexOf(currentNumber);
            if (indexOfCurrentResult >= 1)
            {
                if (indexOfCurrentResult <= numberForChart.Count - 2)
                {
                    if (i == 0 || i == 1)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[0];
                    }
                    else if (i == numberForChart.Count - 1 || i == numberForChart.Count - 2)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[1];
                    }
                    else
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[3];
                    }

                    if (i == indexOfCurrentResult)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[2];
                    }
                }
                else
                {
                    if (i == 0 || i == 1)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[0];
                    }
                    else
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[3];
                    }

                    if (i == indexOfCurrentResult)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[2];
                    }
                }
            }
            else
            {
                if (indexOfCurrentResult <= numberForChart.Count - 2)
                {
                    if (i == numberForChart.Count - 1 || i == numberForChart.Count - 2)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[1];
                    }
                    else
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[3];
                    }

                    if (i == indexOfCurrentResult)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[2];
                    }
                }
                else
                {
                    if (i == numberForChart.Count - 1 || i == numberForChart.Count - 2)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[1];
                    }
                    else
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[3];
                    }

                    if (i == indexOfCurrentResult)
                    {
                        singleElement.GetComponent<Renderer>().material.color = colorTab[2];
                    }
                }
            }

            i++;
        }




        var sizesOfCharList = parent.GetComponent<setParentOfChartCollider>().setCollider(barWidth, maxHeightOfBars, barDepth, breakBetweenElements, (float)min, (float)max, numberForChart.Count);

        var heightOfChart = sizesOfCharList[0];
        var heightOfChartCenter = sizesOfCharList[1];
        var widthOfChartCenter = sizesOfCharList[2];


        var scaler = 1 / (parent.GetComponent<BoxCollider>().size.x / background.transform.localScale.x);
        // parent.transform.localScale *= scaler;

        if (chartInfo == "FPS")
        {
            parent.transform.position -= parent.transform.up * 0.128437329f;
            parent.transform.position += parent.transform.right * 0.76f;


            var infoTxt = Instantiate(infoText, parent.transform);
            infoTxt.transform.position += infoTxt.transform.up * 0.3f;
            infoTxt.transform.position += infoTxt.transform.right * 0.3f;

            infoTxt.GetComponentInChildren<TextMeshPro>().text = "FPS";
        }
        else
        {
            parent.transform.position -= parent.transform.up * 0.128437329f;
            parent.transform.position -= parent.transform.right * 0.58f;


            var infoTxt = Instantiate(infoText, parent.transform);
            infoTxt.transform.position += infoTxt.transform.up * 0.3f;
            infoTxt.transform.position += infoTxt.transform.right * 0.3f;

            infoTxt.GetComponentInChildren<TextMeshPro>().text = "CPU usage [%]";
        }


        //parent.transform.position -= parent.transform.up * (background.transform.localScale.y / 2 + scaler * heightOfChartCenter + scaler * heightOfChart / 2);
        //parent.transform.position -= parent.transform.right * scaler * widthOfChartCenter;

    }

    private void findMinMax(List<float> myData)
    {

        List<float> myDataSort = new List<float>(myData);
        myDataSort.Sort();
        min = myDataSort[0];
        max = myDataSort[myDataSort.Count - 1];
    }

    private void findMinMaxAndSort(List<float> myData)
    {
        myData.Sort();
        min = myData[0];
        max = myData[myData.Count - 1];
    }
}
