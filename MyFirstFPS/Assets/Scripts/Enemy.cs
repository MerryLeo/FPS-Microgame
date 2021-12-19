using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
    [SerializeField] 
    LayerMask enemyLayerMask;
    [SerializeField]
    LayerMask playerLayerMask;
    [SerializeField]
    GameObject headObj;
    [SerializeField]
    EnemyGun currentGun;

    Vector3 _targetForwardVector, _currentForwardVector;
    float _count;
    GameObject _playerObj;
    PlayerStatus _playerStatusScript;
    NavMeshAgent _agent;
    const float _maximumViewDst = 45f, _maximumShootDst = 50f, _rotateSpeed = 0.01f;
    //const float _knockbackResist = 1.25f;
    public bool IsAwake { get; private set; } = false;

    void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _playerObj = GameObject.FindGameObjectWithTag("Player");
        _playerStatusScript = _playerObj.GetComponent<PlayerStatus>();
    }

    void Update() 
    {
        float distance = Vector2.Distance(transform.position, _playerObj.transform.position);
        if (!_playerStatusScript.IsDead) 
        {
            if (IsAwake) 
            {
                // Rotate
                _targetForwardVector = new Vector3(_playerObj.transform.position.x - transform.position.x, 0, _playerObj.transform.position.z - transform.position.z);
                if (_targetForwardVector != _currentForwardVector) 
                {
                    _currentForwardVector = Vector3.Lerp(_currentForwardVector, _targetForwardVector, _count);
                    _count += _rotateSpeed * Time.deltaTime;
                } 
                else if (_count != 0) 
                    _count = 0;
                
                transform.forward = _currentForwardVector;

                // Move
                _agent.destination = _playerObj.transform.position;

                // Shoot
                if (currentGun != null && currentGun.AmmunitionInClip > 0 && distance <= _maximumShootDst) 
                {
                    currentGun.ShootAt(_playerObj.transform.position);
                } 
                
                // Reload
                else if (currentGun.AmmunitionInClip <= 0) 
                    currentGun.Reload();
                
            } 
            else if (distance <= _maximumViewDst) 
            {
                Ray ray = new Ray(transform.position, _playerObj.transform.position - transform.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Player") 
                    WakeUp();
            }
        }
    }

    //public void Knockback(Vector3 dir, float str)
    //{
    //    if (!_agent.isActiveAndEnabled)
    //    {
    //        _agent.enabled = true;
    //    }
    //    _agent.Move(dir.normalized * (str / _knockbackResist));
    //}

    public void WakeUp() {
        _currentForwardVector = transform.forward;
        _count = 0f;
        IsAwake = true;
    }

}
