using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePair : MonoBehaviour
{
    // MAX 100

    private float[,] heightRange;
    private static float MAX = 100f;
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
        this.heightRange[2, 0] = 70f;
        this.heightRange[2, 1] = MAX;

    }

    private void Start()
    {
        this.Init();
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
        var height = Random.Range(this.heightRange[0, 0], this.heightRange[0, 1]);

        // 50
        var bottomHeight = Random.Range(10f, height);
        // 115 - topHeight 
        // 50 - topHeight?
        var topHeight = height - bottomHeight;

        Debug.Log(bottomHeight);
        Debug.Log(topHeight);
        this.pipeBottom.SetSize(bottomHeight);
        this.pipeTop.SetSize(topHeight);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.SetSizePipePair(0);
        }
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
