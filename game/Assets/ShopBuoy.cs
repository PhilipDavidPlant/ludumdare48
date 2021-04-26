using UnityEngine;
using Dion;

public class ShopBuoy : MonoBehaviour
{
    [SerializeField] private GameObject _shopCanvas;

    private void OnTriggerEnter(Collider other) 
    {
        var dion = other.GetComponent<DionController>();
        if (dion != null) 
        {
            _shopCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        var dion = other.GetComponent<DionController>();
        if (dion != null) 
        {
            _shopCanvas.SetActive(false);
        }
    }
}
