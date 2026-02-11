using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    
    public NavNode LeftNode { get; private set; }
    public NavNode RightNode { get; private set; }
    public NavNode UpNode { get; private set; }
    public NavNode DownNode { get; private set; }

    private void Start()
    {
        RaycastHit Hit;
        Physics.Raycast(transform.position, Vector3.left, out Hit, float.MaxValue);
        if(Hit.collider)
        {
            LeftNode = Hit.collider.GetComponent<NavNode>();
        }

        Physics.Raycast(transform.position, Vector3.up, out Hit, float.MaxValue);
        if (Hit.collider)
        {
            UpNode = Hit.collider.GetComponent<NavNode>();
        }

        Physics.Raycast(transform.position, Vector3.right, out Hit, float.MaxValue);
        if (Hit.collider)
        {
            RightNode = Hit.collider.GetComponent<NavNode>();
        }

        Physics.Raycast(transform.position, Vector3.down, out Hit, float.MaxValue);
        if (Hit.collider)
        {
            DownNode = Hit.collider.GetComponent<NavNode>();
        }
    }

    public NavNode FindNode(Vector3 intendedDir)
    {
        if(intendedDir == Vector3.zero) return null;

        if (intendedDir == Vector3.up)
        {
            return UpNode;
        }

        if (intendedDir == Vector3.down)
        {
            return DownNode;
        }

        if (intendedDir == Vector3.left)
        {
            return LeftNode;
        }

        return RightNode;
    }
}
