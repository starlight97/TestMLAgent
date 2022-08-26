using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float moveSpeed;

    public PipePair[] arrPipePairs;

    private int level;

    // UP : 5.6
    // DOWN : -1

    public bool isShow;
    void Start()
    {
        foreach (var pipePair in arrPipePairs)
        {
            pipePair.Init();
        }

        if (this.isShow)
            this.ShowPipes();
        else
            this.HidePipes();
    }

    public void HidePipes()
    {
        foreach (var pipePair in arrPipePairs)
        {
            pipePair.Hide();
        }
    }



    public void ShowPipes()
    {
        foreach (var pipePair in arrPipePairs)
        {
            pipePair.SetSizePipePair(this.level);
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    //private void FixedUpdate()
    //{        
    //    this.transform.Translate(Vector3.left * this.moveSpeed * Time.fixedDeltaTime);
    //    if (this.transform.localPosition.x <= -24.04f)
    //    {
    //        var pos = this.transform.localPosition;
    //        pos.x = 24.04f;
    //        this.transform.localPosition = pos;

    //        ShowPipes();
    //    }
    //}
    private void Update()
    {
        this.transform.Translate(Vector3.left * this.moveSpeed * Time.deltaTime);
        if (this.transform.localPosition.x <= -24.04f)
        {
            var pos = this.transform.localPosition;
            pos.x = 24.04f;
            this.transform.localPosition = pos;

            ShowPipes();
        }
    }

}
