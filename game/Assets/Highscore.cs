using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    
    private Text _text;
    void Start()
    {
        _text = GetComponent<Text>();
        var highscore = PlayerPrefs.GetInt("highscore", 0);
        var text = highscore > 0 ? $"Highscore: {highscore}m" : "No highscore yet =(";
        _text.text = text;
    }

}
