using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AICharacterControlState //en public et en dehors de la clase sinon pas utilisable dans d'autre classe
{
    Patrolling,
    Chasing
}

public class AICharacterControl : MonoBehaviour
{
    [SerializeField] private AICharacterControlState state; 
    public Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    [Tooltip("put the parent Transform of all the waypoint that you want the character to visit here")]public Transform waypointGroup;

    private Transform currentWaypoint;

    //developpement (débug)

    //Space(10) va mettre un espace de 10 pixel pour séparé le débug du reste des variable en serialize fiel ou public
    [SerializeField, Space(10), Header("Dev Tools")] private bool debug; //si on active le debug dans l'inspector unity ça va activer le debug log qu'on a fait 
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentWaypoint = SelectDestination();
        navMeshAgent.SetDestination(currentWaypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case AICharacterControlState.Patrolling:
                if(CheckPlayerVisibility())
                {
                    state = AICharacterControlState.Chasing;
                }
                break;
            case AICharacterControlState.Chasing:
                navMeshAgent.SetDestination(playerTransform.position);
                if(!CheckPlayerVisibility())
                {
                    state = AICharacterControlState.Patrolling;
                    //on lui dit de retourné au waypoint qu'il était
                    navMeshAgent.SetDestination(currentWaypoint.position);
                }
                break;
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.transform == currentWaypoint){
            currentWaypoint = SelectDestination();
            navMeshAgent.SetDestination(currentWaypoint.position);
        }
    }
    Transform SelectDestination(){
        int index = Random.Range(0, waypointGroup.childCount);
        Transform newWaypoint = waypointGroup.GetChild(index);
        //le while va farie en sorte que si on a tombe sur le même waypoint que celui sur lequel on est il en choissise un autre
        while(newWaypoint == currentWaypoint){
            index = Random.Range(0, waypointGroup.childCount);
            newWaypoint = waypointGroup.GetChild(index);
        }

        return newWaypoint;

    }
    bool CheckPlayerVisibility(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, playerTransform.position - transform.position, out hit, 10f)){
            
            if (debug)Debug.Log($"[AICharacterControl] Raycast hit something : {hit.collider.name}"); // si le bool debug est active ça va envoyer le debug

             // si le player est visible dans les 10 float de distance
            if (hit.collider.CompareTag("Player")){ // si il a touché le player
                if(Vector3.Angle(transform.forward, playerTransform.position - transform.position)< 45){ // si le player est dans les 45° a gauche ou a droite du guard il le voit
                    return true;
                }
            }
        }
        return false;
    }
}
