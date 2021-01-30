using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartBenchmark : MonoBehaviour
{
    public FPS FPSScript;
    public CPU CPUScript;
    public ConnectDataBase dataBaseScript;
    public GameObject benchmarkRestultPlateObj;
    public GameObject startBtnsContainer;
    public GameObject singleBallObj;
    public GameObject ballsPosition;
    public Transform ballsContainer;
    public GameObject ballsRamp;

    public GameObject singleDominoObj;
    public Transform dominosContainer;

    public GameObject singleAnimatorObj;
    public Transform animatorsContainer;





    List<double> CPUScores = new List<double>();
    List<int> FPSScores = new List<int>();

    public void onStartClick()
    {
        Time.timeScale = 1;
        StartCoroutine(hideStartBtns());
        StartCoroutine(startClick());
    }

    public IEnumerator startClick()
    {
        yield return new WaitForFixedUpdate();

        // 4x Balls Test
        StartCoroutine(BallsTest(1, 5, 5));
        yield return new WaitForSeconds(15);

        StartCoroutine(BallsTest(10, 10, 10));
        yield return new WaitForSeconds(15);

        StartCoroutine(BallsTest(10, 20, 10));
        yield return new WaitForSeconds(15);

        StartCoroutine(BallsTest(10, 30, 10));
        yield return new WaitForSeconds(15);

        ballsRamp.SetActive(false);

        //4x Domino Test
        yield return new WaitForSeconds(5);
        StartCoroutine(DominoTestLow(1f, 100));
        yield return new WaitForSeconds(15);

        StartCoroutine(DominoTestLow(1f, 200));
        yield return new WaitForSeconds(15);

        StartCoroutine(DominoTest(2f, 1000));
        yield return new WaitForSeconds(20);

        StartCoroutine(DominoTest(2f, 2000));
        yield return new WaitForSeconds(20);

        //4x Animation Test
        StartCoroutine(StartAnimation(100));
        yield return new WaitForSeconds(10);

        StartCoroutine(StartAnimation(200));
        yield return new WaitForSeconds(10);

        StartCoroutine(StartAnimation(500));
        yield return new WaitForSeconds(10);

        StartCoroutine(StartAnimation(1000));
        yield return new WaitForSeconds(10);


        // TEST
        //test();
        StartCoroutine(Score());

    }

    public void test()
    {
        double d = 2.22;
        int calk = 4;
        int x;

        for (int i = 0; i < 12; i++)
        {
            CPUScores.Add(63.0f);

           
            FPSScores.Add(55);
        }
    }

    public IEnumerator Score()
    {
        yield return new WaitForFixedUpdate();

        double CPUScore = 0;
        float FPSScore = 0;

        Quaternion spawnObjRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        Vector3 spawnObjPosition = Camera.main.transform.position + Camera.main.transform.forward * 1;

        var resultPlate = Instantiate(benchmarkRestultPlateObj, spawnObjPosition, spawnObjRotation);
        var resultPlateScript = resultPlate.GetComponent<BenchmarkResultPlate>();

        int i = 0;

        CPUScores.ForEach(delegate (double singleScore)
        {
            resultPlateScript.CPUList[i].text = "Avg. CPU: " + singleScore.ToString() + " %";
            CPUScore += singleScore;
            i++;
        });

        i = 0;

        FPSScores.ForEach(delegate (int singleScore)
        {
            resultPlateScript.FPSList[i].text = "Avg. FPS: " + singleScore.ToString();
            FPSScore += singleScore;
            i++;
        });

        string avgCPU = (CPUScore / 12).ToString("F2");
        string avgFPS = (FPSScore / 12).ToString("F2");

        resultPlateScript.avgCPUAll.text = "Avg. CPU: " + avgCPU + " %";
        resultPlateScript.avgFPSAll.text = "Avg. FPS: " + avgFPS;

        resultPlateScript.avgCPUAllNumber = (float)Math.Round((CPUScore / 12), 2);
        resultPlateScript.avgFPSAllNumber = (float)Math.Round((FPSScore / 12), 2);

        dataBaseScript.sendScoresToDB(avgCPU, avgFPS);

        yield return new WaitForSeconds(5);

        resultPlateScript.createChartFPS();

        resultPlateScript.createChartCPU();

    }



    public IEnumerator StartAnimation(float numberOfAnimation)
    {
        yield return new WaitForFixedUpdate();

        FPSScript.startCountFPS();
        CPUScript.startCountCPU();

        for (int i = 0; i < numberOfAnimation; i++)
        {
            float posX = UnityEngine.Random.Range(-6.5f, 3.2f);
            float posZ = UnityEngine.Random.Range(-4.5f, 4.8f);

            Instantiate(singleAnimatorObj, new Vector3(posX, -1, posZ), Quaternion.identity, animatorsContainer);

        }

        yield return new WaitForSeconds(7);

        FPSScores.Add(FPSScript.stopCountFPS());
        CPUScores.Add(CPUScript.stopCountCPU());

        foreach (Transform child in animatorsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public IEnumerator DominoTest(float radius, int numberOfBlocks)
    {
        yield return new WaitForFixedUpdate();
        FPSScript.startCountFPS();
        CPUScript.startCountCPU();

        Vector3 spawnObjPosition = Camera.main.transform.position + Camera.main.transform.forward * 3;

        var firstDomino = Instantiate(singleDominoObj, new Vector3(spawnObjPosition.x, -0.36385f, spawnObjPosition.z), Quaternion.identity, dominosContainer);


        float rotationY = (float)360 / numberOfBlocks;


        for (int i = 0; i < numberOfBlocks; i++)
        {
            var angle = (i * Mathf.PI * 2) / numberOfBlocks;
            var pos = new Vector3(firstDomino.transform.position.x + Mathf.Cos(angle) * radius, firstDomino.transform.position.y, firstDomino.transform.position.z + Mathf.Sin(angle) * radius);

            var newClonedObject = Instantiate(firstDomino, pos, Quaternion.Euler(0, -(i * rotationY), 0), dominosContainer);

            newClonedObject.GetComponent<Renderer>().material.color = changeColorDomino(i);

            if (i % 5 == 0)
            {
                newClonedObject.transform.eulerAngles = new Vector3(newClonedObject.transform.rotation.x - 30, -(i * rotationY), 0);
            }
        }

        firstDomino.SetActive(false);

        yield return new WaitForSeconds(15);

        FPSScores.Add(FPSScript.stopCountFPS());
        CPUScores.Add(CPUScript.stopCountCPU());

        foreach (Transform child in dominosContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }

    public IEnumerator DominoTestLow(float radius, int numberOfBlocks)
    {
        yield return new WaitForFixedUpdate();
        FPSScript.startCountFPS();
        CPUScript.startCountCPU();

        Vector3 spawnObjPosition = Camera.main.transform.position + Camera.main.transform.forward * 3;

        var firstDomino = Instantiate(singleDominoObj, new Vector3(spawnObjPosition.x, -0.36385f, spawnObjPosition.z), Quaternion.identity, dominosContainer);


        float rotationY = (float)360 / numberOfBlocks;


        for (int i = 0; i < numberOfBlocks; i++)
        {

            var angle = (i * Mathf.PI * 2) / numberOfBlocks;
            var pos = new Vector3(firstDomino.transform.position.x + Mathf.Cos(angle) * radius, firstDomino.transform.position.y, firstDomino.transform.position.z + Mathf.Sin(angle) * radius);

            var newClonedObject = Instantiate(firstDomino, pos, Quaternion.Euler(0, -(i * rotationY), 0), dominosContainer);

            newClonedObject.GetComponent<Renderer>().material.color = changeColorDomino(i);

            //pochylenie ostatniego elementu, aby domino leciało
            if (i == (numberOfBlocks - 1))
            {
                newClonedObject.transform.eulerAngles = new Vector3(newClonedObject.transform.rotation.x - 30, -(i * rotationY), 0);
            }



        }

        firstDomino.SetActive(false);

        yield return new WaitForSeconds(10);

        FPSScores.Add(FPSScript.stopCountFPS());
        CPUScores.Add(CPUScript.stopCountCPU());

        foreach (Transform child in dominosContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }

    Color changeColorDomino(int j)
    {
        if (j % 8 == 0)
        {
            return Color.red;
        }
        else if (j % 8 == 1)
        {
            return Color.yellow;
        }
        else if (j % 8 == 2)
        {
            return Color.green;
        }
        else if (j % 8 == 3)
        {
            return Color.cyan;
        }
        else if (j % 8 == 4)
        {
            return Color.blue;
        }
        else if (j % 8 == 5)
        {
            return Color.magenta;
        }
        else if (j % 8 == 6)
        {
            return Color.gray;
        }
        else if (j % 8 == 7)
        {
            return Color.black;
        }
        else
        {
            return Color.clear;
        }

    }

    public IEnumerator BallsTest(int ballsInWidth, int ballsInHeight, int ballsInDeepth)
    {
        yield return new WaitForFixedUpdate();
        FPSScript.startCountFPS();
        CPUScript.startCountCPU();

        var ballPositionX = ballsPosition.transform.position.x;
        var ballPositionY = ballsPosition.transform.position.y;
        var ballPositionZ = ballsPosition.transform.position.z;


        for (int i = 0; i < ballsInHeight; i++)
        {
            for (int j = 0; j < ballsInWidth; j++)
            {
                for (int k = 0; k < ballsInDeepth; k++)
                {
                    var newBall = Instantiate(singleBallObj, new Vector3(ballPositionX + (0.1f * j), ballPositionY + (1f * i), ballPositionZ + (0.1f * k)), Quaternion.identity, ballsContainer);
                    newBall.GetComponent<Renderer>().material.color = changeColor(i);
                }
            }
        }

        yield return new WaitForSeconds(10);

        //var allChildren = ballsContainer.GetComponentsInChildren<Transform>();

        FPSScores.Add(FPSScript.stopCountFPS());
        CPUScores.Add(CPUScript.stopCountCPU());

        foreach (Transform child in ballsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }

    Color changeColor(int j)
    {
        if (j % 4 == 0)
        {
            return Color.red;
        }
        else if (j % 4 == 1)
        {
            return Color.yellow;
        }
        else if (j % 4 == 2)
        {
            return Color.green;
        }
        else
        {
            return Color.blue;
        }

    }

    IEnumerator hideStartBtns()
    {
        yield return new WaitForSeconds(1);
        startBtnsContainer.SetActive(false);
    }
}
