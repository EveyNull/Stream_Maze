using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchPlayer : Player
{
    public MeshRenderer avatarRenderer;

    private Vector3 queuedMove;
    private bool updateAvatar = false;
    private Coroutine getAvatarCoroutine;

    public string username { get; private set; }

    override protected void Start()
    {
        base.Start();

    }

    override protected void Update()
    {
        if(updateAvatar)
        {
            ManageAvatar();
        }

        base.Update();
    }

    private void ManageAvatar()
    {
        if (getAvatarCoroutine == null)
        {
            getAvatarCoroutine = StartCoroutine(HTTPHelper.GetAvatar(username, OnGetAvatar));
        }
    }

    private void OnGetAvatar(Texture2D newAvatar)
    {
        if(newAvatar && avatarRenderer)
        {
            avatarRenderer.material.mainTexture = newAvatar;
            updateAvatar = false;
            getAvatarCoroutine = null;
        }
    }

    public void NewInput(string input)
    {
        input = input.Trim();
        input = input.ToLower();

        if(input == "left")
        {
            queuedMove = Vector3.left;
        }

        if (input == "right")
        {
            queuedMove = Vector3.right;
        }

        if (input == "up")
        {
            queuedMove = Vector3.up;
        }

        if (input == "down")
        {
            queuedMove = Vector3.down;
        }
    }
    protected override void ManageInputs()
    {
        if (queuedMove == Vector3.zero) return;

        Vector3 newMove = queuedMove;

        if (queuedMove == Vector3.left || queuedMove == Vector3.right)
        {
            if (newMove != Vector3.zero)
            {
                if (currentMove != Vector3.up && currentMove != Vector3.down)
                {
                    currentMove = newMove;
                }
                else
                {
                    nextMove = newMove;
                }
                queuedMove = Vector3.zero;
                return;
            }
        }

        if (currentMove != Vector3.left && currentMove != Vector3.right)
        {
            currentMove = newMove;
        }
        else
        {
            nextMove = newMove;
        }
    }

    public void SetName(string newName)
    {
        this.username = newName;
        updateAvatar = true;
    }

    private IEnumerator RandomMoves()
    {
        List<string> Inputs = new List<string>
        {
            "up",
            "down",
            "left",
            "right"
        };

        while (true)
        {
            NewInput(Inputs[Random.Range(0, Inputs.Count)]);

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        MainPlayer moraPlayer = collision.collider.GetComponent<MainPlayer>();
        if (moraPlayer)
        {
            if(GameController.instance)
            {
                GameController.instance.CatchMora(this);
            }
            moraPlayer.Explode();
        }
    }
}
