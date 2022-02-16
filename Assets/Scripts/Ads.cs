using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{

    private Coroutine showAd;
    private string gameId = "4134022", type = "video";
    private bool testMode = true;

    private static int countLoses;

    private void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        countLoses++;
        if (countLoses % 3 == 2) {
            showAd = StartCoroutine(ShowAd());
        }
    }

    IEnumerator ShowAd() {
        for (; ; ) {
            if (Advertisement.IsReady(type)) {
                Debug.Log("Ready");
                Advertisement.Show(type);
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
