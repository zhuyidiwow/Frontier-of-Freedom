using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    [SerializeField] private GameObject cube;

    private void Start() {
        GenerateCubes();
    }

    void GenerateCubes() {
        Vector3 startPosition = new Vector3(3.5f, -3f, 0f);
        float x = cube.GetComponent<Renderer>().bounds.size.x;
        float y = cube.GetComponent<Renderer>().bounds.size.y;

        Vector3 horiStep = x * Vector3.right;
        Vector3 verStep = y * Vector3.up;
        
        for (int hori = 0; hori < 10; hori++) {
            for (int ver = 0; ver < 2; ver++) {
                Instantiate(cube, startPosition - horiStep * hori + verStep * ver, Quaternion.identity, transform);
            }
        }
    }

}