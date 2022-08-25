using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.left * this.moveSpeed * Time.deltaTime);
        if(this.transform.localPosition.x <= -21f)
        {
            var pos = this.transform.localPosition;
            pos.x = 21f;
            this.transform.localPosition = pos;
        }
    }
}
