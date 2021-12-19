using UnityEngine;
using UnityEngine.AI;

public class FollowObject : MonoBehaviour {
    [SerializeField] Transform obj;

    // Start is called before the first frame update
    void Start() {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = obj.position;
        
    }
}
