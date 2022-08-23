using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerater : MonoBehaviour
{
    public Transform PathGeneratorPoint;
    public Transform PathDistratorPoint;
    //public GameObject pathPrefab;

    //private List<GameObject> pathObjList = new List<GameObject>();
    private float lastZPos = 0;

    public Path lastPath;

    Path firstPath;

    public Spawner spawner;
    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = spawnPath();
            if (i == 0)
            {
                firstPath = obj.GetComponent<Path>();
            }
            
        }
    }

    private void Update()
    {
        if(lastPath.transform.position.z < PathGeneratorPoint.position.z)
        {
            spawnPath();
        }

        if(firstPath.pathEndPoint.transform.position.z < PathDistratorPoint.position.z)
        {
            firstPath.gameObject.SetActive(false);
            firstPath = firstPath.nextPath;
        }
    }

    public GameObject spawnPath()
    {
        /* GameObject currObj = null; 
         for (int i = 0; i < pathObjList.Count; i++)
         {
             if(!pathObjList[i].activeInHierarchy)
             {
                 currObj = pathObjList[i];
                 break;
             }
         }

         if(currObj == null)
         {
             currObj = Instantiate(pathPrefab, transform);          

             pathObjList.Add(currObj);
         }

         currObj.transform.localPosition = new Vector3(0, 0, lastZPos);
         currObj.SetActive(true);*/
        GameObject currObj = spawner.spawnObject(transform.TransformPoint(new Vector3(0, 0, lastZPos)));
        lastZPos += Random.Range(100f, 200f);
        //lastZPos += 180;

        Path currPath = currObj.GetComponent<Path>();

        if (lastPath != null)
        {
            lastPath.nextPath = currPath;
            currPath.prevPath = lastPath;
        }

        lastPath = currPath;

        return currObj;
    }
}
