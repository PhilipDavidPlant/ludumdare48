using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Text _topText;
    [SerializeField] private Text _bottomText;
    [SerializeField] private GameObject _deathMenu;

    private int initialHighscore;
    void Start()
    {
        EventManager.OnPlayerDied += HandlePlayerDied;
        initialHighscore = PlayerPrefs.GetInt("highscore", 0);
    }

    private void OnDestroy() 
    {
        EventManager.OnPlayerDied -= HandlePlayerDied;
    }

    void HandlePlayerDied(int maxDepth)
    {
        _deathMenu.SetActive(true);
        if (initialHighscore > maxDepth)
        {
            _topText.text = $"Reached {maxDepth}m";
            _bottomText.text = $"Personal best: {initialHighscore}m";
        } else 
        {
            _topText.text = $"Oh baby, a triple!";
            _bottomText.text = $"New best: {maxDepth}m";
        }
    }
}
