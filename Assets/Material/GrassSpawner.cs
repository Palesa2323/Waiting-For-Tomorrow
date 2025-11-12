using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public GameObject grassPrefab;
    public int grassAmount = 1000;
    public Vector3 areaSize = new Vector3(10, 0, 10);

    [Header("Path Settings")]
    public Vector3 pathCenter = Vector3.zero;
    public Vector2 pathSize = new Vector2(2, 10); // width x length

    void Start()
    {
        for (int i = 0; i < grassAmount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                0,
                Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
            );

            // Skip spawning inside the path area
            if (Mathf.Abs(randomPos.x - pathCenter.x) < pathSize.x / 2f &&
                Mathf.Abs(randomPos.z - pathCenter.z) < pathSize.y / 2f)
            {
                continue; // skip spawning grass in this area
            }

            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            Instantiate(grassPrefab, transform.position + randomPos, randomRot, transform);
        }
    }
}
