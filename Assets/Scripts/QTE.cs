using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

//Scripted by Ellie Macdonald
//Prerequisites: A TMPro text object, proper input axes in the project settings
//Output: A simple QTE popup that displays a key from E/R/T and debugs a win/lose response



public class QTE : MonoBehaviour
{
    //Settings
    public float timeToWait;


    public TMP_Text keyText;
    private int QTEGenKey=4;
    public bool waitingForKey=false;
    private bool countingDown;
    private bool correctKey;

    public void initiateQTE()
    {
        waitingForKey = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForKey)
        {
            QTEGenKey = Random.Range(1, 4); //1-3
            countingDown = true;
            StartCoroutine(CountDown());

            if(QTEGenKey == 1)
            {
                keyText.text = "|E|";
            }
            else if(QTEGenKey == 2)
            {
                keyText.text = "|R|";
            }
            else if(QTEGenKey == 3)
            {
                keyText.text = "|T|";
            }
            waitingForKey = false;
        }

        if(QTEGenKey == 1)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButtonDown("E"))
                {
                    correctKey = true;
                    StartCoroutine(keyPressing());
                }
                else
                {
                    correctKey = false;
                    StartCoroutine(keyPressing());
                }
            }
        }
        else if (QTEGenKey == 2)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButtonDown("R"))
                {
                    correctKey = true;
                    StartCoroutine(keyPressing());
                }
                else
                {
                    correctKey = false;
                    StartCoroutine(keyPressing());
                }
            }
        }
        else if (QTEGenKey == 3)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButtonDown("T"))
                {
                    correctKey = true;
                    StartCoroutine(keyPressing());
                }
                else
                {
                    correctKey = false;
                    StartCoroutine(keyPressing());
                }
            }
        }
    }


    IEnumerator keyPressing()
    {
        QTEGenKey = 4; //external range for recogntion
        if (correctKey)
        {
            countingDown = false;
            Debug.Log("win"); //TODO replace with event on qte success
            keyText.text = "|win|";
            yield return new WaitForSeconds(1);
            correctKey = false;
            yield return new WaitForSeconds(3);
            //waitingForKey = true;
            countingDown = true;
        }
        else
        {
            countingDown = false;
            Debug.Log("lose"); //TODO replace with event on qte fail
            keyText.text = "|lose|";
            yield return new WaitForSeconds(1);
            correctKey = false;
            yield return new WaitForSeconds(3);
            //waitingForKey = true;
            countingDown = true;
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(timeToWait);
        if (countingDown)
        {
            QTEGenKey = 4; //external range for recogntion
            countingDown = false;
            Debug.Log("lose"); //TODO event on fail
            keyText.text = "|lose|";

        }
    }
}
