using UnityEngine;


public class ChunkManager : MonoBehaviour {
    public static ChunkManager Instance;

    public float Threshold;
    public GameObject ChunkPrefab;
    
    [HideInInspector] public Vector3 HoriStep;
    [HideInInspector] public Vector3 VerStep;
    
    [SerializeField] private float horiInterval;
    [SerializeField] private float verInterval;
    
    private void Awake() {
        if (Instance == null) Instance = this;
        
        HoriStep = Vector3.right * horiInterval;
        VerStep = Vector3.up * verInterval;
    }


    private void Start() {
        Instantiate(ChunkPrefab, transform.position, Quaternion.identity, transform);
    }
}