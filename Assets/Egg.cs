using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 newPos = transform.position;
        newPos.x += Random.Range(-0.1f, 0.1f);
        newPos.y += Random.Range(-0.1f, 0.1f);

        transform.position = newPos;

        float newRotAngle = Random.Range(0.0f, 360.0f);

        Quaternion newRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotAngle);

        transform.rotation = newRot;

        if(GameController.instance != null)
        {
            gameController = GameController.instance;
            gameController.RegisterEgg(this);
        }
    }

    private void OnDestroy()
    {
        if(gameController)
        {
            gameController.DeRegisterEgg(this);
        }
    }
}
