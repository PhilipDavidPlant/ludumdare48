using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
   private Text _text;

   void Start() 
   {
       _text = GetComponent<Text>();
       EventManager.OnMoneyChanged += UpdateText;
   }

   void UpdateText(int value) 
   {
       _text.text = $"$ {value}";
   }

}
