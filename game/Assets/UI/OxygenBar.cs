using UnityEngine;
using UnityEngine.UI;
using Dion;
public class OxygenBar : MonoBehaviour
{
    [SerializeField] private DionController _dionController;
    [SerializeField] private Image _fill;

    void Update()
    {
        _fill.fillAmount = _dionController.oxygenLeft / _dionController.maxOxygen;
    }
}
