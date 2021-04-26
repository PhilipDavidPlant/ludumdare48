using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void MoneyChanged(int value);
    public static event MoneyChanged OnMoneyChanged;

    public static void ChangeMoney(int newValue) 
    {
        if (OnMoneyChanged != null) 
        {
            OnMoneyChanged(newValue);
        }
    }

}
