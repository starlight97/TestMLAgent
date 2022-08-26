using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public GameObject body;
    public Transform point;
    public Transform point2;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var renderer = body.GetComponent<SpriteRenderer>().sprite;
            var posy = renderer.rect.height / renderer.pixelsPerUnit;
            var tpos = point2.transform.position;
            tpos.y = body.transform.localScale.y * posy;
            this.point2.position = tpos;

            var dis = Vector3.Distance(point2.position, point.position);
            Debug.Log(dis);
        }

    }

}
