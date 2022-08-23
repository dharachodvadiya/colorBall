using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnPrefab;

    private List<GameObject> spawnObjectList = new List<GameObject>();

    public GameObject spawnObject(Vector3 position,Transform parent = null)
    {
        GameObject currObj = null;
        for (int i = 0; i < spawnObjectList.Count; i++)
        {
            if (!spawnObjectList[i].activeInHierarchy)
            {
                currObj = spawnObjectList[i];
                break;
            }
        }
        if(parent == null)  parent = transform;

        if (currObj == null)
        {
            currObj = Instantiate(spawnPrefab, parent);

            spawnObjectList.Add(currObj);
        }

        currObj.transform.position = position;
        currObj.SetActive(true);

        return currObj;
    }
}
