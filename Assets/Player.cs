using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected Vector3 trackedMove = Vector3.zero;
    protected Vector3 currentMove = Vector3.zero;
    protected Vector3 nextMove = Vector3.zero;


    protected NavNode currentNode;
    protected NavNode targetNode;

    private GameController gameController;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        NavNode[] NavNodes = GameObject.FindObjectsOfType<NavNode>();
        foreach(NavNode navNode in NavNodes)
        {
            if(navNode.transform.position == transform.position)
            {
                currentNode = navNode;
            }
        }
        if(GameController.instance != null)
        {
            gameController = GameController.instance;
        }
    }

    protected virtual void Update()
    {
        if(!gameController || !gameController.gameRunning)
        {
            return;
        }
        ManageInputs();

        bool recalculateDir = false;
        if(targetNode)
        {
            if(targetNode.transform.position == transform.position)
            {
                currentNode = targetNode;
                recalculateDir = true;
            }
        }

        if(!targetNode)
        {
            recalculateDir = true;
        }

        if(recalculateDir)
        {
            if (nextMove != Vector3.zero)
            {
                NavNode testNode = currentNode.FindNode(nextMove);
                if(testNode != null)
                {
                    currentMove = nextMove;
                    trackedMove = currentMove;
                    nextMove = Vector3.zero;
                }
            }
            targetNode = currentNode.FindNode(currentMove);
        }

        if (targetNode)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, Time.deltaTime);
        }
    }

    virtual protected void ManageInputs()
    {
    }

   
}
