using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : MonoBehaviour
{
    public enum eDirType { Up = 1, Down = -1}

    public GameObject headGo;
    public GameObject bodyGo;
    public eDirType dirType;

    public UnityAction onSetSizeComplete;
    private Bounds bodyBounds;

    private BoxCollider2D col2D;

    private MonoBehaviour mono;
    private int level;
    public void Init()
    {
        mono = this.transform.root.Find("BirdTestMain").GetComponent<MonoBehaviour>();

    }

    public void SetSize(float height)
    {
        this.mono.StartCoroutine(SetSizeRoutine(height));
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    private IEnumerator SetSizeRoutine(float height)
    {
        var localScale = this.bodyGo.transform.localScale;
        localScale.y = (int)this.dirType * height;

        this.bodyGo.transform.localScale = localScale;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        //yield return new WaitForSeconds(0.01f);

        this.bodyBounds = this.bodyGo.GetComponent<BoxCollider2D>().bounds;
        var headPosY = (int)this.dirType * bodyBounds.extents.y * 2;
        var headPos = this.headGo.transform.localPosition;
        headPos.y = headPosY;
        this.headGo.transform.localPosition = headPos;
        this.onSetSizeComplete();
    }
}
