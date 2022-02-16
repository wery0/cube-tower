using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameController : MonoBehaviour
{

    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace = null;
    private float camMoveToYPosition;
    private float camMoveToZPosition;

    private bool IsLose, firstCube;
    public Text scoreText;

    public GameObject restartButton;
    public GameObject[] cubesToCreate;
    public GameObject allCubes, vfxCube;
    public GameObject[] canvasStartPage;
    private Rigidbody allCubesRb;

    private Color toCameraColor;

    private List<Vector3> allCubesPositions = new List<Vector3>();
    private Transform mainCam;
    private Coroutine showCubePlace;

    private List<GameObject> possibleCubesToCreate = new List<GameObject>();
    private int[] records = {0, 5, 10, 15, 25, 40, 55, 70, 85, 100};

    void Start() {
        for (int i = 0; i < 10; i++) {
            if (PlayerPrefs.GetInt("score") >= records[i]) {
                possibleCubesToCreate.Add(cubesToCreate[i]);
            }
        }
        scoreText.text = "<size=18><color=#F80000>Best:</color></size> " + PlayerPrefs.GetInt("score") + "\n<size=13>Now:</size> 0";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToYPosition = 3.17f + nowCube.y - 1f;
        camMoveToZPosition = -8.76f;

        allCubesRb = allCubes.GetComponent<Rigidbody>();
        allCubesPositions.Add(new Vector3(0, 1, 0));
        for (int x = -1; x <= 1; x++) {
            for (int z = -1; z <= 1; z++) {
                allCubesPositions.Add(new Vector3(x, 0, z));
            }
        }
        showCubePlace = StartCoroutine(ShowCubePlace());
    }

    IEnumerator ShowCubePlace() {
        for (;;) {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions() {
        List<Vector3> positions = new List<Vector3>();
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                for (int dz = -1; dz <= 1; dz++) {
                    if (Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz) != 1) continue;
                    Vector3 new_pos = new Vector3(nowCube.x + dx, nowCube.y + dy, nowCube.z + dz);
                    if (IsPositionEmpty(new_pos) && new_pos != cubeToPlace.position) {
                        positions.Add(new_pos);
                    }
                }
            }
        }
        if (positions.Count > 0) {
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        }
        else {
            IsLose = true;
            restartButton.SetActive(true);
            Debug.Log("PIZDA");
        }
    }

    private bool IsPositionEmpty(Vector3 pos) {
        if (pos.y <= 0) return false;
        foreach (Vector3 cur in allCubesPositions) {
            if (pos == cur) return false;
        }
        return true;
    }

    private void Update() {
        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && cubeToPlace != null && allCubes != null
                && !EventSystem.current.IsPointerOverGameObject()) {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif
            if (!firstCube) {
                firstCube = true;
                foreach (GameObject i in canvasStartPage) {
                    Destroy(i);
                }
            }
            GameObject newCube = Instantiate(possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)],
                                             cubeToPlace.position, Quaternion.identity) as GameObject;
            newCube.transform.SetParent(allCubes.transform);
            newCube.transform.localScale = new Vector3(1, 1, 1);
            nowCube.SetVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube.GetVector());
            if (PlayerPrefs.GetString("music") == "Yes") {
                GetComponent<AudioSource>().Play();
            }
            GameObject newVfx = Instantiate(vfxCube, cubeToPlace.position, Quaternion.identity) as GameObject;
            Destroy(newVfx, 3f);
            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;
            SpawnPositions();
            MoveCameraChangeBg();
        }
        if (!IsLose && allCubesRb.velocity.magnitude > 0.2f) {
            Destroy(cubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(showCubePlace);
        }
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
                                new Vector3(mainCam.localPosition.x, camMoveToYPosition, camMoveToZPosition),
                                2f * Time.deltaTime);
        if (Camera.main.backgroundColor != toCameraColor) {
            Camera.main.backgroundColor = Color.Lerp(
                                              Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 2f
                                          );
        }
    }

    private float histMaxY;
    private float deltaColor = 0.2f;
    private void MoveCameraChangeBg() {
        int maxX = 0, maxY = 0, maxZ = 0;
        foreach (Vector3 pos in allCubesPositions) {
            maxX = Math.Max(maxX, (int)Math.Abs(pos.x));
            maxY = Math.Max(maxY, (int)Math.Abs(pos.y));
            maxZ = Math.Max(maxZ, (int)Math.Abs(pos.z));
        }

        if (PlayerPrefs.GetInt("score") < maxY - 1) {
            PlayerPrefs.SetInt("score", maxY - 1);
        }
        scoreText.text = "<size=18><color=#F80000>Best:</color></size> " + PlayerPrefs.GetInt("score") + "\n<size=13>Now:</size> " + (maxY - 1);

        int maxHor = Math.Max(maxX, maxZ);
        camMoveToYPosition = 3.17f + nowCube.y - 1f;
        camMoveToZPosition = -8.76f - maxHor * 2f;
        if (maxY > histMaxY) {
            histMaxY = maxY;
            float[] rgb = {Camera.main.backgroundColor.r, Camera.main.backgroundColor.g, Camera.main.backgroundColor.b};
            for (int q = 0; q < 3; q++) {
                rgb[q] = UnityEngine.Random.Range(Math.Max(0f, rgb[q] - deltaColor), Math.Min(1f, rgb[q] + deltaColor));
            }
            toCameraColor = new Color(rgb[0], rgb[1], rgb[2]);
        }
    }
}

struct CubePos {
    public int x, y, z;

    public CubePos(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector() {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 pos) {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}
