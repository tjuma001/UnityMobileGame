using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            // Victory
            LevelManager.Instance.Victory();
        }
    }
}
