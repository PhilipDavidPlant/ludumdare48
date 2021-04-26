using UnityEngine;

public class SpearShooter : MonoBehaviour
{
    [SerializeField] private float _shootForce;
    [SerializeField] private float _spearReturnSpeed;
    [SerializeField] private float _shootDelay = 0.5f;
    [SerializeField] private GameObject _spear;
    
    public int availableSpears = 1;
    private float _lastShot = -0.5f;
    
    void Update()
    {
        if(Input.GetAxisRaw("Fire1") > 0 && availableSpears > 0 && Time.time > _lastShot + _shootDelay) { 
            Shoot();
        }
    }

    private void Shoot() {
        Debug.Log("Shoot spear");
        var other = GameObject.Instantiate(_spear, transform.position, Quaternion.identity).GetComponent<Spear>();
        other.Initialize(_shootForce, transform, _spearReturnSpeed);
        availableSpears--;
        _lastShot = Time.time;
    }

    public void Upgrade(float percentage)
    {
        _shootForce += _shootForce * percentage;
        _spearReturnSpeed += _spearReturnSpeed * percentage;
    }
}
