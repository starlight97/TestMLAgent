using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTestMain : MonoBehaviour
{
    private BirdAgentTest birdAgentTest;

    public Ground[] grounds;

    void Start()
    {
        this.birdAgentTest = transform.parent.transform.Find("BirdAgent").GetComponent<BirdAgentTest>();
        //this.birdAgentTest = GameObject.FindObjectOfType<BirdAgentTest>();

        this.birdAgentTest.onDie = () =>
        {
            birdAgentTest.transform.localPosition = new Vector3(-8, 0, 0);
            grounds[0].HidePipes();
            grounds[1].ShowPipes();

            grounds[0].gameObject.transform.localPosition = new Vector3(0, -5.7f , 0);
            grounds[1].gameObject.transform.localPosition = new Vector3(21, -5.7f , 0);
        };

        this.birdAgentTest.onSetLevel = (level)=>
        {
            foreach (var ground in grounds)
            {
                ground.SetLevel(level);
            }
        };

    }
}
