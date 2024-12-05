using System;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    private int hour = 0;
    private int minute = 0;
    private int second = 0;
    private TMP_Text textClock; 
    private float elapsedTime = 0f; 
    private bool stopClock = false;

    private void Awake()
    {
        textClock = GetComponent<TMP_Text>(); 
    }

    void Start()
    {
        stopClock = false;
        elapsedTime = 0f; 
    }

    void Update()
    {
        if (!stopClock)
        {
            elapsedTime += Time.deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(elapsedTime);
            hour = span.Hours;
            minute = span.Minutes;
            second = span.Seconds;

            textClock.text = LeadingZero(hour) + ":" + LeadingZero(minute) + ":" + LeadingZero(second);
        }
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void OnGameOver()
    {
        stopClock = true;
    }

    public void OnStartGame()
    {
        stopClock = false;
        elapsedTime = 0f;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

}