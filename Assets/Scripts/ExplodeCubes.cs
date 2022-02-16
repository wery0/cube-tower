using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    private bool was_collision = false;
    public GameObject restartButton, explosion;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Cube" && !was_collision) {
            for (int i = collision.transform.childCount - 1; i >= 0; i--) {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(170f, Vector3.up, 50f);
                child.SetParent(null);
            }
            if (PlayerPrefs.GetString("music") == "Yes") {
                GetComponent<AudioSource>().Play();
            }
            restartButton.SetActive(true);
            Camera.main.transform.localPosition -= new Vector3(0, 0, 6);
            Camera.main.gameObject.AddComponent<CameraShake>();
            GameObject newVfx = Instantiate(explosion,
                                            new Vector3(collision.contacts[0].point.x,
                                                    collision.contacts[0].point.y,
                                                    collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            Destroy(newVfx, 2f);
            Destroy(collision.gameObject);
            was_collision = true;
        }
    }
}
