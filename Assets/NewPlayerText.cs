using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewPlayerText : MonoBehaviour
{
    public TMP_Text nameText;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }
}
