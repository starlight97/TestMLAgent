using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public enum eDirType { Up = 1, Down = -1}

    private float[,] heightRange; 

    public GameObject headGo;
    public GameObject bodyGo;
    public eDirType dirType;
    private Bounds bodyBounds;

    private BoxCollider2D col2D;

    private MonoBehaviour mono;
    private int level;
    public void Init()
    {
        mono = this.transform.parent.GetComponent<MonoBehaviour>();

        this.heightRange = new float[3, 2];
        this.heightRange[0, 0] = 10f;
        this.heightRange[0, 1] = 30f;
        this.heightRange[1, 0] = 30f;
        this.heightRange[1, 1] = 40f;
        this.heightRange[2, 0] = 40f;
        this.heightRange[2, 1] = 55f;

    }
    private void OnEnable()
    {
        if (mono != null)
            Show(this.level);
    }
    public void Show(int level)
    {
        this.mono.StartCoroutine(ShowRoutine(level));
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator ShowRoutine(int level)
    {
        var localScale = this.bodyGo.transform.localScale;
        var randHeight =  Random.Range(this.heightRange[level,0], this.heightRange[level, 1]);
        localScale.y = (int)this.dirType * randHeight;

        this.bodyGo.transform.localScale = localScale;
        yield return null;

        //yield return new WaitForSeconds(0.01f);

        this.bodyBounds = this.bodyGo.GetComponent<BoxCollider2D>().bounds;
        var headPosY = (int)this.dirType * bodyBounds.extents.y * 2;
        var headPos = this.headGo.transform.localPosition;
        headPos.y = headPosY;
        this.headGo.transform.localPosition = headPos;

        this.gameObject.SetActive(true);
    }
}
