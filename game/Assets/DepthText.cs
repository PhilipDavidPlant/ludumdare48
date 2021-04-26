using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DepthText : MonoBehaviour
{
    [SerializeField] private Transform _dionTransform;

    private Text _text;
    private int _currentBest;

    void Start()
    {
        _text = GetComponent<Text>();
        _currentBest = PlayerPrefs.GetInt("highscore", 0);
        StartCoroutine("CheckDepth");
    }

    IEnumerator CheckDepth()
    {
        for(;;)
        {
            var depth = (int) Mathf.Abs(_dionTransform.position.y);
            _text.text = $"Depth: {depth}m";

            if(depth > _currentBest) 
            {
                PlayerPrefs.SetInt("highscore", depth);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
