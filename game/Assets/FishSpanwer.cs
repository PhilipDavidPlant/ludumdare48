using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishSpanwer : MonoBehaviour
{
    [SerializeField] private GameObject _dion;
    [SerializeField] private List<GameObject> _fishes;
    [SerializeField] private int _desiredFishSpawned = 5;

    private List<GameObject> _spawnedFishes = new List<GameObject>();

    void Start()
    {
        StartCoroutine("SpawnFish");
    }

    IEnumerator SpawnFish()
    {
        for (; ; )
        {

            var nearbyFishes = 0;
            _spawnedFishes
                .Where(fish => fish != null)
                .ToList()
                .ForEach(fish =>
                {
                    var distance = Vector3.Distance(fish.transform.position, _dion.transform.position);
                    nearbyFishes += distance <= 15.0 ? 1 : 0;
                });

            for (int i = 0; i < Mathf.Max(0, _desiredFishSpawned - nearbyFishes); i++)
            {
                var spawnPosition = new Vector3(
                    _dion.transform.position.x + Random.Range(-10, 10),
                    Mathf.Min(_dion.transform.position.y + Random.Range(-10, 10), -5), // Don't spawn above sea level
                    _dion.transform.position.z
                );

                _spawnedFishes.Add(Instantiate(FindFish(Mathf.Abs(_dion.transform.position.y)), spawnPosition, Quaternion.identity));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private GameObject FindFish(float depth)
    {
        var fishesAtDepth = _fishes.Where(fish =>
        {
            var fishComponent = fish.GetComponent<Fish>();
            return depth >= fishComponent.minDepth && depth <= fishComponent.maxDepth;
        }).ToList();
        var index = Random.Range(0, fishesAtDepth.Count);
        return fishesAtDepth.ElementAt(index);
    }
}
