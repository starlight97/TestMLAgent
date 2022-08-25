using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAgentTest : MonoBehaviour
{
    private Rigidbody2D rBody;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        this.rBody = this.GetComponent<Rigidbody2D>();
    }

    private void Jump()
    {
        this.rBody.velocity = Vector3.up * this.jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.Jump();
        }
    }
}
