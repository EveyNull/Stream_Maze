using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Player
{
    bool inputXPressed = false;
    bool inputYPressed = false;

    public GameObject explosion;

    override protected void ManageInputs() 
    {
        Vector3 desiredMove = Vector3.zero;
        if (!inputXPressed)
        {
            float HorizontalInput = Input.GetAxis("Horizontal");
            if (HorizontalInput != 0)
            {
                if (HorizontalInput > 0)
                {
                    desiredMove = Vector3.right;
                }
                else
                {
                    desiredMove = Vector3.left;
                }
            }
            else
            {
                inputXPressed = false;
            }
        }

        if (!inputYPressed)
        {
            float VerticalInput = Input.GetAxis("Vertical");
            if (VerticalInput != 0)
            {
                if (VerticalInput > 0)
                {
                    desiredMove = Vector3.up;
                }
                else
                {
                    desiredMove = Vector3.down;
                }
            }
            else
            {
                inputYPressed = false;
            }
        }

        if(trackedMove != Vector3.zero && desiredMove == (trackedMove * -1))
        {
            targetNode = currentNode;
        }

        if (desiredMove != Vector3.zero && currentMove != desiredMove)
        {
            currentMove = desiredMove;
            trackedMove = currentMove;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<Egg>())
        {
            Destroy(collision.collider.gameObject);
            GetComponent<AudioSource>().Play();
        }
    }

    public void Explode()
    {
        Instantiate(explosion, transform);
    }
}
