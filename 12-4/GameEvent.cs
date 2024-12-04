using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void UpdateSquareNumber(int number);
    public static UpdateSquareNumber onUpdateSquareNumber;

    public static void UpdateSquareNumberMethod(int number)
    {
        if(onUpdateSquareNumber != null)
        {
            onUpdateSquareNumber(number);
        }
    }
}
