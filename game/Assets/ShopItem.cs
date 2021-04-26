using UnityEngine;
using UnityEngine.UI;
using Dion;
public class ShopItem : MonoBehaviour
{
    [SerializeField] private float _price;

    private Button _buyButton;
    private DionController _dionController;

    void Start() 
    {
        _buyButton = GetComponentInChildren<Button>();
        _dionController = FindObjectOfType<DionController>();
        _buyButton.interactable = _dionController.money >= _price;
        _buyButton.transform.GetComponentInChildren<Text>().text = $"$ {_price}";

        EventManager.OnMoneyChanged += HandleDionsMoneyChanged;
    }

    private void HandleDionsMoneyChanged(int newValue)
    {
        _buyButton.interactable = newValue >= _price;
    }
}
