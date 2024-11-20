using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerScript : MonoBehaviour
{
    public NavMeshAgent nma;
    BuildScript targetBuilding;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void moveTowardsBuilding(BuildScript target)
    {
        targetBuilding = target;
        if (targetBuilding != null)
        {
            nma.SetDestination(targetBuilding.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetBuilding.tag))
        {
            targetBuilding.expectedWorkers--;
            targetBuilding.currentWorkers += 1;
            Destroy(transform.gameObject);
        }
    }
}
