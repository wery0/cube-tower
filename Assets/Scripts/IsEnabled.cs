using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsEnabled : MonoBehaviour
{
    public int needToUnlock;
    public Material blackMaterial;

    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < needToUnlock)
        {
            GetComponent<MeshRenderer>().material = blackMaterial;
        }
    }

}
