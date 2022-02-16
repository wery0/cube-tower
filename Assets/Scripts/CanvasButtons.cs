using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{

    public Sprite musicOn, musicOff;

    private void Start() {
        if (gameObject.name == "music") {
            if (PlayerPrefs.GetString("music") == "No") {
                GetComponent<Image>().sprite = musicOff;
            } else {
                PlayerPrefs.SetString("music", "Yes");
                GetComponent<Image>().sprite = musicOn;
            }
        }
    }

    public void LoadShop() {
        if (PlayerPrefs.GetString("music") == "Yes") {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene("Shop");
    }

    public void CloseShop() {
        if (PlayerPrefs.GetString("music") == "Yes") {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene("Main");
    }

    public void RestartGame() {
        if (PlayerPrefs.GetString("music") == "Yes") {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadCodeforces() {
        if (PlayerPrefs.GetString("music") == "Yes") {
            GetComponent<AudioSource>().Play();
        }
        Application.OpenURL("https://github.com/wery0");
    }

    public void MusicWork() {
        if (PlayerPrefs.GetString("music") == "No") { //music off
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<AudioSource>().Play();
            GetComponent<Image>().sprite = musicOn;
        } else {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;

        }
    }
}
