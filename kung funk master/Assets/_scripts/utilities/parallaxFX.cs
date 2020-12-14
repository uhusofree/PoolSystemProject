using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using Unity.Mathematics;
using UnityEngine;

public class parallaxFX : MonoBehaviour
{
    public GameObject[] levels;
    public float choke;

    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector3 lastScreenPos;
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach (GameObject obj in levels)
        {
            LoadChildObjects(obj);
        }

        lastScreenPos = transform.position;
       
    }
   void LoadChildObjects(GameObject obj) 
    {
        float objWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objWidth);
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void RepositionChildObjects(GameObject obj) 
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x -choke;

            if (transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjWidth * 2, 
                    lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjWidth * 2, 
                    firstChild.transform.position.y, firstChild.transform.position.z);

            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        foreach (GameObject obj in levels)
        {
            RepositionChildObjects(obj);
            float parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(transform.position.z / obj.transform.position.z));
            float difference = transform.position.x - lastScreenPos.x;
            obj.transform.Translate(Vector3.right * difference *parallaxSpeed);
        }
        lastScreenPos = transform.position;
    }
}
