using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float moveSpeed;

    public Pipe[] arrPipes;

    private int level;

    // UP : 5.6
    // DOWN : -1

    public bool isShow;
    void Start()
    {
        foreach (var pipe in arrPipes)
        {
            pipe.Init();
        }

        if (this.isShow)
            this.ShowPipes();
        else
            this.HidePipes();
    }

    public void HidePipes()
    {
        foreach (var pipe in arrPipes)
        {
            pipe.Hide();
        }
    }



    public void ShowPipes()
    {
        foreach (var pipe in arrPipes)
        {
            pipe.Show(this.level);
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    
    void Update()
    {
        this.transform.Translate(Vector3.left * this.moveSpeed * Time.deltaTime);
        if(this.transform.localPosition.x <= -22.0f)
        {
            var pos = this.transform.localPosition;
            pos.x = 20f;
            this.transform.localPosition = pos;

            ShowPipes();
        }
    }
}
