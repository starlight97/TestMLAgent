using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePair : MonoBehaviour
{
    // MAX 115까지 가능하나 억까패턴때매 난이도 조절중

    private float[,] heightRange;
   
    private static float MAX = 80f;
    public Pipe pipeBottom;
    public Pipe pipeTop;

    private MonoBehaviour mono;
    public void Init()
    {
        mono = this.transform.root.GetComponent<MonoBehaviour>();

        this.heightRange = new float[3, 2];
        this.heightRange[0, 0] = 30f;
        this.heightRange[0, 1] = MAX;
        this.heightRange[1, 0] = 50f;
        this.heightRange[1, 1] = MAX;
        this.heightRange[2, 0] = 60f;
        this.heightRange[2, 1] = MAX;

        this.pipeBottom.Init();
        this.pipeTop.Init();

        this.pipeBottom.onSetSizeComplete = () =>
        {
            this.pipeBottom.Show();
        };
        this.pipeTop.onSetSizeComplete = () =>
        {
            this.pipeTop.Show();
        };

    }

    public void SetSizePipePair(int level)
    {
        // 50 ~ 105
        var height = Random.Range(this.heightRange[level, 0], this.heightRange[level, 1]);
        // 50
        var bottomHeight = Random.Range(10f, height);
        // 115 - topHeight 
        // 50 - topHeight?
        var topHeight = height - bottomHeight;

        this.pipeBottom.SetSize(bottomHeight);
        this.pipeTop.SetSize(topHeight);
    }

    public void Show(int level)
    {
        this.pipeBottom.Show();
        this.pipeTop.Show();

    }

    public void Hide()
    {
        this.pipeBottom.Hide();
        this.pipeTop.Hide();
    }

}
