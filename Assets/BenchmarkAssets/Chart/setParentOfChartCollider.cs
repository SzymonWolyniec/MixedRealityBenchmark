using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setParentOfChartCollider : MonoBehaviour
{
    public List<float> setCollider(float barWidth, float maxHeightOfBars, float barDepth, float breakBetweenElements, float min, float max, int barCount)
    {
        var boxCollider = this.GetComponent<BoxCollider>();

        // var width = (breakBetweenElements * (barCount - 1) + barWidth);

        var width = breakBetweenElements * barCount;

        float height = maxHeightOfBars;
        float heightCenter = 0;

        if (max >= 0 && min >= 0)
        {
            heightCenter = maxHeightOfBars / 2;
        }
        else if (max <= 0 && min <= 0)
        {
            heightCenter = -maxHeightOfBars / 2;
        }
        else if (max >= Mathf.Abs(min))
        {
            height = maxHeightOfBars + (Mathf.Abs(min) / max * maxHeightOfBars);
            heightCenter = maxHeightOfBars - ((maxHeightOfBars + (Mathf.Abs(min) / max * maxHeightOfBars)) / 2);
        }
        else if (max < Mathf.Abs(min))
        {
            height = maxHeightOfBars + (max / Mathf.Abs(min) * maxHeightOfBars);
            heightCenter = -maxHeightOfBars + ((maxHeightOfBars + (max / Mathf.Abs(min) * maxHeightOfBars)) / 2);
        }

        //var widthCenter = (width - barWidth) / 2;
        var widthCenter = width / 2 - barWidth;

        boxCollider.size = new Vector3(width, height, barDepth);
        boxCollider.center = new Vector3(widthCenter, heightCenter, 0);

        List<float> returnList = new List<float>();
        returnList.Add(height);
        returnList.Add(heightCenter);
        returnList.Add(widthCenter);

        return returnList;
    }
}
