using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void MoneyChanged(int value);
    public static event MoneyChanged OnMoneyChanged;

    public delegate void PlayerDied(int maxDepth);
    public static event PlayerDied OnPlayerDied;

    public static void ChangeMoney(int newValue) 
    {
        if (OnMoneyChanged != null) 
        {
            OnMoneyChanged(newValue);
        }
    }

    public static void KillPlayer(int maxDepth)
    {
        if(OnPlayerDied != null)
        {
            OnPlayerDied(maxDepth);
        }
    }

}
