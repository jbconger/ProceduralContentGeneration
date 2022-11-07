using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;

    void Start()
    {
        int index = Random.Range(0, objects.Length);
        GameObject instance = (GameObject)Instantiate(objects[index], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
